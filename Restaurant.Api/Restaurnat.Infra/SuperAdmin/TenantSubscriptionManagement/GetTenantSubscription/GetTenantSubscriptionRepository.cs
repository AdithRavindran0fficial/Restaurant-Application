using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.GetTenantSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.TenantSubscriptionManagement.GetTenantSubscription;

public class GetTenantSubscriptionRepository : IGetTenantSubscriptionRepository
{
    private readonly MasterDbContext _context;

    public GetTenantSubscriptionRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId && !t.IsDeleted);
    }

    public async Task<TenantSubscription?> GetTenantSubscriptionByTenantIdAsync(int tenantId)
    {
        return await _context.TenantSubscriptions
            .Include(ts => ts.Plan)
            .Where(ts => ts.TenantId == tenantId && !ts.IsDeleted)
            .OrderByDescending(ts => ts.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
