using LMS.Data.Entities;
using LMS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Data.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        protected readonly AppDbContext context;

        public EnrollmentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Enrollment?> GetByIdAsync(int id)
            => await context.Enrollments.FindAsync(id);

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
            => await context.Enrollments.ToListAsync();

        public async Task<Enrollment> AddAsync(Enrollment entity)
        {
            await context.Enrollments.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Enrollment entity)
        {
            context.Enrollments.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Enrollment entity)
        {
            context.Enrollments.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await context.Enrollments.AnyAsync(e => e.Id == id);

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId)
            => await context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.Category)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(Guid courseId)
            => await context.Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

        public async Task<Enrollment?> GetEnrollmentWithDetailsAsync(int id)
            => await context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<bool> IsStudentEnrolledAsync(Guid courseId, Guid studentId)
            => await context.Enrollments
                .AnyAsync(e => e.CourseId == courseId && e.StudentId == studentId && e.Status == "Active");
    }
}
