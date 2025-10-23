using LMS.Data.Entities;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LMS.Logic.Helpers
{
    /// JWT tokenlar bilan ishlash interfeysi
    public interface IJwtHelper
    { 
        /// User uchun JWT token yaratish
        string GenerateToken(User user);

        /// Token dan user ID sini olish
        string GetUserIdFromToken(string token);
    }
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration configuration;

        public JwtHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // Token claims - user haqida ma'lumotlar
            var claims = new[]
            {
                new System.Security.Claims.Claim("id", user.Id),
                new System.Security.Claims.Claim("email", user.Email),
                new Claim("roles", string.Join(",", user.Roles ?? new List<string>())),
                new System.Security.Claims.Claim("firstName", user.FirstName),
                new System.Security.Claims.Claim("lastName", user.LastName)
            };

            // Secret key
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            // Token yaratish
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.First(c => c.Type == "id").Value;
        }
    }
}