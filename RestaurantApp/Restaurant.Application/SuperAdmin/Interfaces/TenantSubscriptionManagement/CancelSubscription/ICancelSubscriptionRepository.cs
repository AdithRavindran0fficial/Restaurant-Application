using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.CancelSubscription
{
    public interface ICancelSubscriptionRepository
    {
        Task<Tenant?> GetTenantByIdAsync(int tenantId);
        Task<TenantSubscription?> GetActiveSubscriptionByTenantIdAsync(int tenantId);
        Task<bool> CancelSubscriptionAsync(TenantSubscription subscription);
    }
}
