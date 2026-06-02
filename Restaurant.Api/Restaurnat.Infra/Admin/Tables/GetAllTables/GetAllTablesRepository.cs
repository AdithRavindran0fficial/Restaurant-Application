using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Tables.GetAllTables;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Tables.GetAllTables
{
    public class GetAllTablesRepository : IGetAllTablesRepository
    {
        private readonly MasterDbContext _context;

        public GetAllTablesRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<List<DiningTable>> GetAllTablesAsync(int tenantId)
        {
            return await _context.Tables
                .Where(t => t.TenantId == tenantId && !t.IsDeleted)
                .OrderBy(t => t.TableNumber)
                .ToListAsync();
        }
    }
}
