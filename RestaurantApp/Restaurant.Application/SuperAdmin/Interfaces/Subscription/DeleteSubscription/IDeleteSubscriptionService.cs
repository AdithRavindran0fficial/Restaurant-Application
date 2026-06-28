using Restaurant.Application.Common;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeleteSubscription;

public interface IDeleteSubscriptionService
{
    Task<ApiResponse<bool>> DeleteSubscriptionAsync(int subscriptionId);
}
