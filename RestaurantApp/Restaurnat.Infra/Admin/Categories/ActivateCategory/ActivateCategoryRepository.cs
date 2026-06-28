using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Categories.ActivateCategory;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Categories.ActivateCategory
{
    public class ActivateCategoryRepository : IActivateCategoryRepository
    {
        private readonly MasterDbContext _context;

        public ActivateCategoryRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId)
        {
            return await _context.Categories
                .Where(c => c.Id == categoryId && c.TenantId == tenantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ActivateCategoryAsync(Category category)
        {
            try
            {
                category.IsActive = true;
                category.UpdatedAt = DateTime.UtcNow;

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
