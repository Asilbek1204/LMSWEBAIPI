using LMS.Logic.Services;
using LMS.Shared.Dtos;
using LMS.Shared.Dtos.TeacherDtos;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers()
        {
            var teachers = await teacherService.GetAllAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TeacherDto>> GetTeacherById(Guid id)
        {
            var teacher = await teacherService.GetByIdAsync(id);
            return Ok(teacher);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<TeacherDto>> GetTeacherByUserId(string userId)
        {
            var teacher = await teacherService.GetByUserIdAsync(userId);
            return Ok(teacher);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<TeacherDto>> UpdateTeacher(Guid id, [FromBody] UpdateTeacherDto updateTeacherDto)
        {
            var teacher = await teacherService.UpdateAsync(id, updateTeacherDto);
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult> DeleteTeacher(Guid id)
        {
            await teacherService.DeleteAsync(id);
            return NoContent();
        }
    }
}