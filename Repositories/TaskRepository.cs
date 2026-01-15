using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;
using TaskLabBackend.Models;

namespace TaskLabBackend.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<IEnumerable<Models.Task>> GetAllTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Models.Task> AddTask(TasksDto tasksDto)
        {
            var Task = new Models.Task
            {
                TaskTitle = tasksDto.TaskTitle,
                TaskDescription = tasksDto.TaskDescription,
                TaskDueDate = tasksDto.TaskDueDate,
                TaskStatus = tasksDto.TaskStatus,
                TaskRemarks = tasksDto.TaskRemarks,
            };
            _context.Tasks.Add(Task);
           await _context.SaveChangesAsync();
            return Task;
          
        }

        public async Task<Models.Task> GetTaskById(int id)
        {
            return await _context.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Models.Task> SearchTask(string keyword)
        {
          return  await _context.Tasks.Where(x => x.TaskTitle.Contains(keyword)).FirstOrDefaultAsync();
           
        }

        public async Task<Models.Task> UpdateTask(Models.Task task)
        {
            var Task = new Models.Task
            {
                TaskTitle = task.TaskTitle,
                TaskDescription = task.TaskDescription,
                TaskDueDate = task.TaskDueDate,
                TaskStatus = task.TaskStatus,
                TaskRemarks = task.TaskRemarks,
            };
            _context.Tasks.Update(Task);
           await _context.SaveChangesAsync();
            return Task;
            
        }

        public async Task<Models.Task> DeleteTask(int id)
        {
            var task = await _context.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(task != null)
            {
                _context.Tasks.Remove(task);
               await _context.SaveChangesAsync();
            }
            return task;
        }
    }
}
