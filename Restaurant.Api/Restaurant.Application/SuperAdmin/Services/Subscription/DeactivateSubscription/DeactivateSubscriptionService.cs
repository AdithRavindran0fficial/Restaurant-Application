using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeactivateSubscription;

namespace Restaurant.Application.SuperAdmin.Services.Subscription.DeactivateSubscription;

public class DeactivateSubscriptionService : IDeactivateSubscriptionService
{
    private readonly IDeactivateSubscriptionRepository _repository;

    public DeactivateSubscriptionService(IDeactivateSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> DeactivateSubscriptionAsync(int subscriptionId)
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
                    "Cannot deactivate deleted subscription plan",
                    new List<string> { $"Subscription plan with ID {subscriptionId} is marked as deleted" });
            }

            // Check if already inactive
            if (!subscriptionPlan.IsActive)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Subscription plan already inactive",
                    new List<string> { $"Subscription plan with ID {subscriptionId} is already inactive" });
            }

            // Deactivate the subscription plan
            var result = await _repository.DeactivateSubscriptionAsync(subscriptionPlan);

            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "Failed to deactivate subscription plan. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Subscription plan deactivated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ServerErrorResponse(
                "An error occurred while deactivating the subscription plan. Please try again later.");
        }
    }
}
