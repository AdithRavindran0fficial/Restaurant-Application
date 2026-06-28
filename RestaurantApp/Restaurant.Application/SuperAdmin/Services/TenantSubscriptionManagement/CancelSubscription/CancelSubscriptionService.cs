using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.CancelSubscription;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Services.TenantSubscriptionManagement.CancelSubscription
{
    public class CancelSubscriptionService : ICancelSubscriptionService
    {
        private readonly ICancelSubscriptionRepository _repository;

        public CancelSubscriptionService(ICancelSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<TenantSubscriptionDto>> CancelSubscriptionAsync(int tenantId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<TenantSubscriptionDto>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            var tenant = await _repository.GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return ApiResponse<TenantSubscriptionDto>.NotFoundResponse(
                    "Tenant not found");
            }

            var activeSubscription = await _repository.GetActiveSubscriptionByTenantIdAsync(tenantId);
            if (activeSubscription == null)
            {
                return ApiResponse<TenantSubscriptionDto>.NotFoundResponse(
                    "No active subscription found for this tenant");
            }

            if (activeSubscription.Status == "cancelled")
            {
                return ApiResponse<TenantSubscriptionDto>.FailureResponse(
                    "Subscription is already cancelled", 400);
            }

            var success = await _repository.CancelSubscriptionAsync(activeSubscription);
            if (!success)
            {
                return ApiResponse<TenantSubscriptionDto>.ServerErrorResponse(
                    "Failed to cancel subscription");
            }

            // Reload to get updated subscription with Plan details
            var updatedSubscription = await _repository.GetActiveSubscriptionByTenantIdAsync(tenantId);

            // If no active subscription found after cancellation, use the cancelled one
            if (updatedSubscription == null)
            {
                updatedSubscription = activeSubscription;
            }

            var responseDto = new TenantSubscriptionDto
            {
                Id = updatedSubscription.Id,
                TenantId = updatedSubscription.TenantId,
                PlanId = updatedSubscription.PlanId,
                PlanName = updatedSubscription.Plan?.Name ?? "",
                BillingCycle = updatedSubscription.BillingCycle,
                Price = updatedSubscription.Price,
                StartDate = updatedSubscription.StartDate,
                EndDate = updatedSubscription.EndDate,
                IsTrial = updatedSubscription.IsTrial,
                TrialEndsAt = updatedSubscription.TrialEndsAt,
                Status = updatedSubscription.Status,
                IsActive = updatedSubscription.IsActive,
                CreatedAt = updatedSubscription.CreatedAt,
                UpdatedAt = updatedSubscription.UpdatedAt,
                Plan = updatedSubscription.Plan != null ? new SubscriptionPlanDto
                {
                    Id = updatedSubscription.Plan.Id,
                    Name = updatedSubscription.Plan.Name,
                    PriceMonthly = updatedSubscription.Plan.PriceMonthly,
                    PriceYearly = updatedSubscription.Plan.PriceYearly,
                    MaxTables = updatedSubscription.Plan.MaxTables,
                    MaxStaff = updatedSubscription.Plan.MaxStaff,
                    StorageLimitMb = updatedSubscription.Plan.StorageLimitMb,
                    HasNotifications = updatedSubscription.Plan.HasNotifications,
                    HasAnalytics = updatedSubscription.Plan.HasAnalytics,
                    FeaturesJson = updatedSubscription.Plan.FeaturesJson,
                    Description = updatedSubscription.Plan.Description,
                    IsActive = updatedSubscription.Plan.IsActive,
                    CreatedAt = updatedSubscription.Plan.CreatedAt,
                    UpdatedAt = updatedSubscription.Plan.UpdatedAt
                } : null
            };

            return ApiResponse<TenantSubscriptionDto>.SuccessResponse(
                responseDto,
                "Subscription cancelled successfully",
                200);
        }
    }
}
