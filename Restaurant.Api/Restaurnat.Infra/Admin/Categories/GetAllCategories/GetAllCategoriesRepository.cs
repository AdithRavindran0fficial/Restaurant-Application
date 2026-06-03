using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Categories.GetAllCategories;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Categories.GetAllCategories
{
    public class GetAllCategoriesRepository : IGetAllCategoriesRepository
    {
        private readonly MasterDbContext _context;

        public GetAllCategoriesRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync(int tenantId)
        {
            return await _context.Categories
                .Where(c => c.TenantId == tenantId && !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }
    }
}
