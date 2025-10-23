using LMS.Data.Entities;
using LMS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        protected readonly AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Category?> GetByIdAsync(int id)
            => await context.Categories.FindAsync(id);

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await context.Categories.ToListAsync();

        public async Task<Category> AddAsync(Category entity)
        {
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Category entity)
        {
            context.Categories.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category entity)
        {
            context.Categories.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await context.Categories.AnyAsync(c => c.Id == id);

        public async Task<bool> ExistsAsync(Expression<Func<Category, bool>> predicate)
        {
            return await context.Categories.AnyAsync(predicate);
        }

        public async Task<Category?> GetCategoryWithCoursesAsync(int id)
            => await context.Categories
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<bool> CategoryExistsAsync(string name)
            => await context.Categories.AnyAsync(c => c.Name == name);
    }
}
