using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;

public interface IUpdateSubscriptionRepository
{
    Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId);
    Task<SubscriptionPlan?> GetSubscriptionByNameAsync(string name, int excludeId);
    Task<bool> UpdateSubscriptionAsync(SubscriptionPlan subscriptionPlan);
}
