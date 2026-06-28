using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.MenuItems.CreateMenuItem;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.MenuItems.CreateMenuItem
{
    public class CreateMenuItemRepository : ICreateMenuItemRepository
    {
        private readonly MasterDbContext _context;

        public CreateMenuItemRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<bool> MenuItemExistsAsync(int tenantId, string name, int categoryId)
        {
            var exists = await  _context.MenuItems
                .AnyAsync(m => m.TenantId == tenantId && m.Name.ToLower() == name.ToLower() && m.CategoryId == categoryId);
            return exists;
        }

        public async Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.TenantId == tenantId && c.Id == categoryId && !c.IsDeleted);
            return category;
        }

        public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }
    }
}
