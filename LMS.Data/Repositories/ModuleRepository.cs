using LMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AppDbContext context;

        public ModuleRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Module?> GetByIdAsync(int id)
        {
            return await context.Modules.FindAsync(id);
        }

        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            return await context.Modules.ToListAsync();
        }

        public async Task<Module> AddAsync(Module entity)
        {
            await context.Modules.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Module entity)
        {
            context.Modules.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Module entity)
        {
            context.Modules.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Modules.AnyAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Module>> GetModulesByCourseAsync(Guid courseId)
        {
            return await context.Modules
                .Where(m => m.CourseId == courseId)
                .OrderBy(m => m.Order)
                .ToListAsync();
        }

        public async Task<Module?> GetModuleWithCourseAsync(int id)
        {
            return await context.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task ReorderModulesAsync(Guid courseId, int fromOrder, int toOrder)
        {
            var modules = await context.Modules
                .Where(m => m.CourseId == courseId)
                .OrderBy(m => m.Order)
                .ToListAsync();

            var moduleToMove = modules.FirstOrDefault(m => m.Order == fromOrder);
            if (moduleToMove == null) return;

            if (fromOrder < toOrder)
            {
                foreach (var module in modules.Where(m => m.Order > fromOrder && m.Order <= toOrder))
                {
                    module.Order--;
                }
            }
            else
            {
                foreach (var module in modules.Where(m => m.Order < fromOrder && m.Order >= toOrder))
                {
                    module.Order++;
                }
            }

            moduleToMove.Order = toOrder;
            await context.SaveChangesAsync();
        }
    }
}
