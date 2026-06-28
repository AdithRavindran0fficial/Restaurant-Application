using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Categories.UpdateCategory;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Categories.UpdateCategory
{
    public class UpdateCategoryRepository : IUpdateCategoryRepository
    {
        private readonly MasterDbContext _context;

        public UpdateCategoryRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId)
        {
            return await _context.Categories
                .Where(c => c.Id == categoryId && c.TenantId == tenantId && !c.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<Category?> GetCategoryByNameAsync(int tenantId, string name, int excludeId)
        {
            return await _context.Categories
                .Where(c => c.TenantId == tenantId && c.Name.ToLower() == name.ToLower() && c.Id != excludeId && !c.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
