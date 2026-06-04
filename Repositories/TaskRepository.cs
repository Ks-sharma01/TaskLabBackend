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

        public async Task<IEnumerable<Models.Task>> GetAllTasks()
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
                CreatedOn = DateTime.UtcNow,
                TeamMemberId = tasksDto.TeamMemberId,
                
                
                //TeamMember = 
            };
           await _context.Tasks.AddAsync(Task);
           await _context.SaveChangesAsync();
           return Task;
          
        }

        public async Task<Models.Task> GetTaskById(int id)
        {
            return await _context.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Models.Task>> SearchTask(string keyword)
        {
          return await _context.Tasks.Where(x => x.TaskTitle.Contains(keyword)).ToListAsync();
           
        }

        public async Task<Models.Task> UpdateTask(int id, UpdateTaskDto updateTask)
        {
            var tasks = await _context.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (tasks != null)
            {

                tasks.TaskTitle = updateTask.TaskTitle;
                tasks.TaskDescription = updateTask.TaskDescription;
                tasks.TaskDueDate = updateTask.TaskDueDate;
                tasks.TaskStatus = updateTask.TaskStatus;
                tasks.TaskRemarks = updateTask.TaskRemarks;
            };
            _context.Tasks.Update(tasks);
            await _context.SaveChangesAsync();
            return tasks;
            
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
