using LMS.Data.Entities;

public interface IUserRepository
{
    // GET operations
    Task<User> GetByIdAsync(string id);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByUserNameAsync(string userName);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetByRoleAsync(string role);

    // CREATE operations
    Task<User> CreateAsync(User user);

    // UPDATE operations  
    Task<User> UpdateAsync(User user);

    // DELETE operations
    Task<bool> DeleteAsync(string id);

    // AUTH operations
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UserNameExistsAsync(string userName);
}