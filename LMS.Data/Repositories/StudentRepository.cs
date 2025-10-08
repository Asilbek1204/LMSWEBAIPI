using LMS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Data.Repositories
{
    public class StudentProfileRepository(AppDbContext context) 
        : IStudentProfileRepository
    {

        public async Task<StudentProfile> GetByIdAsync(int id)
        {
            return await context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StudentProfile> GetByUserIdAsync(string userId)
        {
            return await context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<IEnumerable<StudentProfile>> GetAllAsync()
        {
            return await context.Students
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task AddAsync(StudentProfile studentProfile)
        {
            await context.Students.AddAsync(studentProfile);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentProfile studentProfile)
        {
            context.Students.Update(studentProfile);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var studentProfile = await GetByIdAsync(id);
            if (studentProfile != null)
            {
                context.Students.Remove(studentProfile);
                await context.SaveChangesAsync();
            }
            
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Students.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> ExistsForUserAsync(string userId)
        {
            return await context.Students.AnyAsync(s => s.UserId == userId);
        }
    }
}