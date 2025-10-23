using AutoMapper;
using LMS.Data.Entities;
using LMS.Data.Repositories;
using LMS.Logic.Exceptions;
using LMS.Shared.Dtos;

namespace LMS.Logic.Services
{
    public class UserService(
        IUserRepository userRepository,
        IMapper mapper) : IUserService
    {
        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found");
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await userRepository.GetByEmailAsync(email);
            if (user == null) throw new NotFoundException("User not found");
            return mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IEnumerable<UserDto>> GetByRoleAsync(string role)
        {
            var users = await userRepository.GetByRoleAsync(role);
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> UpdateAsync(string id, UpdateUserDto updateUserDto)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found");

            mapper.Map(updateUserDto, user);
            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await userRepository.UpdateAsync(user);
            return mapper.Map<UserDto>(updatedUser);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found");

            return await userRepository.DeleteAsync(id);
        }
    }
}