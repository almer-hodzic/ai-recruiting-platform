using Recruiting.Application.DTOs;

namespace Recruiting.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
