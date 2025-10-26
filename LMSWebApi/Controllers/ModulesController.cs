using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService moduleService;

        public ModulesController(IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModulesByCourse(Guid courseId)
        {
            var modules = await moduleService.GetModulesByCourseAsync(courseId);
            return Ok(modules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDto>> GetModule(int id)
        {
            var module = await moduleService.GetModuleByIdAsync(id);
            if (module == null) return NotFound();
            return Ok(module);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<ModuleDto>> CreateModule(ModuleCreateDto dto)
        {
            try
            {
                var teacherId = Guid.NewGuid(); // Temporary - auth qilganda o'zgartirish kerak
                var module = await moduleService.CreateModuleAsync(dto, teacherId);
                return CreatedAtAction(nameof(GetModule), new { id = module.Id }, module);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<ModuleDto>> UpdateModule(int id, ModuleUpdateDto dto)
        {
            try
            {
                var teacherId = Guid.NewGuid(); // Temporary - auth qilganda o'zgartirish kerak
                var module = await moduleService.UpdateModuleAsync(id, dto, teacherId);
                if (module == null) return NotFound();
                return Ok(module);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            try
            {
                var teacherId = Guid.NewGuid(); // Temporary - auth qilganda o'zgartirish kerak
                var result = await moduleService.DeleteModuleAsync(id, teacherId);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("{courseId}/reorder")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> ReorderModules(Guid courseId, [FromBody] ReorderRequest request)
        {
            try
            {
                var teacherId = Guid.NewGuid(); // Temporary - auth qilganda o'zgartirish kerak
                await moduleService.ReorderModulesAsync(courseId, request.FromOrder, request.ToOrder, teacherId);
                return Ok(new { message = "Modules reordered successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        public class ReorderRequest
        {
            public int FromOrder { get; set; }
            public int ToOrder { get; set; }
        }
    }
}