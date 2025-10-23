using LMS.Shared.Dtos.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Logic.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(Guid studentId);
        Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(Guid courseId);
        Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id);
        Task<EnrollmentDto> CreateEnrollmentAsync(EnrollmentCreateDto dto);
        Task<EnrollmentDto?> UpdateEnrollmentAsync(int id, EnrollmentUpdateDto dto);
        Task<bool> DeleteEnrollmentAsync(int id);
        Task<bool> IsStudentEnrolledAsync(Guid courseId, Guid studentId);
    }
}
