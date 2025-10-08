using LMS.Shared.Dtos.TeacherDtos;
using LMS.Shared.Dtos.UserDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Logic.Services
{
    public interface ITeacherService
    {
        Task<TeacherDto> GetTeacherByIdAsync(int id);
        Task<TeacherDto> GetTeacherByUserIdAsync(string userId); 
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
        Task<TeacherDto> CreateTeacherAsync(CreateUserDto createUserDto);
        Task<TeacherDto> UpdateTeacherAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteTeacherAsync(int id); 
    }
}