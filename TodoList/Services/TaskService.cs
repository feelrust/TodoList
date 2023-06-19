using System;
using Microsoft.EntityFrameworkCore;
using TodoList.Entity;

namespace TodoList.Services
{
	public interface ITaskService
	{
        Task<List<TaskToDo>> GetAll(int userId);

        ValueTask<TaskToDo?> Find(int id, int userId);

        Task Add(TaskToDo task);

        Task Update(TaskToDo task);

        Task Remove(TaskToDo task);
    }

	public class TaskService : ITaskService
	{
        private readonly AppDbContext _dbContext;
        public TaskService(AppDbContext dbContext)
		{
            _dbContext = dbContext;
        }

        public async Task Add(TaskToDo task)
        {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<TaskToDo?> Find(int id, int userId)
        {
            return await _dbContext.Tasks.SingleAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<List<TaskToDo>> GetAll(int userId)
        {
            return await _dbContext.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task Remove(TaskToDo task)
        {
            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TaskToDo task)
        {
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();
        }
    }
}

