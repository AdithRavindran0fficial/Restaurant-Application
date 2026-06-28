using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetSubscriptionById;

namespace Restaurant.Application.SuperAdmin.Services.Subscription.GetSubscriptionById;

public class GetSubscriptionByIdService : IGetSubscriptionByIdService
{
    private readonly IGetSubscriptionByIdRepository _repository;

    public GetSubscriptionByIdService(IGetSubscriptionByIdRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<SubscriptionPlanDto>> GetSubscriptionByIdAsync(int subscriptionId)
    {
        try
        {
            // Validation
            if (subscriptionId <= 0)
            {
                return ApiResponse<SubscriptionPlanDto>.ValidationErrorResponse(
                    "Invalid subscription ID",
                    new List<string> { "Subscription ID must be greater than 0" });
            }

            var subscriptionPlan = await _repository.GetSubscriptionByIdAsync(subscriptionId);

            if (subscriptionPlan == null)
            {
                return ApiResponse<SubscriptionPlanDto>.NotFoundResponse(
                    $"Subscription plan with ID {subscriptionId} not found");
            }

            // Map to DTO
            var subscriptionDto = new SubscriptionPlanDto
            {
                Id = subscriptionPlan.Id,
                Name = subscriptionPlan.Name,
                PriceMonthly = subscriptionPlan.PriceMonthly,
                PriceYearly = subscriptionPlan.PriceYearly,
                MaxTables = subscriptionPlan.MaxTables,
                MaxStaff = subscriptionPlan.MaxStaff,
                StorageLimitMb = subscriptionPlan.StorageLimitMb,
                HasNotifications = subscriptionPlan.HasNotifications,
                HasAnalytics = subscriptionPlan.HasAnalytics,
                FeaturesJson = subscriptionPlan.FeaturesJson,
                Description = subscriptionPlan.Description,
                IsActive = subscriptionPlan.IsActive,
                CreatedAt = subscriptionPlan.CreatedAt,
                UpdatedAt = subscriptionPlan.UpdatedAt
            };

            return ApiResponse<SubscriptionPlanDto>.SuccessResponse(
                subscriptionDto,
                "Subscription plan retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<SubscriptionPlanDto>.ServerErrorResponse(
                "An error occurred while retrieving the subscription plan. Please try again later.");
        }
    }
}
