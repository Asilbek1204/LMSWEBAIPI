using LMS.Shared.Dtos.StudentDtos;

namespace LMS.Logic.Services
{
    public interface IStudentService
    {
        Task<StudentDto> GetByIdAsync(Guid id);
        Task<StudentDto> GetByUserIdAsync(string userId);
        Task<StudentDto> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<StudentDto>> GetAllAsync();
        Task<StudentDto> UpdateProfileAsync(Guid id, UpdateStudentProfileDto updateProfileDto);
        Task<bool> DeleteAsync(Guid id);
    }
}