using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Services.Subscription.CreateSubscription;

public class CreateSubscriptionService : ICreateSubscriptionService
{
    private readonly ICreateSubscriptionRepository _repository;

    public CreateSubscriptionService(ICreateSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<SubscriptionPlanDto>> CreateSubscriptionAsync(CreateSubscriptionDto createDto)
    {
        try
        {
            // Validation
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(createDto.Name))
            {
                validationErrors.Add("Subscription plan name is required");
            }

            if (createDto.PriceMonthly < 0)
            {
                validationErrors.Add("Monthly price cannot be negative");
            }

            if (createDto.PriceYearly.HasValue && createDto.PriceYearly.Value < 0)
            {
                validationErrors.Add("Yearly price cannot be negative");
            }

            if (createDto.MaxTables <= 0)
            {
                validationErrors.Add("Max tables must be greater than 0");
            }

            if (createDto.MaxStaff <= 0)
            {
                validationErrors.Add("Max staff must be greater than 0");
            }

            if (createDto.StorageLimitMb <= 0)
            {
                validationErrors.Add("Storage limit must be greater than 0");
            }

            if (validationErrors.Any())
            {
                return ApiResponse<SubscriptionPlanDto>.ValidationErrorResponse(
                    "Validation failed",
                    validationErrors);
            }

            // Check if subscription plan with same name already exists
            var existingPlan = await _repository.GetSubscriptionByNameAsync(createDto.Name);
            if (existingPlan != null)
            {
                return ApiResponse<SubscriptionPlanDto>.ValidationErrorResponse(
                    "Subscription plan already exists",
                    new List<string> { $"A subscription plan with the name '{createDto.Name}' already exists" });
            }

            // Create new subscription plan entity
            var subscriptionPlan = new SubscriptionPlan
            {
                Name = createDto.Name,
                PriceMonthly = createDto.PriceMonthly,
                PriceYearly = createDto.PriceYearly,
                MaxTables = createDto.MaxTables,
                MaxStaff = createDto.MaxStaff,
                StorageLimitMb = createDto.StorageLimitMb,
                HasNotifications = createDto.HasNotifications,
                HasAnalytics = createDto.HasAnalytics,
                FeaturesJson = createDto.FeaturesJson,
                Description = createDto.Description,
                IsActive = createDto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save to database
            var createdPlan = await _repository.CreateSubscriptionAsync(subscriptionPlan);

            // Map to DTO
            var subscriptionDto = new SubscriptionPlanDto
            {
                Id = createdPlan.Id,
                Name = createdPlan.Name,
                PriceMonthly = createdPlan.PriceMonthly,
                PriceYearly = createdPlan.PriceYearly,
                MaxTables = createdPlan.MaxTables,
                MaxStaff = createdPlan.MaxStaff,
                StorageLimitMb = createdPlan.StorageLimitMb,
                HasNotifications = createdPlan.HasNotifications,
                HasAnalytics = createdPlan.HasAnalytics,
                FeaturesJson = createdPlan.FeaturesJson,
                Description = createdPlan.Description,
                IsActive = createdPlan.IsActive,
                CreatedAt = createdPlan.CreatedAt,
                UpdatedAt = createdPlan.UpdatedAt
            };

            return ApiResponse<SubscriptionPlanDto>.SuccessResponse(
                subscriptionDto,
                "Subscription plan created successfully",
                201);
        }
        catch (Exception ex)
        {
            return ApiResponse<SubscriptionPlanDto>.ServerErrorResponse(
                "An error occurred while creating the subscription plan. Please try again later.");
        }
    }
}
