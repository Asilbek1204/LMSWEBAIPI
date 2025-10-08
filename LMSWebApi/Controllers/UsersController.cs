using LMS.Logic.Exceptions;
using LMS.Logic.Services;
using LMS.Shared.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error getting all users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
             var user = await userService.GetUserByIdAsync(id);
            //var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            //var isAdmin = User.IsInRole("Admin");

            //if (currentUserId != id && !isAdmin)
            //throw new BadRequestException(message: "Only Admin and the user themselves can update this user.");
            

            return Ok(user);
            
        }

        //[HttpGet("role/{role}")]
        //public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(string role)
        //{
        //    try
        //    {
        //        var users = await userService.GetUsersByRoleAsync(role);
        //        return Ok(users);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.logger.LogError(ex, $"Error getting users with role {role}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
     
                logger.LogInformation("Creating new user");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        error = "Invalid model",
                        details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                // Required field validation
                if (string.IsNullOrEmpty(createUserDto.Email) || string.IsNullOrEmpty(createUserDto.Password))
                {
                    return BadRequest(new { error = "Email and Password are required" });
                }

                var user = await userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUserDto updateUserDto)
        {

                //var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                //var isAdmin = User.IsInRole("Admin");

                //if (currentUserId != id && !isAdmin)
                //    throw new BadRequestException(message: "Only Admin and the user themselves can update this user.");

            var user = await userService.UpdateUserAsync(id, updateUserDto);
                return Ok(user);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string id)
        {
                await userService.DeleteUserAsync(id);
                return NoContent();
            
        }

        [HttpPost("{userId}/assign-role/{role}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignRole(string userId, string role)
        {

                await userService.AssignRoleAsync(userId, role);
                return Ok($"Role {role} assigned to user {userId} successfully");
        }
    }
}