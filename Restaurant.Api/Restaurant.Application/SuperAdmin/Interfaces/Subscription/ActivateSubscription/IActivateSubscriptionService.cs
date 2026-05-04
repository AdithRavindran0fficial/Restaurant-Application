using Restaurant.Application.Common;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.ActivateSubscription;

public interface IActivateSubscriptionService
{
    Task<ApiResponse<bool>> ActivateSubscriptionAsync(int subscriptionId);
}
