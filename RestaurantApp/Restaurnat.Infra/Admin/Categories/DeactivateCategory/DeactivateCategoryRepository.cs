using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Categories.DeactivateCategory;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Categories.DeactivateCategory
{
    public class DeactivateCategoryRepository : IDeactivateCategoryRepository
    {
        private readonly MasterDbContext _context;

        public DeactivateCategoryRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId)
        {
            return await _context.Categories
                .Where(c => c.Id == categoryId && c.TenantId == tenantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeactivateCategoryAsync(Category category)
        {
            try
            {
                category.IsActive = false;
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
