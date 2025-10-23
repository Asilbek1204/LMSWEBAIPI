using LMS.Logic.Services;
using LMS.Shared.Dtos;
using LMS.Shared.Dtos.TeacherDtos;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService teacherService;
        private readonly ILogger<TeachersController> logger;

        public TeachersController(ITeacherService teacherService, ILogger<TeachersController> logger)
        {
            this.teacherService = teacherService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers()
        {
            var teachers = await teacherService.GetAllAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDto>> GetTeacherById(Guid id)
        {
            var teacher = await teacherService.GetByIdAsync(id);
            return Ok(teacher);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<TeacherDto>> GetTeacherByUserId(string userId)
        {
            var teacher = await teacherService.GetByUserIdAsync(userId);
            return Ok(teacher);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TeacherDto>> UpdateTeacher(Guid id, [FromBody] UpdateTeacherDto updateTeacherDto)
        {
            var teacher = await teacherService.UpdateAsync(id, updateTeacherDto);
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(Guid id)
        {
            await teacherService.DeleteAsync(id);
            return NoContent();
        }
    }
}