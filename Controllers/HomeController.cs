using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskLabBackend.Db;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet("AllTasks")]
        public async Task<IActionResult> AllTasks()
        {
            var allTasks = await _context.Tasks.ToListAsync(); 
            if (allTasks.Any())
            {
                  return Ok(allTasks);

            }
            else
            {
                return Ok(new { message = "No Task Found" });
            }
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask()
        {

        }
    }
}
