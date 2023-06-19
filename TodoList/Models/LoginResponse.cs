using System;
namespace TodoList.Models
{
	public class LoginResponse
	{
        public LoginResponse(string userName, string token)
        {
            Username = userName;
            Token = token;
        }

        public string Username { get; set; }
        public string Token { get; set; }
    }
}

