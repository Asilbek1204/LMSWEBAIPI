using LMS.Data.Entities;

namespace LMS.Data.Repositories
{
    public interface IModuleRepository
    {
        Task<Module?> GetByIdAsync(int id);
        Task<IEnumerable<Module>> GetAllAsync();
        Task<Module> AddAsync(Module entity);
        Task UpdateAsync(Module entity);
        Task DeleteAsync(Module entity);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Module>> GetModulesByCourseAsync(Guid courseId);
        Task<Module?> GetModuleWithCourseAsync(int id);
        Task ReorderModulesAsync(Guid courseId, int fromOrder, int toOrder);
    }
}