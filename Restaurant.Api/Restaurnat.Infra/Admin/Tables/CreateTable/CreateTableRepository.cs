using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Tables.CreateTable;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Tables.CreateTable
{
    public class CreateTableRepository : ICreateTableRepository
    {
        private readonly MasterDbContext _context;

        public CreateTableRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TableNumberExistsAsync(int tenantId, int tableNumber)
        {
            return await _context.Tables
                .AnyAsync(t => t.TenantId == tenantId && t.TableNumber == tableNumber && !t.IsDeleted);
        }

        public async Task<DiningTable> CreateTableAsync(DiningTable table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
            return table;
        }
    }
}
