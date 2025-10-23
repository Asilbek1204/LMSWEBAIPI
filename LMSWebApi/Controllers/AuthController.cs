using LMS.Logic.Services;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos;
using LMS.Shared.Dtos.AuthDtos;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await authService.RegisterUserAsync(dto);
            return Ok(response);
        }

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await authService.RegisterStudentAsync(dto);
            return Ok(response);
        }

        [HttpPost("register-teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await authService.RegisterTeacherAsync(dto);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await authService.LoginAsync(dto);
            return Ok(response);
        }
    }
}