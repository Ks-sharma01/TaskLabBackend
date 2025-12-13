using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;

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
            try
            {

            var allTasks = await _context.Tasks.ToListAsync(); 
            if (allTasks.Any())
            {
                  return Ok(allTasks);

            }
            else
            {
                return NotFound(new { message = "No Task Found" });
            }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString(), ex);
            }
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask(TasksDto tasksDto)
        {
            try
            {

            var Tasks = new Models.Task()
            {
                TaskTitle = tasksDto.TaskTitle,
                TaskDescription = tasksDto.TaskDescription,
                TaskStatus = tasksDto.TaskStatus,
                TaskRemarks = tasksDto.TaskRemarks,
                TaskDueDate = tasksDto.TaskDueDate,
            };
                _context.Tasks.Add(Tasks);
                await _context.SaveChangesAsync();
                return Ok(Tasks);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString(), ex);
            }
        }

        [HttpGet("TaskById/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _context.Tasks.Where(p => p.Id == id).ToListAsync();
                if(task.Count > 0)
                {
                    return Ok(task);
                }
                else
                {
                    return NotFound(new { message = "Task Not Found" });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString(), ex);
            }
        }
    }
}
