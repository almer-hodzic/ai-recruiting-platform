using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Recruiting.Domain.Entities;
using Recruiting.Application.DTOs;
using Recruiting.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Recruiting.Infrastructure.Data;

namespace Recruiting.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<UserDto?> RegisterAsync(CreateUserDto dto)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existing != null) return null;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Password))
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == encoded);

            if (user == null) return null;

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

