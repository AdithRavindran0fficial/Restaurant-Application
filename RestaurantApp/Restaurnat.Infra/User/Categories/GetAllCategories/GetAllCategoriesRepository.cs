using Microsoft.EntityFrameworkCore;
using Restaurant.Application.User.Interfaces.Categories.GetAllCategories;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurnat.Infra.User.Categories.GetAllCategories
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
            var categories = await _context.Categories.Where(c => c.TenantId == tenantId && c.IsActive && !c.IsDeleted).
                OrderBy(c => c.DisplayOrder).ToListAsync();


            return categories;

        }

        public async Task<DiningTable?> GetDiningTableByQrTokenAsync(string qrToken)
        {
            var diningTable = await _context.Tables.Where(qr => qr.QrToken == qrToken && qr.IsActive && !qr.IsDeleted).FirstOrDefaultAsync();
            return diningTable;
        }
    }
}
