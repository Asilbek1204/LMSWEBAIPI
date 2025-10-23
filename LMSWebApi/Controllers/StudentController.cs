using LMS.Logic.Services;
using LMS.Shared.Dtos;
using LMS.Shared.Dtos.StudentDtos;
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
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            var students = await studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentById(Guid id)
        {
            var student = await studentService.GetByIdAsync(id);
            return Ok(student);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<StudentDto>> GetStudentByUserId(string userId)
        {
            var student = await studentService.GetByUserIdAsync(userId);
            return Ok(student);
        }

        [HttpGet("studentid/{studentId}")]
        public async Task<ActionResult<StudentDto>> GetStudentByStudentId(string studentId)
        {
            var student = await studentService.GetByStudentIdAsync(studentId);
            return Ok(student);
        }

        [HttpPut("{id}/profile")]
        public async Task<ActionResult<StudentDto>> UpdateStudentProfile(Guid id, [FromBody] UpdateStudentProfileDto updateProfileDto)
        {
            var student = await studentService.UpdateProfileAsync(id, updateProfileDto);
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(Guid id)
        {
            await studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}