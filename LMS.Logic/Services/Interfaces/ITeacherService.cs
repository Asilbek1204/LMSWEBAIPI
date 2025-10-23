using LMS.Shared.Dtos.TeacherDtos;

namespace LMS.Logic.Services
{
    public interface ITeacherService
    {
        Task<TeacherDto> GetByIdAsync(Guid id);
        Task<TeacherDto> GetByUserIdAsync(string userId);
        Task<IEnumerable<TeacherDto>> GetAllAsync();
        Task<TeacherDto> UpdateAsync(Guid id, UpdateTeacherDto updateTeacherDto);
        Task<bool> DeleteAsync(Guid id);
        Task<TeacherDto?> GetTeacherByUserIdAsync(string userId);
        Task<TeacherDto?> GetTeacherByIdAsync(Guid id);
    }
}