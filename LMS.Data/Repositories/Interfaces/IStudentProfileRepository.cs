using LMS.Data.Entities;


namespace LMS.Data.Repositories
{
    public interface IStudentProfileRepository
    {
        Task<StudentProfile> GetByIdAsync(int id);
        Task<StudentProfile> GetByUserIdAsync(string userId);
        Task<IEnumerable<StudentProfile>> GetAllAsync();
        Task AddAsync(StudentProfile studentProfile);
        Task UpdateAsync(StudentProfile studentProfile);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsForUserAsync(string userId);
    }
}