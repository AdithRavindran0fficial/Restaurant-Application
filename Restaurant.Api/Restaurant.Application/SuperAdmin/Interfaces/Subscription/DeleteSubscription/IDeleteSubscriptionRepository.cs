using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeleteSubscription;

public interface IDeleteSubscriptionRepository
{
    Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId);
    Task<bool> DeleteSubscriptionAsync(SubscriptionPlan subscriptionPlan);
}
