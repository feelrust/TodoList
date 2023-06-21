using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoList.Entity;
using TodoList.Enums;

namespace TodoList.Services
{
	public interface IUserService
	{
        ValueTask<User?> Find(int id);

        ValueTask<User?> FindByUserName(string userName);

        Task Add(User user, int roleId);

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

        public async Task Add(User user, int roleId)
        {

            user.UserRoles.Add(new UserRole()
            {
               UserId = user.Id, RoleId = roleId > 0 ? roleId : (int)RoleName.User
            });

            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<User?> Find(int id)
        {
            return await _dbContext.Users
                .Include(o => o.Roles)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async ValueTask<User?> FindByUserName(string userName)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Include(o => o.Roles)
                .Where(u => u.Username == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users
                .Include(o => o.Roles)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}

