using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.MenuItems.DeleteMenuItem;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.MenuItems.DeleteMenuItem
{
    public class DeleteMenuItemRepository : IDeleteMenuItemRepository
    {
        private readonly MasterDbContext _context;

        public DeleteMenuItemRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId)
        {
            return await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.TenantId == tenantId);
        }

        public async Task<bool> SoftDeleteMenuItemAsync(MenuItem menuItem)
        {
            menuItem.IsDeleted = true;
            menuItem.UpdatedAt = System.DateTime.UtcNow;
            _context.MenuItems.Update(menuItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
