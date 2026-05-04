using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetSubscriptionById;

public interface IGetSubscriptionByIdService
{
    Task<ApiResponse<SubscriptionPlanDto>> GetSubscriptionByIdAsync(int subscriptionId);
}
