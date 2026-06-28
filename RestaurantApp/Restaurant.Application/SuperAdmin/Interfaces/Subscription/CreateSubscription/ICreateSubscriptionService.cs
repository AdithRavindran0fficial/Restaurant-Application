using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;

public interface ICreateSubscriptionService
{
    Task<ApiResponse<SubscriptionPlanDto>> CreateSubscriptionAsync(CreateSubscriptionDto createDto);
}
