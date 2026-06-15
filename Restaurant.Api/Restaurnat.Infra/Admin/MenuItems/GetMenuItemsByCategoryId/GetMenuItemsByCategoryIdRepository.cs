using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetMenuItemsByCategoryId;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.MenuItems.GetMenuItemsByCategoryId
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
            return await _context.MenuItems.Include(m => m.Category)
                .Where(m => m.TenantId == tenantId && m.CategoryId == categoryId && !m.IsDeleted)
                .OrderBy(m => m.DisplayOrder)
                .ThenBy(m => m.Name)
                .ToListAsync();
        }
    }
}
