using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Categories.CreateCategory;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Categories.CreateCategory
{
    public class CreateCategoryRepository : ICreateCategoryRepository
    {
        private readonly MasterDbContext _context;

        public CreateCategoryRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExistsAsync(int tenantId, string name)
        {
            return await _context.Categories
                .AnyAsync(c => c.TenantId == tenantId && c.Name.ToLower() == name.ToLower() && !c.IsDeleted);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
