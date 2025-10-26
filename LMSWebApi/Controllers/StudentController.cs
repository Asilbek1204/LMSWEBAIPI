using LMS.Logic.Services;
using LMS.Shared.Dtos;
using LMS.Shared.Dtos.StudentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly ILogger<StudentsController> logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            this.studentService = studentService;
            this.logger = logger;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            var students = await studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<StudentDto>> GetStudentById(Guid id)
        {
            var student = await studentService.GetByIdAsync(id);
            return Ok(student);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<StudentDto>> GetStudentByUserId(string userId)
        {
            var student = await studentService.GetByUserIdAsync(userId);
            return Ok(student);
        }

        [HttpGet("studentid/{studentId}")]
        [Authorize]
        public async Task<ActionResult<StudentDto>> GetStudentByStudentId(string studentId)
        {
            var student = await studentService.GetByStudentIdAsync(studentId);
            return Ok(student);
        }

        [HttpPut("{id}/profile")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<ActionResult<StudentDto>> UpdateStudentProfile(Guid id, [FromBody] UpdateStudentProfileDto updateProfileDto)
        {
            var student = await studentService.UpdateProfileAsync(id, updateProfileDto);
            return Ok(student);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteStudent(Guid id)
        {
            await studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}