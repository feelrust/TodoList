using System;
using Microsoft.EntityFrameworkCore;
using TodoList.Entity;

namespace TodoList.Services
{
	public interface IUserService
	{
        ValueTask<User?> Find(int id);

        ValueTask<User?> FindByUserName(string userName);

        Task Add(User todo);
    }
	public class UserService: IUserService
	{
        private readonly AppDbContext _dbContext;
        public UserService(AppDbContext dbContext)
		{
            _dbContext = dbContext;
		}

        public async Task Add(User todo)
        {
            await _dbContext.Users.AddAsync(todo);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<User?> Find(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async ValueTask<User?> FindByUserName(string userName)
        {
            return await _dbContext.Users.SingleAsync(t => t.Username == userName);
        }
    }
}

