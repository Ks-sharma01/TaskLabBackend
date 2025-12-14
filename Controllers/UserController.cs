using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(RegisterDto register) 
        //{
        //    try
        //    {
        //        var user = new Models.User()
        //        {
        //            Name = register.Name,
        //            Email = register.Email,
        //            Password = register.Password,
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //}
    }
}
