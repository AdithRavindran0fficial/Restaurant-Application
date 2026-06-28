using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.MenuItems.UpdateMenuItem;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.MenuItems.UpdateMenuItem
{
    public class UpdateMenuItemRepository : IUpdateMenuItemRepository
    {
        private readonly MasterDbContext _context;

        public UpdateMenuItemRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId)
        {
            return await _context.MenuItems.Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.TenantId == tenantId && !m.IsDeleted);
        }

        public async Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.TenantId == tenantId && !c.IsDeleted);
        }

        public async Task<bool> MenuItemExistsAsync(int tenantId, string name, int categoryId, int excludeMenuItemId)
        {
            return await _context.MenuItems
                .AnyAsync(m => m.TenantId == tenantId && m.CategoryId == categoryId && m.Id != excludeMenuItemId && m.Name.ToLower() == name.ToLower() && !m.IsDeleted);
        }

        public async Task<bool> UpdateMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
