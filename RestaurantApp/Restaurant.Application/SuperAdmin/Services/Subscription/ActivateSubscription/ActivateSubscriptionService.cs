using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.ActivateSubscription;

namespace Restaurant.Application.SuperAdmin.Services.Subscription.ActivateSubscription;

public class ActivateSubscriptionService : IActivateSubscriptionService
{
    private readonly IActivateSubscriptionRepository _repository;

    public ActivateSubscriptionService(IActivateSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> ActivateSubscriptionAsync(int subscriptionId)
    {
        try
        {
            // Validation
            if (subscriptionId <= 0)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Invalid subscription ID",
                    new List<string> { "Subscription ID must be greater than 0" });
            }

            // Get the subscription plan
            var subscriptionPlan = await _repository.GetSubscriptionByIdAsync(subscriptionId);

            if (subscriptionPlan == null)
            {
                return ApiResponse<bool>.NotFoundResponse(
                    $"Subscription plan with ID {subscriptionId} not found");
            }

            // Check if subscription is deleted
            if (subscriptionPlan.IsDeleted)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Cannot activate deleted subscription plan",
                    new List<string> { $"Subscription plan with ID {subscriptionId} is marked as deleted. Please restore it first." });
            }

            // Check if already active
            if (subscriptionPlan.IsActive)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Subscription plan already active",
                    new List<string> { $"Subscription plan with ID {subscriptionId} is already active" });
            }

            // Activate the subscription plan
            var result = await _repository.ActivateSubscriptionAsync(subscriptionPlan);

            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "Failed to activate subscription plan. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Subscription plan activated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ServerErrorResponse(
                "An error occurred while activating the subscription plan. Please try again later.");
        }
    }
}
