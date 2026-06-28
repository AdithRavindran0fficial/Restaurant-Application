using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeactivateSubscription;

public interface IDeactivateSubscriptionRepository
{
    Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId);
    Task<bool> DeactivateSubscriptionAsync(SubscriptionPlan subscriptionPlan);
}
