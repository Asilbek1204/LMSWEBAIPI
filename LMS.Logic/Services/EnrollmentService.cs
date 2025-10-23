using AutoMapper;
using LMS.Data.Entities;
using LMS.Data.Repositories;
using LMS.Data.Repositories.Interfaces;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;

namespace LMS.Logic.Services
{
    public class EnrollmentService(
            IMapper mapper,
            IEnrollmentRepository enrollmentRepository,
            ICourseRepository courseRepository,
            IStudentRepository studentRepository) : IEnrollmentService
    {

        public async Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(Guid studentId)
        {
            var enrollments = await enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            var enrollmentDtos = mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);

            foreach (var enrollmentDto in enrollmentDtos)
            {
                var enrollment = enrollments.FirstOrDefault(e => e.Id == enrollmentDto.Id);
                if (enrollment != null)
                {
                    enrollmentDto.CourseTitle = enrollment.Course?.Title ?? string.Empty;
                    enrollmentDto.StudentName = enrollment.Student?.User?.FirstName + " " + enrollment.Student?.User?.LastName;
                }
            }

            return enrollmentDtos;
        }

        public async Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(Guid courseId)
        {
            var enrollments = await enrollmentRepository.GetEnrollmentsByCourseAsync(courseId);
            var enrollmentDtos = mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);

            var course = await courseRepository.GetByIdAsync(courseId);
            foreach (var enrollmentDto in enrollmentDtos)
            {
                enrollmentDto.CourseTitle = course?.Title ?? string.Empty;

                var enrollment = enrollments.FirstOrDefault(e => e.Id == enrollmentDto.Id);
                if (enrollment != null)
                {
                    enrollmentDto.StudentName = enrollment.Student?.User?.FirstName + " " + enrollment.Student?.User?.LastName;
                }
            }

            return enrollmentDtos;
        }

        public async Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await enrollmentRepository.GetEnrollmentWithDetailsAsync(id);
            if (enrollment == null) return null;

            var enrollmentDto = mapper.Map<EnrollmentDto>(enrollment);
            enrollmentDto.CourseTitle = enrollment.Course?.Title ?? string.Empty;
            enrollmentDto.StudentName = enrollment.Student?.User?.FirstName + " " + enrollment.Student?.User?.LastName;

            return enrollmentDto;
        }

        public async Task<EnrollmentDto> CreateEnrollmentAsync(EnrollmentCreateDto dto)
        {
            var course = await courseRepository.GetByIdAsync(dto.CourseId);
            if (course == null)
                throw new ArgumentException("Course not found");

            if (!course.IsPublished)
                throw new InvalidOperationException("Cannot enroll in unpublished course");

            var student = await studentRepository.GetByIdAsync(dto.StudentId);
            if (student == null)
                throw new ArgumentException("Student not found");

            if (await enrollmentRepository.IsStudentEnrolledAsync(dto.CourseId, dto.StudentId))
                throw new InvalidOperationException("Student is already enrolled in this course");

            var enrollment = mapper.Map<Enrollment>(dto);
            enrollment.EnrollmentDate = DateTime.UtcNow;
            enrollment.Status = "Active";

            var createdEnrollment = await enrollmentRepository.AddAsync(enrollment);
            return await GetEnrollmentByIdAsync(createdEnrollment.Id);
        }

        public async Task<EnrollmentDto?> UpdateEnrollmentAsync(int id, EnrollmentUpdateDto dto)
        {
            var enrollment = await enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null) return null;

            mapper.Map(dto, enrollment);
            await enrollmentRepository.UpdateAsync(enrollment);
            return await GetEnrollmentByIdAsync(id);
        }

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            var enrollment = await enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null) return false;

            await enrollmentRepository.DeleteAsync(enrollment);
            return true;
        }

        public async Task<bool> IsStudentEnrolledAsync(Guid courseId, Guid studentId)
        {
            return await enrollmentRepository.IsStudentEnrolledAsync(courseId, studentId);
        }
    }
}
