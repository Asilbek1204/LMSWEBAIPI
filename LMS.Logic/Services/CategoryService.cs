using AutoMapper;
using LMS.Data.Entities;
using LMS.Data.Repositories.Interfaces;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos.EntityDtos;

namespace LMS.Logic.Services
{
    public class CategoryService(IMapper mapper, ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await categoryRepository.GetAllAsync();
            var categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);

            foreach (var categoryDto in categoryDtos)
            {
                var category = await categoryRepository.GetCategoryWithCoursesAsync(categoryDto.Id);
                categoryDto.CourseCount = category?.Courses?.Count ?? 0;
            }

            return categoryDtos;
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await categoryRepository.GetCategoryWithCoursesAsync(id);
            if (category == null) return null;

            var categoryDto = mapper.Map<CategoryDto>(category);
            categoryDto.CourseCount = category.Courses?.Count ?? 0;
            return categoryDto;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            if (await categoryRepository.CategoryExistsAsync(dto.Name))
                throw new ArgumentException($"Category with name '{dto.Name}' already exists");

            var category = mapper.Map<Category>(dto);
            var createdCategory = await categoryRepository.AddAsync(category);
            return mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            // Yangi nom boshqa kategoriyada mavjudligini tekshirish
            if (await categoryRepository.ExistsAsync(c => c.Name == dto.Name && c.Id != id))
                throw new ArgumentException($"Category with name '{dto.Name}' already exists");

            mapper.Map(dto, category);
            await categoryRepository.UpdateAsync(category);
            return await GetCategoryByIdAsync(id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await categoryRepository.GetCategoryWithCoursesAsync(id);
            if (category == null) return false;

            if (category.Courses.Any())
                throw new InvalidOperationException("Cannot delete category that has courses");

            await categoryRepository.DeleteAsync(category);
            return true;
        }
    }
}
