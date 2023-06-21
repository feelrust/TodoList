using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoList.Entity;

namespace TodoList.Services
{
	public interface IUserService
	{
        ValueTask<User?> Find(int id);

        ValueTask<User?> FindByUserName(string userName);

        Task Add(User user);

        Task<List<User>> GetAll();

        Task Update(User user);
    }
	public class UserService: IUserService
	{
        private readonly AppDbContext _dbContext;
        public UserService(AppDbContext dbContext)
		{
            _dbContext = dbContext;
		}

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
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

        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}

