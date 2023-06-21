using System;
namespace TodoList.Entity
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public List<UserRole> UserRoles { get; } = new();
        public List<Role> Roles { get; } = new();
    }
}

