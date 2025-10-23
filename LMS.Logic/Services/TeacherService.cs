using AutoMapper;
using LMS.Data.Repositories;
using LMS.Logic.Exceptions;
using LMS.Shared.Dtos.TeacherDtos;

namespace LMS.Logic.Services
{
    public class TeacherService(ITeacherRepository teacherRepository,
        IUserRepository userRepository,
        IUserService userService,
        IMapper mapper) : ITeacherService
    {

        public async Task<TeacherDto> GetByIdAsync(Guid id)
        {
            var teacher = await teacherRepository.GetByIdAsync(id);
            if (teacher == null) throw new NotFoundException("Teacher not found");

            return mapper.Map<TeacherDto>(teacher); 
        }

        public async Task<TeacherDto> GetByUserIdAsync(string userId)
        {
            var teacher = await teacherRepository.GetByUserIdAsync(userId);
            if (teacher == null) throw new NotFoundException("Teacher not found");

            return mapper.Map<TeacherDto>(teacher); 
        }

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            var teachers = await teacherRepository.GetAllAsync();
            return mapper.Map<IEnumerable<TeacherDto>>(teachers); 
        }

        public async Task<TeacherDto?> GetTeacherByUserIdAsync(string userId)
        {
            // TeacherRepository ga yangi method qo'shish kerak
            var teacher = await teacherRepository.GetByUserIdAsync(userId);
            return teacher == null ? null : mapper.Map<TeacherDto>(teacher);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(Guid id)
        {
            var teacher = await teacherRepository.GetByIdAsync(id);
            return teacher == null ? null : mapper.Map<TeacherDto>(teacher);
        }

        public async Task<TeacherDto> UpdateAsync(Guid id, UpdateTeacherDto updateTeacherDto)
        {
            var teacher = await teacherRepository.GetByIdAsync(id);
            if (teacher == null) throw new NotFoundException("Teacher not found");

            mapper.Map(updateTeacherDto, teacher); 
            var updatedTeacher = await teacherRepository.UpdateAsync(teacher);

            return mapper.Map<TeacherDto>(updatedTeacher); 
        }

        public async Task<TeacherDto> UpdateProfileAsync(Guid id, UpdateTeacherProfileDto updateProfileDto)
        {
            var teacher = await teacherRepository.GetByIdAsync(id);
            if (teacher == null) throw new NotFoundException("Teacher not found");

            // Teacher ma'lumotlarini yangilash
            if (updateProfileDto.TeacherInfo != null)
            {
                mapper.Map(updateProfileDto.TeacherInfo, teacher); 
                await teacherRepository.UpdateAsync(teacher);
            }

            // User ma'lumotlarini yangilash
            if (updateProfileDto.UserInfo != null)
            {
                await userService.UpdateAsync(teacher.UserId, updateProfileDto.UserInfo);
            }

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var teacher = await teacherRepository.GetByIdAsync(id);
            if (teacher == null) throw new NotFoundException("Teacher not found");

            return await teacherRepository.DeleteAsync(id);
        }
    }
}