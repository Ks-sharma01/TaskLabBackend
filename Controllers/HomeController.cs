using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;
using TaskLabBackend.Repositories;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public HomeController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("AllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
               var task =  await _taskRepository.GetAllTasks();
               return Ok(task);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            
        }

        [HttpPost("AddTask")]
        public  IActionResult AddTask([FromBody] TasksDto tasksDto)
        {
            try
            {
                var task = _taskRepository.AddTask(tasksDto);
                return Ok(task);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        [HttpGet("SearchTask")]
        public async Task<IActionResult> SearchTask(string keyword)
        {
            try
            {
              var task=  await _taskRepository.SearchTask(keyword);
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
