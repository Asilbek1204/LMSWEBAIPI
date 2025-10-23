using LMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> AddAsync(Category entity);
        Task UpdateAsync(Category entity);
        Task DeleteAsync(Category entity);
        Task<bool> ExistsAsync(int id);

        // Lambda expression qabul qiladigan ExistsAsync methodi
        Task<bool> ExistsAsync(Expression<Func<Category, bool>> predicate);

        Task<Category?> GetCategoryWithCoursesAsync(int id);
        Task<bool> CategoryExistsAsync(string name);
    }
}
