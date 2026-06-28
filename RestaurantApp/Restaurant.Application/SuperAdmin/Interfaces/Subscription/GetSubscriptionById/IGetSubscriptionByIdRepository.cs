using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetSubscriptionById;

public interface IGetSubscriptionByIdRepository
{
    Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId);
}
