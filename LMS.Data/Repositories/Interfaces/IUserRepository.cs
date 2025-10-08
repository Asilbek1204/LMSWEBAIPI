using LMS.Data.Entities;

namespace LMS.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        //Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task AddAsync(ApplicationUser user);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}