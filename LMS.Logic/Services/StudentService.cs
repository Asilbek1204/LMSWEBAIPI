using AutoMapper;
using LMS.Data.Repositories;
using LMS.Logic.Exceptions;
using LMS.Shared.Dtos.StudentDtos;

namespace LMS.Logic.Services
{
    public class StudentService(IStudentRepository studentRepository,
        IUserRepository userRepository,
        IUserService userService,
        IMapper mapper) : IStudentService
    {
        public async Task<StudentDto> GetByIdAsync(Guid id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) throw new NotFoundException("Student not found");

            return mapper.Map<StudentDto>(student); 
        }

        public async Task<StudentDto> GetByUserIdAsync(string userId)
        {
            var student = await studentRepository.GetByUserIdAsync(userId);
            if (student == null) throw new NotFoundException("Student not found");

            return mapper.Map<StudentDto>(student); 
        }

        public async Task<StudentDto> GetByStudentIdAsync(string studentId)
        {
            var student = await studentRepository.GetByStudentIdAsync(studentId);
            if (student == null) throw new NotFoundException("Student not found");

            return mapper.Map<StudentDto>(student); 
        }

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var students = await studentRepository.GetAllAsync();
            return mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto> UpdateProfileAsync(Guid id, UpdateStudentProfileDto updateProfileDto)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) throw new NotFoundException("Student not found");

            // Student ma'lumotlarini yangilash
            if (updateProfileDto.StudentInfo != null)
            {
                mapper.Map(updateProfileDto.StudentInfo, student); 
                await studentRepository.UpdateAsync(student);
            }

            // User ma'lumotlarini yangilash
            if (updateProfileDto.UserInfo != null)
            {
                await userService.UpdateAsync(student.UserId, updateProfileDto.UserInfo);
            }

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) throw new NotFoundException("Student not found");

            return await studentRepository.DeleteAsync(id);
        }
    }
}