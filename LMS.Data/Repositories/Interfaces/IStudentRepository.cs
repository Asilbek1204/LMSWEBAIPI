using LMS.Data.Entities;


namespace LMS.Data.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(Guid id);
        Task<Student> GetByUserIdAsync(string userId);
        Task<Student> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> CreateAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<bool> DeleteAsync(Guid id);
        Task<string> GenerateStudentIdAsync();
    }
}