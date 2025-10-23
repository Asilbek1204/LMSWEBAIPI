using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetStudentEnrollments(Guid studentId)
        {
            var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(studentId);
            return Ok(enrollments);
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetCourseEnrollments(Guid courseId)
        {
            var enrollments = await _enrollmentService.GetCourseEnrollmentsAsync(courseId);
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDto>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null) return NotFound();
            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentDto>> CreateEnrollment(EnrollmentCreateDto dto)
        {
            try
            {
                var enrollment = await _enrollmentService.CreateEnrollmentAsync(dto);
                return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, enrollment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EnrollmentDto>> UpdateEnrollment(int id, EnrollmentUpdateDto dto)
        {
            var enrollment = await _enrollmentService.UpdateEnrollmentAsync(id, dto);
            if (enrollment == null) return NotFound();
            return Ok(enrollment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("check/{courseId}/{studentId}")]
        public async Task<ActionResult> CheckEnrollment(Guid courseId, Guid studentId)
        {
            var isEnrolled = await _enrollmentService.IsStudentEnrolledAsync(courseId, studentId);
            return Ok(new { isEnrolled });
        }
    }
}