using Microsoft.EntityFrameworkCore;
using Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemById;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Threading.Tasks;

namespace Restaurnat.Infra.User.MenuItems.GetMenuItemById
{
    public class GetMenuItemByIdRepository : IGetMenuItemByIdRepository
    {
        private readonly MasterDbContext _context;

        public GetMenuItemByIdRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId)
        {
            return await _context.MenuItems.Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.TenantId == tenantId && !m.IsDeleted);
        }

        public async Task<DiningTable?> GetDiningTableByQrTokenAsync(string qrToken)
        {
            return await _context.Tables.FirstOrDefaultAsync(t => t.QrToken == qrToken && t.IsActive && !t.IsDeleted);
        }
    }
}
