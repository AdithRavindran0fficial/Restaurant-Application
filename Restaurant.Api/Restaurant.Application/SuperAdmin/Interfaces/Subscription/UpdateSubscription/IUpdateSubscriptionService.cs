using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;

public interface IUpdateSubscriptionService
{
    Task<ApiResponse<SubscriptionPlanDto>> UpdateSubscriptionAsync(int subscriptionId, UpdateSubscriptionDto updateDto);
}
