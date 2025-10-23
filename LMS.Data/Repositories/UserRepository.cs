using LMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                 .Where(u => u.Roles != null &&
                u.Roles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserNameExistsAsync(string userName)
        {
            return await context.Users.AnyAsync(u => u.UserName == userName);
        }
    }
}