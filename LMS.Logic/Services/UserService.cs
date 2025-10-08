using AutoMapper;
using LMS.Data.Entities;
using LMS.Data.Repositories.Interfaces;
using LMS.Logic.Exceptions;
using LMS.Shared.Dtos.UserDtos;
using LMS.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace LMS.Logic.Services
{
    public class UserService(
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper) : IUserService
    {

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found.");

            var userDto = mapper.Map<UserDto>(user);

            // Get user roles
            var roles = await userManager.GetRolesAsync(user);
            userDto.Role = roles.FirstOrDefault();

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = mapper.Map<UserDto>(user);
                var roles = await userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();
                userDtos.Add(userDto);
            }

            return userDtos;
        }

        //public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
        //{
        //    if (!await roleManager.RoleExistsAsync(role))
        //        throw new ArgumentException($"Role {role} does not exist.");

        //    var users = await userRepository.GetByRoleAsync(role);
        //    var userDtos = mapper.Map<IEnumerable<UserDto>>(users);

        //    foreach (var userDto in userDtos)
        //    {
        //        userDto.Role = role;
        //    }

        //    return userDtos;
        //}

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Check if user already exists
            var existingUser = await userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
                throw new ArgumentException($"User with email {createUserDto.Email} already exists.");

            // Validation for Teacher
            if (createUserDto.Role == UserRole.Teacher.ToString() &&
                string.IsNullOrEmpty(createUserDto.Specialization))
            {
                throw new ArgumentException("Specialization is required for Teacher role.");
            }

            // Create new user
            var user = new ApplicationUser
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            // Create user with UserManager (bu user ni database ga saqlaydi)
            var result = await userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }

            // Assign role
            if (!string.IsNullOrEmpty(createUserDto.Role))
            {
                if (!await roleManager.RoleExistsAsync(createUserDto.Role))
                    throw new ArgumentException($"Role {createUserDto.Role} does not exist.");

                await userManager.AddToRoleAsync(user, createUserDto.Role);
            }

            // Create profile based on role
            if (createUserDto.Role == UserRole.Teacher.ToString())
            {
                user.TeacherProfile = new TeacherProfile
                {
                    UserId = user.Id,
                    Bio = createUserDto.Bio ?? "",
                    Qualifications = createUserDto.Qualifications ?? "",
                    Specialization = createUserDto.Specialization,
                    YearsOfExperience = 0
                };

                // Profile ni qo'shish uchun UPDATE
                await userRepository.UpdateAsync(user);
            }
            else if (createUserDto.Role == UserRole.Student.ToString())
            {
                user.StudentProfile = new StudentProfile
                {
                    UserId = user.Id,
                    StudentId = GenerateStudentId(),
                    EnrollmentDate = DateTime.UtcNow
                };

                // Profile ni qo'shish uchun UPDATE
                await userRepository.UpdateAsync(user);
            }

            var userDto = mapper.Map<UserDto>(user);
            userDto.Role = createUserDto.Role;

            return userDto;
        }


        public async Task<UserDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found.");

            mapper.Map(updateUserDto, user);

            await userRepository.UpdateAsync(user);

            var userDto = mapper.Map<UserDto>(user);
            var roles = await userManager.GetRolesAsync(user);
            userDto.Role = roles.FirstOrDefault();

            return userDto;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found.");

            await userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AssignRoleAsync(string userId, string role)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found.");

            if (!await roleManager.RoleExistsAsync(role))
                throw new ArgumentException($"Role {role} does not exist.");

            // Remove existing roles
            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add new role
            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                throw new Exception($"Failed to assign role: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            return true;
        }

        private string GenerateStudentId()
        {
            return $"STU{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}