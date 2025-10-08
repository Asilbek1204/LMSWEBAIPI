using LMS.Data.Entities;


namespace LMS.Data.Repositories
{
    public interface ITeacherProfileRepository
    {
        Task<TeacherProfile> GetByIdAsync(int id);
        Task<TeacherProfile> GetByUserIdAsync(string userId);
        Task<IEnumerable<TeacherProfile>> GetAllAsync();
        Task AddAsync(TeacherProfile teacherProfile);
        Task UpdateAsync(TeacherProfile teacherProfile);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsForUserAsync(string userId);
    }
}