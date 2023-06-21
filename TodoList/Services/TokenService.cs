using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Claims;
using System.Text;
using TodoList.Entity;
using Microsoft.AspNetCore.Identity;

namespace TodoList.Services
{
	public class TokenService
	{
		public string GenerateToken(User user)
		{
			JwtSecurityTokenHandler tokenHandler = new();

            var key = ApiSettings.GenerateSecretByte();

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            });
                    
            foreach (var role in user.Roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
				Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
	}
}

