using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
        [EnableRateLimiting("login-policy")]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel loginRequest)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == loginRequest.Email && x.Password == loginRequest.Password);
            if (user == null) return BadRequest("User not found");

            var result = await jwtConfigure.Authenticate(user.Email, user.Password);

            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(p => p.Email == registerDto.Email);
            if (user != null)
            {
                return BadRequest(new { message = "Already Registered" });
            }


            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
                CreatedAt = DateTime.UtcNow,
            };

            context.Add(newUser);
            await context.SaveChangesAsync();

            return Ok(newUser);
        }

        //[HttpPost("refresh")]
        //public async Task<IActionResult> Refresh([FromBody] RefreshToken refreshToken)
        //{
        //    var storedToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == refreshToken.Id && !x.IsExpired);

        //    var user = await context.Users.FindAsync(storedToken.UserId);
        //    if(storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
        //    {
        //        return Unauthorized();
        //    }
        //    storedToken.IsExpired = true;

        //    var newAccessToken = jwtConfigure.GenerateAccessToken(user);
        //    var newRefreshToken = jwtConfigure.GenerateRefreshToken();

        //    context.RefreshTokens.Add(new RefreshToken
        //    {
        //        Token = newRefreshToken,
        //        UserId = storedToken.UserId,
        //        ExpiryDate = DateTime.UtcNow.AddDays(7)

        //    });

        //    await context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        AccessToken = newAccessToken,
        //        RefreshToken = newRefreshToken,
        //    });
        //}

        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (user == null) return BadRequest("User not found");

            var existingOtp = context.OtpRequests.Where(x => x.UserId == user.Id && !x.IsUsed);
            foreach(var item in existingOtp)
            {
                item.IsUsed = true;
            }
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
            try
            {
               await _emailService.SendOtpEmailAsync(user.Email, otp);

            }
            catch
            {
                return StatusCode(500, "Failed to send OTP email");
            }
            return Ok(new { msg= $"OTP Sent to {user.Email}" });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(p => p.Email == verifyOtpDto.Email);
            if (user == null) return BadRequest("Invalid Email");

                

            var otpEntry = await context.OtpRequests.Where(x => x.UserId == user.Id && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            if (otpEntry == null || otpEntry.ExpiryTime < DateTime.UtcNow) return BadRequest(new { msg= "OTP Expired"});

            if (!_otpService.VerifyOtp(verifyOtpDto.Otp, otpEntry.OtpHash)) return BadRequest(new { msg = "Invalid OTP" });

            otpEntry.IsUsed = true;

            await context.SaveChangesAsync();
            var result = await jwtConfigure.Authenticate(user.Email, user.Password);


            return Ok(new { msg = "Otp Verified", token= result.AccessToken});
        }

        //public async Task<IActionResult> GoogleSignup([FromBody] GoogleSignupDto googleSignupDto)
        //{
        //    var payload = await GoogleJsonWebSignature.ValidateAsync(googleSignupDto.IdToken);
        //    var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);

        //    if (existingUser != null)
        //    {
        //        return BadRequest("User already exists. Please login.");
        //    }
        //    var user = new User
        //    {
        //        Name = payload.Name,
        //        Email = payload.Email,
        //        Provider = "Google",
        //        IsEmailVerified = true
        //    }
        //}

    }
}
