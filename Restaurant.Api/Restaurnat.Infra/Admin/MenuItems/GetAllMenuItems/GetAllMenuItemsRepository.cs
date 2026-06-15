using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetAllMenuItems;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.MenuItems.GetAllMenuItems
{
    public class GetAllMenuItemsRepository : IGetAllMenuItemsRepository
    {
        private readonly MasterDbContext _context;

        public GetAllMenuItemsRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetAllMenuItemsAsync(int tenantId)
        {
            return await _context.MenuItems.Include(m => m.Category)
                .Where(m => m.TenantId == tenantId && !m.IsDeleted)
                .OrderBy(m => m.DisplayOrder)
                .ThenBy(m => m.Name)
                .ToListAsync();
        }
    }
}
