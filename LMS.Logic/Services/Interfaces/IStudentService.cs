using LMS.Shared.Dtos.StudentDtos;
using LMS.Shared.Dtos.UserDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Logic.Services
{
    public interface IStudentService
    {
        Task<StudentDto> GetStudentByIdAsync(int id);
        Task<StudentDto> GetStudentByUserIdAsync(string userId);
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto> CreateStudentAsync(CreateUserDto createUserDto);
        Task<StudentDto> UpdateStudentAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}