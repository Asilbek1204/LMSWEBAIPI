using LMS.Logic.Services;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController(ICourseService courseService,ITeacherService teacherService) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetCourses(
             [FromQuery] string? title = null,
             [FromQuery] Guid? teacherId = null,
             [FromQuery] int? categoryId = null,
             [FromQuery] string? sortBy = "title",
             [FromQuery] bool sortDescending = false,
             [FromQuery] int page = 1,
             [FromQuery] int pageSize = 10)
        {
            var (courses, totalCount) = await courseService.GetAllCoursesAsync(
                title, teacherId, categoryId, sortBy, sortDescending, page, pageSize);

            return Ok(new
            {
                Items = courses,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
        {
            var course = await courseService.GetCourseByIdAsync(id);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseCreateDto dto)
        {
            // Faqat 1 qator - BaseController dan GetCurrentUserId() ishlatish
            var userId = GetCurrentUserId();
            var course = await courseService.CreateCourseAsync(dto, userId);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<CourseDto>> UpdateCourse(Guid id, CourseUpdateDto dto)
        {
            var userId = GetCurrentUserId();
            var course = await courseService.UpdateCourseAsync(id, dto, userId);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await courseService.DeleteCourseAsync(id, userId);
            return result ? NoContent() : NotFound();
        }

        [HttpPost("{id}/publish")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> PublishCourse(Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await courseService.PublishCourseAsync(id, userId);
            return result ? Ok(new { message = "Course published" }) : NotFound();
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetTeacherCourses(Guid teacherId)
        {
            var courses = await courseService.GetTeacherCoursesAsync(teacherId);
            return Ok(courses);
        }

        [HttpGet("my-courses")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetMyCourses()
        {
            var userId = GetCurrentUserId();

            // TeacherService ni to'g'ridan-to'g'ri ishlatish
            var teacher = await teacherService.GetTeacherByUserIdAsync(userId);

            if (teacher == null)
                return NotFound(new { error = "Teacher profile not found" });

            var courses = await courseService.GetTeacherCoursesAsync(teacher.Id);
            return Ok(courses);
        }

        // Debug uchun endpoint
        [HttpGet("debug-user")]
        public IActionResult DebugUser()
        {
            var userId = GetCurrentUserId();
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new
            {
                UserId = userId,
                Claims = claims,
                IsAuthenticated = User.Identity.IsAuthenticated
            });
        }
    }
}