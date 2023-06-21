using System;
namespace TodoList.Models
{
	public class UserResponse
	{
		public int id { get; set; }
        public string? Username { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}

