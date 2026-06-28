using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;

namespace Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;

public interface IGetAllSubscriptionsService
{
    Task<ApiResponse<IEnumerable<SubscriptionPlanDto>>> GetAllSubscriptionsAsync();
}
