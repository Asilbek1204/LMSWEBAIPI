using LMS.Shared.Dtos.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Logic.Services.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleDto>> GetModulesByCourseAsync(Guid courseId);
        Task<ModuleDto?> GetModuleByIdAsync(int id);
        Task<ModuleDto> CreateModuleAsync(ModuleCreateDto dto, Guid teacherId);
        Task<ModuleDto?> UpdateModuleAsync(int id, ModuleUpdateDto dto, Guid teacherId);
        Task<bool> DeleteModuleAsync(int id, Guid teacherId);
        Task ReorderModulesAsync(Guid courseId, int fromOrder, int toOrder, Guid teacherId);
    }
}
