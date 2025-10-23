using LMS.Data.Repositories;
using LMS.Logic.Exceptions;
using LMS.Logic.Services;
using LMS.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService,
        ILogger<UsersController> logger,
        IUserRepository userRepository) : ControllerBase

    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await userService.GetByEmailAsync(email);
            return Ok(user);
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(string role)
        {
            var users = await userService.GetByRoleAsync(role);
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await userService.UpdateAsync(id, updateUserDto);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await userService.DeleteAsync(id);
            return NoContent();
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] string newRole)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var currentUserRole = User.FindFirst("role")?.Value;

            // 1️⃣ Admin yoki o‘zi bo‘lishi shart
            if (currentUserRole != "Admin" && currentUserId != id.ToString())
                return Forbid("❌ Sizda bu amalni bajarish uchun ruxsat yo‘q.");

            var user = await userRepository.GetByIdAsync(id.ToString());
            if (user == null)
                throw new NotFoundException("❌ User not found");
            if (!user.Roles.Contains(newRole))
                user.Roles.Add(newRole);
            await userRepository.UpdateAsync(user);

            return Ok(new
            {
                message = "✅ User role successfully updated",
                user.Id,
                user.UserName,
                user.Roles
            });
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("{id}/add-role")]
        public async Task<IActionResult> AddRoleToUser(Guid id, [FromBody] string roleToAdd)
        {
            var user = await userRepository.GetByIdAsync(id.ToString());
            if (user == null)
                throw new NotFoundException("❌ User not found");

            bool hasRole = user.Roles != null && user.Roles.Any(r =>
            string.Equals(r, roleToAdd, StringComparison.OrdinalIgnoreCase));

            if (hasRole)
                return BadRequest("⚠️ User already has this role");

            if (user.Roles == null)
                user.Roles = new List<string>();

            user.Roles.Add(roleToAdd);
            await userRepository.UpdateAsync(user);

            return Ok(new
            {
                message = "✅ Role successfully added to user",
                user.Id,
                user.UserName,
                user.Roles
            });
        }
    }
}