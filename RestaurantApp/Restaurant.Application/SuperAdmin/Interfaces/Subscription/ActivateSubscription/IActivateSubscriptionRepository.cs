using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.ActivateSubscription;

public interface IActivateSubscriptionRepository
{
    Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId);
    Task<bool> ActivateSubscriptionAsync(SubscriptionPlan subscriptionPlan);
}
