using Restaurant.Application.Common;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeactivateSubscription;

public interface IDeactivateSubscriptionService
{
    Task<ApiResponse<bool>> DeactivateSubscriptionAsync(int subscriptionId);
}
