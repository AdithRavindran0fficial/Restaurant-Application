using Microsoft.EntityFrameworkCore;
using Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemsByCategoryId;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurnat.Infra.User.MenuItems.GetMenuItemsByCategoryId
{
    public class GetMenuItemsByCategoryIdRepository : IGetMenuItemsByCategoryIdRepository
    {
        private readonly MasterDbContext _context;
        public GetMenuItemsByCategoryIdRepository(MasterDbContext context)
        {
            _context = context;
        }
        public async Task<List<MenuItem>> GetMenuItemsByCategoryIdAsync(int tenantId, int categoryId)
        {
            var menuItems = await _context.MenuItems
                .Include(c => c.Category)
                .Where(c => c.TenantId == tenantId && c.CategoryId == categoryId && c.IsAvailable && !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
            return menuItems;
        }

        public async Task<DiningTable?> GetDiningTableByQrTokenAsync(string qrToken)
        {
            var table = await _context.Tables.FirstOrDefaultAsync(d => d.QrToken == qrToken && d.IsActive && !d.IsDeleted);
            return table;
        }
    }
}
