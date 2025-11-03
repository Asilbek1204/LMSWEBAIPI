using LMS.Data.Entities;
using LMS.Shared.Dtos.PaginationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Data.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(Guid id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> AddAsync(Course entity);
        Task UpdateAsync(Course entity);
        Task DeleteAsync(Course entity);
        Task<bool> ExistsAsync(Guid id);

        Task<(IEnumerable<Course> Items, int TotalCount)> GetAllAsync(CourseFilterParams filterParams);
        Task<IEnumerable<Course>> GetCoursesByTeacherAsync(Guid teacherId);
        Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId);
        Task<Course?> GetCourseWithDetailsAsync(Guid id);
        Task<bool> IsTeacherCourseOwnerAsync(Guid courseId, Guid teacherId);
    }
}
