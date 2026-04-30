using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.SoftDeleteTenant;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.SoftDeleteTenant;

public class SoftDeleteTenantRepository : ISoftDeleteTenantRepository
{
    private readonly MasterDbContext _context;

    public SoftDeleteTenantRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId);
    }

    public async Task<bool> SoftDeleteTenantAsync(Tenant tenant)
    {
        try
        {
            tenant.IsDeleted = true;
            tenant.UpdatedAt = DateTime.UtcNow;

            _context.Tenants.Update(tenant);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch
        {
            return false;
        }
    }
}
