using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Tables.RegenerateTableQr;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Tables.RegenerateTableQr
{
    public class RegenerateTableQrRepository : IRegenerateTableQrRepository
    {
        private readonly MasterDbContext _context;

        public RegenerateTableQrRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId)
        {
            return await _context.Tables
                .Where(t => t.Id == tableId && t.TenantId == tenantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateTableAsync(DiningTable table)
        {
            try
            {
                _context.Tables.Update(table);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
