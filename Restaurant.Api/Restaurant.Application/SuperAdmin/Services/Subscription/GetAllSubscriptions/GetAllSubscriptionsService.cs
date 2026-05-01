using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;

namespace Restaurant.Application.SuperAdmin.Services.Subscription.GetAllSubscriptions;

public class GetAllSubscriptionsService : IGetAllSubscriptionsService
{
    private readonly IGetAllSubscriptionsRepository _repository;

    public GetAllSubscriptionsService(IGetAllSubscriptionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<SubscriptionPlanDto>>> GetAllSubscriptionsAsync()
    {
        try
        {
            var subscriptions = await _repository.GetAllSubscriptionsAsync();

            var subscriptionDtos = subscriptions.Select(s => new SubscriptionPlanDto
            {
                Id = s.Id,
                Name = s.Name,
                PriceMonthly = s.PriceMonthly,
                PriceYearly = s.PriceYearly,
                MaxTables = s.MaxTables,
                MaxStaff = s.MaxStaff,
                StorageLimitMb = s.StorageLimitMb,
                HasNotifications = s.HasNotifications,
                HasAnalytics = s.HasAnalytics,
                FeaturesJson = s.FeaturesJson,
                Description = s.Description,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            if (!subscriptionDtos.Any())
            {
                return ApiResponse<IEnumerable<SubscriptionPlanDto>>.SuccessResponse(
                    subscriptionDtos,
                    "No subscription plans found");
            }

            return ApiResponse<IEnumerable<SubscriptionPlanDto>>.SuccessResponse(
                subscriptionDtos,
                "Subscription plans retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<SubscriptionPlanDto>>.ServerErrorResponse(
                "An error occurred while retrieving subscription plans. Please try again later.");
        }
    }
}
