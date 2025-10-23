using LMS.Data.Entities;

namespace LMS.Data.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetByIdAsync(int id);
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment> AddAsync(Enrollment entity);
        Task UpdateAsync(Enrollment entity);
        Task DeleteAsync(Enrollment entity);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(Guid courseId);
        Task<Enrollment?> GetEnrollmentWithDetailsAsync(int id);
        Task<bool> IsStudentEnrolledAsync(Guid courseId, Guid studentId);
    }
}
