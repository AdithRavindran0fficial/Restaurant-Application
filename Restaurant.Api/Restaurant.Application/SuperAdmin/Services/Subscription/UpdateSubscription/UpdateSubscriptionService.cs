using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;

namespace Restaurant.Application.SuperAdmin.Services.Subscription.UpdateSubscription;

public class UpdateSubscriptionService : IUpdateSubscriptionService
{
    private readonly IUpdateSubscriptionRepository _repository;

    public UpdateSubscriptionService(IUpdateSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<SubscriptionPlanDto>> UpdateSubscriptionAsync(int subscriptionId, UpdateSubscriptionDto updateDto)
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

            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(updateDto.Name))
            {
                validationErrors.Add("Subscription plan name is required");
            }

            if (updateDto.PriceMonthly < 0)
            {
                validationErrors.Add("Monthly price cannot be negative");
            }

            if (updateDto.PriceYearly.HasValue && updateDto.PriceYearly.Value < 0)
            {
                validationErrors.Add("Yearly price cannot be negative");
            }

            if (updateDto.MaxTables <= 0)
            {
                validationErrors.Add("Max tables must be greater than 0");
            }

            if (updateDto.MaxStaff <= 0)
            {
                validationErrors.Add("Max staff must be greater than 0");
            }

            if (updateDto.StorageLimitMb <= 0)
            {
                validationErrors.Add("Storage limit must be greater than 0");
            }

            if (validationErrors.Any())
            {
                return ApiResponse<SubscriptionPlanDto>.ValidationErrorResponse(
                    "Validation failed",
                    validationErrors);
            }

            // Get existing subscription plan
            var existingPlan = await _repository.GetSubscriptionByIdAsync(subscriptionId);
            if (existingPlan == null)
            {
                return ApiResponse<SubscriptionPlanDto>.NotFoundResponse(
                    $"Subscription plan with ID {subscriptionId} not found");
            }

            // Check if subscription is deleted
            if (existingPlan.IsDeleted)
            {
                return ApiResponse<SubscriptionPlanDto>.ValidationErrorResponse(
                    "Cannot update deleted subscription plan",
                    new List<string> { $"Subscription plan with ID {subscriptionId} is marked as deleted" });
            }

            // Check if name is being changed and if new name already exists
            if (existingPlan.Name != updateDto.Name)
            {
                var duplicatePlan = await _repository.GetSubscriptionByNameAsync(updateDto.Name, subscriptionId);
                if (duplicatePlan != null)
                {
                    return ApiResponse<SubscriptionPlanDto>.ValidationErrorResponse(
                        "Subscription plan name already exists",
                        new List<string> { $"A subscription plan with the name '{updateDto.Name}' already exists" });
                }
            }

            // Update the subscription plan
            existingPlan.Name = updateDto.Name;
            existingPlan.PriceMonthly = updateDto.PriceMonthly;
            existingPlan.PriceYearly = updateDto.PriceYearly;
            existingPlan.MaxTables = updateDto.MaxTables;
            existingPlan.MaxStaff = updateDto.MaxStaff;
            existingPlan.StorageLimitMb = updateDto.StorageLimitMb;
            existingPlan.HasNotifications = updateDto.HasNotifications;
            existingPlan.HasAnalytics = updateDto.HasAnalytics;
            existingPlan.FeaturesJson = updateDto.FeaturesJson;
            existingPlan.Description = updateDto.Description;
            existingPlan.IsActive = updateDto.IsActive;
            existingPlan.UpdatedAt = DateTime.UtcNow;

            // Save to database
            var result = await _repository.UpdateSubscriptionAsync(existingPlan);

            if (!result)
            {
                return ApiResponse<SubscriptionPlanDto>.ServerErrorResponse(
                    "Failed to update subscription plan. Please try again later.");
            }

            // Map to DTO
            var subscriptionDto = new SubscriptionPlanDto
            {
                Id = existingPlan.Id,
                Name = existingPlan.Name,
                PriceMonthly = existingPlan.PriceMonthly,
                PriceYearly = existingPlan.PriceYearly,
                MaxTables = existingPlan.MaxTables,
                MaxStaff = existingPlan.MaxStaff,
                StorageLimitMb = existingPlan.StorageLimitMb,
                HasNotifications = existingPlan.HasNotifications,
                HasAnalytics = existingPlan.HasAnalytics,
                FeaturesJson = existingPlan.FeaturesJson,
                Description = existingPlan.Description,
                IsActive = existingPlan.IsActive,
                CreatedAt = existingPlan.CreatedAt,
                UpdatedAt = existingPlan.UpdatedAt
            };

            return ApiResponse<SubscriptionPlanDto>.SuccessResponse(
                subscriptionDto,
                "Subscription plan updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<SubscriptionPlanDto>.ServerErrorResponse(
                "An error occurred while updating the subscription plan. Please try again later.");
        }
    }
}
