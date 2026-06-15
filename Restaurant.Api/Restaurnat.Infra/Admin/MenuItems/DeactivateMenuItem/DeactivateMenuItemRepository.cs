using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.MenuItems.DeactivateMenuItem;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.MenuItems.DeactivateMenuItem
{
    public class DeactivateMenuItemRepository : IDeactivateMenuItemRepository
    {
        private readonly MasterDbContext _context;

        public DeactivateMenuItemRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId)
        {
            return await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.TenantId == tenantId);
        }

        public async Task<bool> DeactivateMenuItemAsync(MenuItem menuItem)
        {
            menuItem.IsActive = false;
            menuItem.UpdatedAt = System.DateTime.UtcNow;
            _context.MenuItems.Update(menuItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
