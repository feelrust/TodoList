using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Claims;
using System.Text;
using TodoList.Entity;

namespace TodoList.Services
{
	public class TokenService
	{

		//public string TestHash(string hash)
		//{
		//	var x = new PasswordHasher<User>();
		//	var user = new User() { Username = ",", Password = "," };
		//	var hashed = x.HashPassword(user, user.Password);
		//	return hashed;
		//}

		public string GenerateToken(User user)
		{
			JwtSecurityTokenHandler tokenHandler = new();

            var key = ApiSettings.GenerateSecretByte();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString()),
				}),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
	}
}

