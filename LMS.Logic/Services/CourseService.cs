using AutoMapper;
using LMS.Data.Entities;
using LMS.Data.Repositories;
using LMS.Data.Repositories.Interfaces;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Logic.Services
{
    public class CourseService(
            IMapper mapper,
            ICourseRepository courseRepository,
            ICategoryRepository categoryRepository,
            ITeacherRepository teacherRepository) : ICourseService
    {

        public async Task<(IEnumerable<CourseDto> Items, int TotalCount)> GetAllCoursesAsync(
            string? title = null,
            Guid? teacherId = null,
            int? categoryId = null,
            string? sortBy = "title",
            bool sortDescending = false,
            int page = 1,
            int pageSize = 10)
        {
            var (courses, totalCount) = await courseRepository.GetAllAsync(
                title, teacherId, categoryId, sortBy, sortDescending, page, pageSize);

            var courseDtos = mapper.Map<IEnumerable<CourseDto>>(courses);
            return (courseDtos, totalCount);
        }

        public async Task<CourseDto?> GetCourseByIdAsync(Guid id)
        {
            var course = await courseRepository.GetCourseWithDetailsAsync(id);
            if (course == null) return null;

            var courseDto = mapper.Map<CourseDto>(course);
            courseDto.EnrollmentCount = course.Enrollments?.Count ?? 0;
            courseDto.ModuleCount = course.Modules?.Count ?? 0;
            courseDto.CategoryName = course.Category?.Name ?? string.Empty;
            courseDto.TeacherName = course.Teacher?.User?.FirstName + " " + course.Teacher?.User?.LastName;

            return courseDto;
        }

        public async Task<CourseDto> CreateCourseAsync(CourseCreateDto dto, string userId)
        {
            var category = await categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new ArgumentException("Category not found");

            var teacher = await teacherRepository.GetByUserIdAsync(userId);
            if (teacher == null)
                throw new ArgumentException("Teacher profile not found");

            var course = mapper.Map<Course>(dto);
            course.Id = Guid.NewGuid();
            course.TeacherId = teacher.Id;
            course.CreatedAt = DateTime.UtcNow;
            course.IsPublished = dto.IsPublished;

            var createdCourse = await courseRepository.AddAsync(course);
            return await GetCourseByIdAsync(createdCourse.Id);
        }

        public async Task<CourseDto?> UpdateCourseAsync(Guid id, CourseUpdateDto dto, string userId)
        {
            var course = await courseRepository.GetByIdAsync(id);
            if (course == null) return null;

            // UserId bo'yicha Teacher ni topish
            var teacher = await teacherRepository.GetByUserIdAsync(userId);
            if (teacher == null)
                throw new UnauthorizedAccessException("Teacher profile not found");

            // Course Teacher ga tegishli ekanligini tekshirish
            if (course.TeacherId != teacher.Id)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            if (course.CategoryId != dto.CategoryId)
            {
                var category = await categoryRepository.GetByIdAsync(dto.CategoryId);
                if (category == null)
                    throw new ArgumentException("Category not found");
            }

            mapper.Map(dto, course);
            course.UpdatedAt = DateTime.UtcNow;

            await courseRepository.UpdateAsync(course);
            return await GetCourseByIdAsync(id);
        }

        public async Task<bool> DeleteCourseAsync(Guid id, string userId)
        {
            var course = await courseRepository.GetByIdAsync(id);
            if (course == null) return false;

            // UserId bo'yicha Teacher ni topish
            var teacher = await teacherRepository.GetByUserIdAsync(userId);
            if (teacher == null)
                throw new UnauthorizedAccessException("Teacher profile not found");

            if (course.TeacherId != teacher.Id)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            await courseRepository.DeleteAsync(course);
            return true;
        }

        public async Task<IEnumerable<CourseDto>> GetTeacherCoursesAsync(Guid teacherId)
        {
            var courses = await courseRepository.GetCoursesByTeacherAsync(teacherId);
            return mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<bool> PublishCourseAsync(Guid id, string userId)
        {
            var course = await courseRepository.GetByIdAsync(id);
            if (course == null) return false;

            // UserId bo'yicha Teacher ni topish
            var teacher = await teacherRepository.GetByUserIdAsync(userId);
            if (teacher == null)
                throw new UnauthorizedAccessException("Teacher profile not found");

            if (course.TeacherId != teacher.Id)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            course.IsPublished = true;
            course.UpdatedAt = DateTime.UtcNow;

            await courseRepository.UpdateAsync(course);
            return true;
        }
    }
}
