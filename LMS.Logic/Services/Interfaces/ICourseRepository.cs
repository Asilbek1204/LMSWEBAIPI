using LMS.Shared.Dtos.EntityDtos;
using LMS.Shared.Dtos.PaginationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Logic.Services.Interfaces
{
    public interface ICourseService
    {
        Task<PagedResult<CourseDto>> GetAllCoursesAsync(CourseFilterParams filterParams);

        Task<CourseDto?> GetCourseByIdAsync(Guid id);
        Task<CourseDto> CreateCourseAsync(CourseCreateDto dto, string userId);
        Task<CourseDto?> UpdateCourseAsync(Guid id, CourseUpdateDto dto, string userId);
        Task<bool> DeleteCourseAsync(Guid id, string userId);
        Task<IEnumerable<CourseDto>> GetTeacherCoursesAsync(Guid teacherId);
        Task<bool> PublishCourseAsync(Guid id, string userId);
    }
}
