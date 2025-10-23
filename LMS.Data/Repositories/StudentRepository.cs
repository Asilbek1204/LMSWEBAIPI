using LMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;

        public StudentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Student> GetByIdAsync(Guid id)
        {
            return await context.Students
                .Include(s => s.User)
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student> GetByUserIdAsync(string userId)
        {
            return await context.Students
                .Include(s => s.User)
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<Student> GetByStudentIdAsync(string studentId)
        {
            return await context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await context.Students
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<Student> CreateAsync(Student student)
        {
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            context.Students.Update(student);
            await context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await GetByIdAsync(id);
            if (student == null) return false;

            context.Students.Remove(student);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateStudentIdAsync()
        {
            var year = DateTime.Now.Year;
            var lastStudent = await context.Students
                .Where(s => s.StudentId.StartsWith($"STU{year}"))
                .OrderByDescending(s => s.StudentId)
                .FirstOrDefaultAsync();

            var nextNumber = 1;
            if (lastStudent != null)
            {
                var lastNumber = int.Parse(lastStudent.StudentId.Substring(7)); // STU2025001 -> 001
                nextNumber = lastNumber + 1;
            }

            return $"STU{year}{nextNumber:D3}"; // STU2025001, STU2025002, ...
        }
    }
}