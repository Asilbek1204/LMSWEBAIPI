using LMS.Shared.Dtos;

namespace LMS.Logic.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> GetByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<IEnumerable<UserDto>> GetByRoleAsync(string role);
        Task<UserDto> UpdateAsync(string id, UpdateUserDto updateUserDto);
        Task<bool> DeleteAsync(string id);
    }
}