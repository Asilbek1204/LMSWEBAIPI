using LMS.Logic.Services;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController(ICourseService courseService) : ControllerBase
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
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseCreateDto dto)
        {
            try
            {
                // Authentication dan User ID ni olish
                var userId = GetCurrentUserId();
                var course = await courseService.CreateCourseAsync(dto, userId);
                return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> UpdateCourse(Guid id, CourseUpdateDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var course = await courseService.UpdateCourseAsync(id, dto, userId);
                if (course == null) return NotFound();
                return Ok(course);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await courseService.DeleteCourseAsync(id, userId);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> PublishCourse(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await courseService.PublishCourseAsync(id, userId);
                if (!result) return NotFound();
                return Ok(new { message = "Course published successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
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
            try
            {
                var userId = GetCurrentUserId();

                // UserId bo'yicha Teacher ni topish
                var teacherService = HttpContext.RequestServices.GetService<ITeacherService>();
                var teacher = await teacherService.GetTeacherByUserIdAsync(userId);

                if (teacher == null)
                    return NotFound(new { error = "Teacher profile not found" });

                var courses = await courseService.GetTeacherCoursesAsync(teacher.Id);
                return Ok(courses);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
        private string GetCurrentUserId()
        {
            // JWT token dan User ID ni olish
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }

            throw new UnauthorizedAccessException("User not authenticated");
        }
    }
}