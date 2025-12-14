using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                throw new Exception(ex.Message.ToString());
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
                throw new Exception(ex.Message.ToString());
            }
        }

        [HttpGet("TaskById/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _context.Tasks.Where(p => p.Id == id).ToListAsync();
                if(task != null)
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
                throw new Exception(ex.Message.ToString());
            }
        }

        [HttpPut("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updateTask)
        {
            try
            {
                var task = await _context.Tasks.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (task != null)
                {
                    task.TaskTitle = updateTask.TaskTitle;
                    task.TaskDescription = updateTask.TaskDescription;
                    task.TaskStatus = updateTask.TaskStatus;
                    task.TaskDueDate = updateTask.TaskDueDate;
                    task.TaskRemarks = updateTask.TaskRemarks;

                    await _context.SaveChangesAsync();
                    return Ok(task);
                }
                else
                {
                    return NotFound(new { message = "Task Not Found" });
                }
            }
            catch(Exception ex)
            {
                throw new Exception (ex.Message.ToString());
            }
        }

        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {

                var task = await _context.Tasks.Where(p =>p.Id == id).FirstOrDefaultAsync();
                if (task != null)
                {
                    _context.Tasks.Remove(task);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Task Deleted" });
                }
                else
                {
                    return NotFound(new { message = "Task Not Found" });
                }
            }
            catch(Exception ex)
            {
                throw new Exception (ex.Message.ToString());
            }

        }
    }
}
