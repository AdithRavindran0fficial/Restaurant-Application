using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.GetAllTenants;

public class TenantRepository : ITenantRepository
{
    private readonly MasterDbContext _context;

    public TenantRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tenant>> GetAllTenantsAsync()
    {
        return await _context.Tenants
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId && !t.IsDeleted);
    }
}
