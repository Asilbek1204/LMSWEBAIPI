using LMS.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace LMS.Data.Repositories
{
    public class TeacherProfileRepository(AppDbContext context) : ITeacherProfileRepository
    {

        public async Task<TeacherProfile> GetByIdAsync(int id)
        {
            return await context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TeacherProfile> GetByUserIdAsync(string userId)
        {
            return await context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<IEnumerable<TeacherProfile>> GetAllAsync()
        {
            return await context.Teachers
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task AddAsync(TeacherProfile teacherProfile)
        {
            await context.Teachers.AddAsync(teacherProfile);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeacherProfile teacherProfile)
        {
            context.Teachers.Update(teacherProfile);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var teacherProfile = await GetByIdAsync(id);
            if (teacherProfile != null)
            {
                context.Teachers.Remove(teacherProfile);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Teachers.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> ExistsForUserAsync(string userId)
        {
            return await context.Teachers.AnyAsync(t => t.UserId == userId);
        }
    }
}