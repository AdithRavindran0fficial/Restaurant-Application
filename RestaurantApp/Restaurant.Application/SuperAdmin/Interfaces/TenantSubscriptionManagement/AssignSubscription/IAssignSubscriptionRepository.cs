using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.AssignSubscription
{
    public interface IAssignSubscriptionRepository
    {
        Task<Tenant?> GetTenantByIdAsync(int tenantId);
        Task<SubscriptionPlan?> GetSubscriptionPlanByIdAsync(int planId);
        Task<TenantSubscription?> GetActiveSubscriptionByTenantIdAsync(int tenantId);
        Task<TenantSubscription> CreateTenantSubscriptionAsync(TenantSubscription tenantSubscription);
        Task DeactivatePreviousSubscriptionsAsync(int tenantId);
    }
}
