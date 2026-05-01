using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.ActivateTenant;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.ActivateTenant;

public class ActivateTenantRepository : IActivateTenantRepository
{
    private readonly MasterDbContext _context;

    public ActivateTenantRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId);
    }

    public async Task<bool> ActivateTenantAsync(Tenant tenant)
    {
        try
        {
            tenant.IsActive = true;
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
