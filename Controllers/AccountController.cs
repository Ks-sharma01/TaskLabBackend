using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskLabBackend.Models.Api;
using TaskLabBackend.Services;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtConfigure jwtConfigure;

        public AccountController(JwtConfigure jwtConfigure)
        {
            this.jwtConfigure = jwtConfigure;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel loginRequest)
        {
            var result = await jwtConfigure.Authenticate(loginRequest);
           if (result == null)
            {
                return Unauthorized();
            }
           return Ok(result);
        }
    }
}
