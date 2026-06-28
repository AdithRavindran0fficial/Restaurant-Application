using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;

public interface ICreateSubscriptionRepository
{
    Task<SubscriptionPlan?> GetSubscriptionByNameAsync(string name);
    Task<SubscriptionPlan> CreateSubscriptionAsync(SubscriptionPlan subscriptionPlan);
}
