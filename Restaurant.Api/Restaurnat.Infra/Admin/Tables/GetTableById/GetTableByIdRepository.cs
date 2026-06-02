using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Tables.GetTableById;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Tables.GetTableById
{
    public class GetTableByIdRepository : IGetTableByIdRepository
    {
        private readonly MasterDbContext _context;

        public GetTableByIdRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId)
        {
            return await _context.Tables
                .Where(t => t.Id == tableId && t.TenantId == tenantId && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }
    }
}
