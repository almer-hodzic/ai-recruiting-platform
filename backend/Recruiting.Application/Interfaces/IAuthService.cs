using Recruiting.Application.DTOs;

namespace Recruiting.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(CreateUserDto dto);
        Task<string?> LoginAsync(string email, string password);
    }
}

