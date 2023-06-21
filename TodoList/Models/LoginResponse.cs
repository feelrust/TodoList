using System;
namespace TodoList.Models
{
	public class LoginResponse
	{
        public LoginResponse(string userName, string token, IEnumerable<string> roles) 
        {
            Username = userName;
            Token = token;
            Roles = roles;
        }

        public string Username { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

