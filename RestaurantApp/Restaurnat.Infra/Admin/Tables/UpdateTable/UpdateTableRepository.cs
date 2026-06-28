using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Tables.UpdateTable;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Tables.UpdateTable
{
    public class UpdateTableRepository : IUpdateTableRepository
    {
        private readonly MasterDbContext _context;

        public UpdateTableRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId)
        {
            return await _context.Tables
                .Where(t => t.Id == tableId && t.TenantId == tenantId && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> TableNumberExistsAsync(int tenantId, int tableNumber, int excludeTableId)
        {
            return await _context.Tables
                .AnyAsync(t => t.TenantId == tenantId && t.TableNumber == tableNumber && t.Id != excludeTableId && !t.IsDeleted);
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
