using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.GetTenantSubscription;

namespace Restaurant.Application.SuperAdmin.Services.TenantSubscriptionManagement.GetTenantSubscription;

public class GetTenantSubscriptionService : IGetTenantSubscriptionService
{
    private readonly IGetTenantSubscriptionRepository _repository;

    public GetTenantSubscriptionService(IGetTenantSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<TenantSubscriptionDto>> GetTenantSubscriptionAsync(int tenantId)
    {
        try
        {
            // Validation
            if (tenantId <= 0)
            {
                return ApiResponse<TenantSubscriptionDto>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            // Check if tenant exists
            var tenant = await _repository.GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return ApiResponse<TenantSubscriptionDto>.NotFoundResponse(
                    $"Tenant with ID {tenantId} not found");
            }

            // Get tenant subscription
            var tenantSubscription = await _repository.GetTenantSubscriptionByTenantIdAsync(tenantId);

            if (tenantSubscription == null)
            {
                return ApiResponse<TenantSubscriptionDto>.NotFoundResponse(
                    $"No subscription found for tenant with ID {tenantId}");
            }

            // Map to DTO
            var subscriptionDto = new TenantSubscriptionDto
            {
                Id = tenantSubscription.Id,
                TenantId = tenantSubscription.TenantId,
                PlanId = tenantSubscription.PlanId,
                PlanName = tenantSubscription.Plan?.Name ?? string.Empty,
                BillingCycle = tenantSubscription.BillingCycle,
                Price = tenantSubscription.Price,
                StartDate = tenantSubscription.StartDate,
                EndDate = tenantSubscription.EndDate,
                IsTrial = tenantSubscription.IsTrial,
                TrialEndsAt = tenantSubscription.TrialEndsAt,
                Status = tenantSubscription.Status,
                IsActive = tenantSubscription.IsActive,
                CreatedAt = tenantSubscription.CreatedAt,
                UpdatedAt = tenantSubscription.UpdatedAt
            };

            // Include plan details if available
            if (tenantSubscription.Plan != null)
            {
                subscriptionDto.Plan = new SubscriptionPlanDto
                {
                    Id = tenantSubscription.Plan.Id,
                    Name = tenantSubscription.Plan.Name,
                    PriceMonthly = tenantSubscription.Plan.PriceMonthly,
                    PriceYearly = tenantSubscription.Plan.PriceYearly,
                    MaxTables = tenantSubscription.Plan.MaxTables,
                    MaxStaff = tenantSubscription.Plan.MaxStaff,
                    StorageLimitMb = tenantSubscription.Plan.StorageLimitMb,
                    HasNotifications = tenantSubscription.Plan.HasNotifications,
                    HasAnalytics = tenantSubscription.Plan.HasAnalytics,
                    FeaturesJson = tenantSubscription.Plan.FeaturesJson,
                    Description = tenantSubscription.Plan.Description,
                    IsActive = tenantSubscription.Plan.IsActive,
                    CreatedAt = tenantSubscription.Plan.CreatedAt,
                    UpdatedAt = tenantSubscription.Plan.UpdatedAt
                };
            }

            return ApiResponse<TenantSubscriptionDto>.SuccessResponse(
                subscriptionDto,
                "Tenant subscription retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantSubscriptionDto>.ServerErrorResponse(
                "An error occurred while retrieving the tenant subscription. Please try again later.");
        }
    }
}
