using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;
using TaskLabBackend.Models;
using TaskLabBackend.Models.Api;
using TaskLabBackend.Services;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtConfigure jwtConfigure;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration _configuration;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;


        public AccountController(JwtConfigure jwtConfigure, ApplicationDbContext context, IConfiguration configuration, IOtpService otpService, IEmailService emailService)
        {
            this.jwtConfigure = jwtConfigure;
            this.context = context;
            this._configuration = configuration;
            this._otpService = otpService;
            this._emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel loginRequest)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);
            if (user == null) return BadRequest("User not found");

            var result = await jwtConfigure.Authenticate(loginRequest);
            var refreshToken = jwtConfigure.GenerateRefreshToken();

            context.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = refreshToken,
                    UserId = Convert.ToString(result.Id),
                    ExpiryDate = DateTime.UtcNow.AddDays(
                        Convert.ToDouble(_configuration["JwtConfig:RefreshTokenDays"]))
                }
            );

            await context.SaveChangesAsync();

           if (result == null || refreshToken == null)
            {
                return Unauthorized();
            }
           return Ok(new
           {
               result,
               refreshToken
           });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto, string otp)
        {
            var user = await context.Users.FirstOrDefaultAsync(p => p.Email == registerDto.Email);
            if (user != null)
            {
                return BadRequest(new { message = "Already Registered" });
            }

            var otpEntry = await context.OtpRequests.Where(x => x.UserId == user.Id && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            if (otpEntry == null || otpEntry.ExpiryTime < DateTime.UtcNow) return BadRequest("OTP Expired");

            if (!_otpService.VerifyOtp(otp, otpEntry.OtpHash)) return BadRequest("Invalid OTP");

            otpEntry.IsUsed = true;

            await context.SaveChangesAsync();

            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
            };

            context.Add(newUser);
            await context.SaveChangesAsync();

            return Ok(newUser);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshToken refreshToken)
        {
            var storedToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == refreshToken.Id && !x.IsExpired);

            var user = await context.Users.FindAsync(storedToken.UserId);
            if(storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return Unauthorized();
            }
            storedToken.IsExpired = true;

            var newAccessToken = jwtConfigure.GenerateAccessToken(user);
            var newRefreshToken = jwtConfigure.GenerateRefreshToken();

            context.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = storedToken.UserId,
                ExpiryDate = DateTime.UtcNow.AddDays(7)

            });

            await context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return BadRequest("User not found");

            var otp = _otpService.GenerateOtp();

            var optEntry = new OtpRequest
            {
                UserId = user.Id,
                OtpHash = _otpService.HashOtp(otp),
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,

            };
            context.OtpRequests.Add(optEntry);
            await context.SaveChangesAsync();
            await _emailService.SendOtpEmailAsync(user.Email, otp);
            return Ok(new { otp = $"OTP Sent to {user.Email}" });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(string Email, string otp)
        {
            var user = await context.Users.FirstOrDefaultAsync(p => p.Email == Email);
            if (user == null) return BadRequest("Invalid Email");


            var otpEntry = await context.OtpRequests.Where(x => x.UserId == user.Id && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            if (otpEntry == null || otpEntry.ExpiryTime < DateTime.UtcNow) return BadRequest("OTP Expired");

            if (!_otpService.VerifyOtp(otp, otpEntry.OtpHash)) return BadRequest("Invalid OTP");

            otpEntry.IsUsed = true;

            await context.SaveChangesAsync();

            return Ok("Otp Verified");
        }

    }
}
