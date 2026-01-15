using TaskLabBackend.Dto;
using TaskLabBackend.Models;

namespace TaskLabBackend.Repositories
{
    public interface ITaskRepository
    {
       Task<IEnumerable<Models.Task>> GetAllTasks();
        Task<Models.Task> AddTask(TasksDto tasksDto);
       Task<Models.Task> GetTaskById(int id);
        Task<Models.Task> SearchTask(string keyword);
        Task<Models.Task> UpdateTask(int id, UpdateTaskDto updateTask);
        Task<Models.Task> DeleteTask(int id);
    }
}
