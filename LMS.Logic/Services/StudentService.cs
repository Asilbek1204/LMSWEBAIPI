using AutoMapper;
using LMS.Data.Entities;
using LMS.Data.Repositories;
using LMS.Logic.Exceptions;
using LMS.Shared.Dtos.StudentDtos;
using LMS.Shared.Dtos.UserDtos;
using LMS.Shared.Enums;

namespace LMS.Logic.Services
{
    public class StudentService(IStudentProfileRepository studentProfileRepository,
        IUserService userService,
        IMapper mapper) : IStudentService
    {

        public async Task<StudentDto> GetStudentByIdAsync(int id)
        {
            var student = await studentProfileRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException($"Student with ID {id} not found.");

            var studentDto = mapper.Map<StudentDto>(student);

            if (!string.IsNullOrEmpty(student.UserId))
            {
                var userDto = await userService.GetUserByIdAsync(student.UserId);
                studentDto.UserInfo = userDto;
            }

            return studentDto;
        }

        public async Task<StudentDto> GetStudentByUserIdAsync(string userId)
        {
            var student = await studentProfileRepository.GetByUserIdAsync(userId);
            if (student == null)
                throw new NotFoundException($"Student with User ID {userId} not found.");

            var studentDto = mapper.Map<StudentDto>(student);

            var userDto = await userService.GetUserByIdAsync(userId);
            studentDto.UserInfo = userDto;

            return studentDto;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await studentProfileRepository.GetAllAsync();

            if (students == null || !students.Any())
                return new List<StudentDto>();

            var studentDtos = new List<StudentDto>();

            foreach (var student in students)
            {
                var studentDto = mapper.Map<StudentDto>(student);

                if (!string.IsNullOrEmpty(student.UserId))
                {
                    try
                    {
                        var userDto = await userService.GetUserByIdAsync(student.UserId);
                        studentDto.UserInfo = userDto;
                    }
                    catch
                    {
                        studentDto.UserInfo = null;
                    }
                }

                studentDtos.Add(studentDto);
            }

            return studentDtos;
        }

        public async Task<StudentDto> CreateStudentAsync(CreateUserDto createUserDto)
        {

                createUserDto.Role = UserRole.Student.ToString();
                var userDto = await userService.CreateUserAsync(createUserDto);
                if (userDto == null)
                    throw new BadRequestException("Failed to create user for student.");

                var studentProfile = new StudentProfile
                {
                    UserId = userDto.Id,
                    StudentId = GenerateStudentId(),
                    EnrollmentDate = DateTime.UtcNow
                };

                if (string.IsNullOrEmpty(studentProfile.UserId))
                    throw new BadRequestException("User ID cannot be null or empty when creating student profile.");

                await studentProfileRepository.AddAsync(studentProfile);

                var studentDto = mapper.Map<StudentDto>(studentProfile);
                studentDto.UserInfo = userDto;

                return studentDto;
            
        }

        public async Task<StudentDto> UpdateStudentAsync(int id, UpdateUserDto updateUserDto)
        {
            var student = await studentProfileRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException($"Student with ID {id} not found.");

            var userDto = await userService.UpdateUserAsync(student.UserId, updateUserDto);

            var studentDto = mapper.Map<StudentDto>(student);
            studentDto.UserInfo = userDto;

            return studentDto;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await studentProfileRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException($"Student with ID {id} not found.");

            await studentProfileRepository.DeleteAsync(id);
            await userService.DeleteUserAsync(student.UserId);

            return true;
        }

        private string GenerateStudentId()
        {
            return $"STU{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}