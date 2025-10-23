using LMS.Data.Entities;

public interface ITeacherRepository
{
    Task<Teacher> GetByIdAsync(Guid id);
    Task<Teacher> GetByUserIdAsync(string userId);
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher> CreateAsync(Teacher teacher);
    Task<Teacher> UpdateAsync(Teacher teacher);
    Task<bool> DeleteAsync(Guid id);
}