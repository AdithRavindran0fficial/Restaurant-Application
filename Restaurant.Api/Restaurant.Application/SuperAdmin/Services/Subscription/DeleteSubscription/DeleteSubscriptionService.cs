using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeleteSubscription;

namespace Restaurant.Application.SuperAdmin.Services.Subscription.DeleteSubscription;

public class DeleteSubscriptionService : IDeleteSubscriptionService
{
    private readonly IDeleteSubscriptionRepository _repository;

    public DeleteSubscriptionService(IDeleteSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> DeleteSubscriptionAsync(int subscriptionId)
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

            // Check if already deleted
            if (subscriptionPlan.IsDeleted)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Subscription plan already deleted",
                    new List<string> { $"Subscription plan with ID {subscriptionId} is already marked as deleted" });
            }

            // Soft delete the subscription plan
            var result = await _repository.DeleteSubscriptionAsync(subscriptionPlan);

            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "Failed to delete subscription plan. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Subscription plan deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ServerErrorResponse(
                "An error occurred while deleting the subscription plan. Please try again later.");
        }
    }
}
