using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.GetTenantSubscription;

public interface IGetTenantSubscriptionRepository
{
    Task<Tenant?> GetTenantByIdAsync(int tenantId);
    Task<TenantSubscription?> GetTenantSubscriptionByTenantIdAsync(int tenantId);
}
