using LMS.Data.Entities;
using LMS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                .ToListAsync();
        }

        //public async Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string role)
        //{
        //    var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == role);
        //    if (userRole == null) return new List<ApplicationUser>();

        //    var userRoleIds = await context.UserRoles
        //        .Where(ur => ur.RoleId == userRole.Id)
        //        .Select(ur => ur.UserId)
        //        .ToListAsync();

        //    return await context.Users
        //        .Where(u => userRoleIds.Contains(u.Id))
        //        .Include(u => u.TeacherProfile)
        //        .Include(u => u.StudentProfile)
        //        .ToListAsync();
        //}

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await context.Users
                .Include(u => u.TeacherProfile)
                .Include(u => u.StudentProfile)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(ApplicationUser user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await context.Users.AnyAsync(u => u.Id == id);
        }
    }
}