using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;

public interface IGetAllSubscriptionsRepository
{
    Task<IEnumerable<SubscriptionPlan>> GetAllSubscriptionsAsync();
}
