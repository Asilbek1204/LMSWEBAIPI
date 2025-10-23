using LMS.Shared.Dtos.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Logic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryUpdateDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
