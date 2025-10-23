using AutoMapper;
using LMS.Data.Entities;
using LMS.Data.Repositories;
using LMS.Data.Repositories.Interfaces;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;

namespace LMS.Logic.Services
{
    public class ModuleService(
            IMapper mapper,
            IModuleRepository moduleRepository,
            ICourseRepository courseRepository) : IModuleService
    {
        public async Task<IEnumerable<ModuleDto>> GetModulesByCourseAsync(Guid courseId)
        {
            var modules = await moduleRepository.GetModulesByCourseAsync(courseId);
            var moduleDtos = mapper.Map<IEnumerable<ModuleDto>>(modules);

            // Set course title for each module
            var course = await courseRepository.GetByIdAsync(courseId);
            foreach (var moduleDto in moduleDtos)
            {
                moduleDto.CourseTitle = course?.Title ?? string.Empty;
            }

            return moduleDtos;
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(int id)
        {
            var module = await moduleRepository.GetModuleWithCourseAsync(id);
            if (module == null) return null;

            var moduleDto = mapper.Map<ModuleDto>(module);
            moduleDto.CourseTitle = module.Course?.Title ?? string.Empty;
            return moduleDto;
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleCreateDto dto, Guid teacherId)
        {
            var course = await courseRepository.GetByIdAsync(dto.CourseId);
            if (course == null)
                throw new ArgumentException("Course not found");

            if (course.TeacherId != teacherId)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            var module = mapper.Map<Module>(dto);
            var createdModule = await moduleRepository.AddAsync(module);

            var moduleDto = mapper.Map<ModuleDto>(createdModule);
            moduleDto.CourseTitle = course.Title;
            return moduleDto;
        }

        public async Task<ModuleDto?> UpdateModuleAsync(int id, ModuleUpdateDto dto, Guid teacherId)
        {
            var module = await moduleRepository.GetModuleWithCourseAsync(id);
            if (module == null) return null;

            if (module.Course.TeacherId != teacherId)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            mapper.Map(dto, module);
            await moduleRepository.UpdateAsync(module);

            return await GetModuleByIdAsync(id);
        }

        public async Task<bool> DeleteModuleAsync(int id, Guid teacherId)
        {
            var module = await moduleRepository.GetModuleWithCourseAsync(id);
            if (module == null) return false;

            if (module.Course.TeacherId != teacherId)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            await moduleRepository.DeleteAsync(module);
            return true;
        }

        public async Task ReorderModulesAsync(Guid courseId, int fromOrder, int toOrder, Guid teacherId)
        {
            var course = await courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new ArgumentException("Course not found");

            if (course.TeacherId != teacherId)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            await moduleRepository.ReorderModulesAsync(courseId, fromOrder, toOrder);
        }
    }
}
