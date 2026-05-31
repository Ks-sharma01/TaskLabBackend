using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;
using TaskLabBackend.Models;
using TaskLabBackend.Repositories;
using TaskLabBackend.Services.Redis;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("SlidingWindowPolicy")]
    public class HomeController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IRedisCacheService _cache;
        public HomeController(ITaskRepository taskRepository, IRedisCacheService cache)
        {
            _taskRepository = taskRepository;
            _cache = cache;
        }
        [HttpGet("AllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = _cache.GetData<IEnumerable<TaskLabBackend.Models.Task>>("tasks");
                if (tasks is not null)
                {
                    return Ok(tasks);
                }
                tasks =  await _taskRepository.GetAllTasks();
                _cache.SetData("tasks", tasks);
               return Ok(tasks);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] TasksDto tasksDto)
        {
            try
            {
                var task = await _taskRepository.AddTask(tasksDto);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                }); 
            }
        }

        [HttpGet("SearchTask")]
        public async Task<IActionResult> SearchTask(string keyword)
        {
            try
            {
              var task = _cache.GetData<IEnumerable<TaskLabBackend.Models.Task>>("searchedtasks");
                if(task is not null)
                {
                    return Ok(task);
                }

               task = await _taskRepository.SearchTask(keyword);
               _cache.SetData("searchedtasks", task);
                return Ok(task);

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
               var task = await _taskRepository.GetTaskById(id);
                return Ok(task);

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
                var task = await _taskRepository.UpdateTask( id, updateTask);
                return Ok(task);
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
               var task = await _taskRepository.DeleteTask(id);
                return Ok(task);
            }
            catch(Exception ex)
            {
                throw new Exception (ex.Message.ToString());
            }

        }

        
    }
}
