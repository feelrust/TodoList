
using System;
namespace TodoList.Entity
{
	public class Role
	{
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<UserRole> UserRoles { get; } = new();
        public List<User> Users { get; } = new();
    }
}

