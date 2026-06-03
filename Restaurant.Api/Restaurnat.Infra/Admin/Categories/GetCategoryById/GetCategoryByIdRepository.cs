using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Categories.GetCategoryById;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Categories.GetCategoryById
{
    public class GetCategoryByIdRepository : IGetCategoryByIdRepository
    {
        private readonly MasterDbContext _context;

        public GetCategoryByIdRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId)
        {
            return await _context.Categories
                .Where(c => c.Id == categoryId && c.TenantId == tenantId && !c.IsDeleted)
                .FirstOrDefaultAsync();
        }
    }
}
