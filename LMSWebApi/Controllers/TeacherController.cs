using LMS.Logic.Exceptions;
using LMS.Logic.Services;
using LMS.Shared.Dtos.TeacherDtos;
using LMS.Shared.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
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
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers()
        {
           
                var teachers = await teacherService.GetAllTeachersAsync();
                return Ok(teachers);
        }

        [HttpGet("{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<TeacherDto>> GetTeacherById(int id)
        {

                var teacher = await teacherService.GetTeacherByIdAsync(id);
                return Ok(teacher);
    
        }

        [HttpGet("user/{userId}")]
        //[AllowAnonymous]
        public async Task<ActionResult<TeacherDto>> GetTeacherByUserId(string userId)
        {
  
                var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
                return Ok(teacher);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<TeacherDto>> CreateTeacher(CreateUserDto createUserDto)
        {

                var teacher = await teacherService.CreateTeacherAsync(createUserDto);
              return Ok(teacher);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<TeacherDto>> UpdateTeacher(int id, UpdateUserDto updateUserDto)
        {
                //var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                //var isAdmin = User.IsInRole("Admin") || User.IsInRole("Teacher");

                //if (!isAdmin)
                // {
                //    var existingTeacher = await teacherService.GetTeacherByIdAsync(id);
                //    if (existingTeacher.UserInfo.Id != currentUserId)
                //         throw new BadRequestException(message: "Only Admin and Teacher can update this teacher.");
                // }

            var teacher = await teacherService.UpdateTeacherAsync(id, updateUserDto);
                return Ok(teacher);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
                await teacherService.DeleteTeacherAsync(id);
                return NoContent();
        }
    }
}