using LMS.Data.Entities;
using LMS.Data.Repositories.Interfaces;
using LMS.Shared.Dtos.PaginationDtos;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        protected readonly AppDbContext context;

        public CourseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Course?> GetByIdAsync(Guid id)
            => await context.Courses.FindAsync(id);

        public async Task<IEnumerable<Course>> GetAllAsync()
            => await context.Courses.ToListAsync();

        public async Task<Course> AddAsync(Course entity)
        {
            await context.Courses.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Course entity)
        {
            context.Courses.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course entity)
        {
            context.Courses.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await context.Courses.AnyAsync(c => c.Id == id);

        //Expression pridecate
        public async Task<(IEnumerable<Course> Items, int TotalCount)> GetAllAsync(CourseFilterParams filterParams)
        {
            var query = context.Courses
                .Include(c => c.Category)
                .Include(c => c.Teacher)
                .ThenInclude(t => t.User)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(filterParams.Title))
                query = query.Where(c => c.Title.ToLower().Contains(filterParams.Title.ToLower()));

            if (filterParams.TeacherId.HasValue)
                query = query.Where(c => c.TeacherId == filterParams.TeacherId);

            if (filterParams.CategoryId.HasValue)
                query = query.Where(c => c.CategoryId == filterParams.CategoryId);

            // Sorting
            query = (filterParams.SortBy?.ToLower(), filterParams.SortDescending) switch
            {
                ("price", false) => query.OrderBy(c => c.Price),
                ("price", true) => query.OrderByDescending(c => c.Price),
                ("createdat", false) => query.OrderBy(c => c.CreatedAt),
                ("createdat", true) => query.OrderByDescending(c => c.CreatedAt),
                ("duration", false) => query.OrderBy(c => c.Duration),
                ("duration", true) => query.OrderByDescending(c => c.Duration),
                (_, false) => query.OrderBy(c => c.Title),
                (_, true) => query.OrderByDescending(c => c.Title)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filterParams.Page - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Course>> GetCoursesByTeacherAsync(Guid teacherId)
            => await context.Courses
                .Include(c => c.Category)
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();

        public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId)
            => await context.Courses
                .Include(c => c.Category)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t.User)
                .Where(c => c.CategoryId == categoryId && c.IsPublished)
                .ToListAsync();

        public async Task<Course?> GetCourseWithDetailsAsync(Guid id)
            => await context.Courses
                .Include(c => c.Category)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t.User)
                .Include(c => c.Modules)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                        .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<bool> IsTeacherCourseOwnerAsync(Guid courseId, Guid teacherId)
            => await context.Courses
                .AnyAsync(c => c.Id == courseId && c.TeacherId == teacherId);
    }
}
