using LMS.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace LMS.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext context;

        public TeacherRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Teacher> GetByIdAsync(Guid id)
        {
            return await context.Teachers
                .Include(t => t.User)
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<Teacher> GetByUserIdAsync(string userId)
        {
            return await context.Teachers
                .Include(t => t.User)
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await context.Teachers
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<Teacher> CreateAsync(Teacher teacher)
        {
            context.Teachers.Add(teacher);
            await context.SaveChangesAsync();
            return teacher;
        }

        public async Task<Teacher> UpdateAsync(Teacher teacher)
        {
            context.Teachers.Update(teacher);
            await context.SaveChangesAsync();
            return teacher;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var teacher = await GetByIdAsync(id);
            if (teacher == null) return false;

            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();
            return true;
        }
    }
}