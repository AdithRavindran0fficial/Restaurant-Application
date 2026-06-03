using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Tables.DeactivateTable;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Tables.DeactivateTable
{
    public class DeactivateTableRepository : IDeactivateTableRepository
    {
        private readonly MasterDbContext _context;

        public DeactivateTableRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId)
        {
            return await _context.Tables
                .Where(t => t.Id == tableId && t.TenantId == tenantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeactivateTableAsync(DiningTable table)
        {
            try
            {
                table.IsActive = false;
                table.UpdatedAt = DateTime.UtcNow;

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
