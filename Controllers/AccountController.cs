using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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


        public AccountController(JwtConfigure jwtConfigure, ApplicationDbContext context, IConfiguration configuration)
        {
            this.jwtConfigure = jwtConfigure;
            this.context = context;
            this._configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel loginRequest)
        {
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
    }
}
