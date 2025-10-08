using AutoMapper;
using LMS.Data.Repositories;
using LMS.Logic.Exceptions;
using LMS.Shared.Dtos.TeacherDtos;
using LMS.Shared.Dtos.UserDtos;
using LMS.Shared.Enums;

namespace LMS.Logic.Services
{
    public class TeacherService(ITeacherProfileRepository teacherProfileRepository,
        IUserService userService,
        IMapper mapper) : ITeacherService
    {
   
        public async Task<TeacherDto> GetTeacherByIdAsync(int id)
        {
            var teacherProfile = await teacherProfileRepository.GetByIdAsync(id);
            if (teacherProfile == null)
                throw new NotFoundException($"Teacher with ID {id} not found.");

            var teacherDto = mapper.Map<TeacherDto>(teacherProfile);
            teacherDto.UserInfo = await userService.GetUserByIdAsync(teacherProfile.UserId);

            return teacherDto;
        }

        public async Task<TeacherDto> GetTeacherByUserIdAsync(string userId)
        {
            var teacherProfile = await teacherProfileRepository.GetByUserIdAsync(userId);
            if (teacherProfile == null)
                throw new NotFoundException($"Teacher with User ID {userId} not found.");

            var teacherDto = mapper.Map<TeacherDto>(teacherProfile);
            teacherDto.UserInfo = await userService.GetUserByIdAsync(userId);

            return teacherDto;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teacherProfiles = await teacherProfileRepository.GetAllAsync();
            var teacherDtos = new List<TeacherDto>();

            foreach (var teacher in teacherProfiles)
            {
                var teacherDto = mapper.Map<TeacherDto>(teacher);

                
                if (!string.IsNullOrEmpty(teacher.UserId))
                {
                    var userInfo = await userService.GetUserByIdAsync(teacher.UserId);
                    teacherDto.UserInfo = userInfo;
                }

                teacherDtos.Add(teacherDto);
            }

            return teacherDtos;
        }

        public async Task<TeacherDto> CreateTeacherAsync(CreateUserDto createUserDto)
        {
            createUserDto.Role = UserRole.Teacher.ToString();
            var userDto = await userService.CreateUserAsync(createUserDto);

            var teacherProfile = await teacherProfileRepository.GetByUserIdAsync(userDto.Id);
            //if (teacherProfile != null)
            //    throw new BadRequestException("Teacher profile already exists for this user.");
            var teacherDto = mapper.Map<TeacherDto>(teacherProfile);
            teacherDto.UserInfo = userDto;
            if (string.IsNullOrEmpty(teacherProfile.UserId))
                throw new BadRequestException("User ID cannot be null or empty when creating teacher profile.");
            return teacherDto;
        }

        public async Task<TeacherDto> UpdateTeacherAsync(int id, UpdateUserDto updateUserDto)
        {
            var teacherProfile = await teacherProfileRepository.GetByIdAsync(id);
            if (teacherProfile == null)
                throw new NotFoundException($"Teacher with ID {id} not found.");

            var userDto = await userService.UpdateUserAsync(teacherProfile.UserId, updateUserDto);

            mapper.Map(updateUserDto, teacherProfile);
            await teacherProfileRepository.UpdateAsync(teacherProfile);

            var teacherDto = mapper.Map<TeacherDto>(teacherProfile);
            teacherDto.UserInfo = userDto;

            return teacherDto;
        }

        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var teacherProfile = await teacherProfileRepository.GetByIdAsync(id);
            if (teacherProfile == null)
                throw new NotFoundException($"Teacher with ID {id} not found.");

            await userService.DeleteUserAsync(teacherProfile.UserId);
            return true;
        }
    }
}