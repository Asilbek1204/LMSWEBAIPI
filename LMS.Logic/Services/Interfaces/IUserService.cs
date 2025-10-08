using LMS.Shared.Dtos.UserDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Logic.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        //Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> AssignRoleAsync(string userId, string role);
    }
}