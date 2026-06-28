using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.AssignSubscription;
using Restaurant.Application.Common;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Services.TenantSubscriptionManagement.AssignSubscription
{
    public class AssignSubscriptionService : IAssignSubscriptionService
    {
        private readonly IAssignSubscriptionRepository _repository;

        public AssignSubscriptionService(IAssignSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<TenantSubscriptionDto>> AssignSubscriptionAsync(int tenantId, AssignSubscriptionDto dto)
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

            var plan = await _repository.GetSubscriptionPlanByIdAsync(dto.PlanId);
            if (plan == null)
            {
                return ApiResponse<TenantSubscriptionDto>.NotFoundResponse(
                    "Subscription plan not found");
            }

            if (!plan.IsActive || plan.IsDeleted)
            {
                return ApiResponse<TenantSubscriptionDto>.FailureResponse(
                    "Subscription plan is not active or has been deleted", 400);
            }

            if (dto.IsTrial && (!dto.TrialDays.HasValue || dto.TrialDays.Value <= 0))
            {
                return ApiResponse<TenantSubscriptionDto>.ValidationErrorResponse(
                    "Trial days must be specified for trial subscriptions",
                    new List<string> { "Trial days must be greater than 0 for trial subscriptions" });
            }

            // Deactivate any previous active subscriptions
            await _repository.DeactivatePreviousSubscriptionsAsync(tenantId);

            // Calculate price based on billing cycle
            decimal price = dto.BillingCycle.ToLower() == "yearly" && plan.PriceYearly.HasValue
                ? plan.PriceYearly.Value
                : plan.PriceMonthly;

            // Calculate dates
            DateTime startDate = dto.StartDate ?? DateTime.UtcNow;
            DateTime? endDate = null;
            DateTime? trialEndsAt = null;

            if (dto.IsTrial)
            {
                trialEndsAt = startDate.AddDays(dto.TrialDays.Value);
            }
            else
            {
                endDate = dto.BillingCycle.ToLower() == "yearly"
                    ? startDate.AddYears(1)
                    : startDate.AddMonths(1);
            }

            var tenantSubscription = new TenantSubscription
            {
                TenantId = tenantId,
                PlanId = dto.PlanId,
                BillingCycle = dto.BillingCycle.ToLower(),
                Price = price,
                StartDate = startDate,
                EndDate = endDate,
                IsTrial = dto.IsTrial,
                TrialEndsAt = trialEndsAt,
                Status = "active",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdSubscription = await _repository.CreateTenantSubscriptionAsync(tenantSubscription);

            // Reload with Plan navigation
            var subscriptionWithPlan = await _repository.GetActiveSubscriptionByTenantIdAsync(tenantId);

            var responseDto = new TenantSubscriptionDto
            {
                Id = subscriptionWithPlan.Id,
                TenantId = subscriptionWithPlan.TenantId,
                PlanId = subscriptionWithPlan.PlanId,
                PlanName = subscriptionWithPlan.Plan?.Name ?? "",
                BillingCycle = subscriptionWithPlan.BillingCycle,
                Price = subscriptionWithPlan.Price,
                StartDate = subscriptionWithPlan.StartDate,
                EndDate = subscriptionWithPlan.EndDate,
                IsTrial = subscriptionWithPlan.IsTrial,
                TrialEndsAt = subscriptionWithPlan.TrialEndsAt,
                Status = subscriptionWithPlan.Status,
                IsActive = subscriptionWithPlan.IsActive,
                CreatedAt = subscriptionWithPlan.CreatedAt,
                UpdatedAt = subscriptionWithPlan.UpdatedAt,
                Plan = subscriptionWithPlan.Plan != null ? new SubscriptionPlanDto
                {
                    Id = subscriptionWithPlan.Plan.Id,
                    Name = subscriptionWithPlan.Plan.Name,
                    PriceMonthly = subscriptionWithPlan.Plan.PriceMonthly,
                    PriceYearly = subscriptionWithPlan.Plan.PriceYearly,
                    MaxTables = subscriptionWithPlan.Plan.MaxTables,
                    MaxStaff = subscriptionWithPlan.Plan.MaxStaff,
                    StorageLimitMb = subscriptionWithPlan.Plan.StorageLimitMb,
                    HasNotifications = subscriptionWithPlan.Plan.HasNotifications,
                    HasAnalytics = subscriptionWithPlan.Plan.HasAnalytics,
                    FeaturesJson = subscriptionWithPlan.Plan.FeaturesJson,
                    Description = subscriptionWithPlan.Plan.Description,
                    IsActive = subscriptionWithPlan.Plan.IsActive,
                    CreatedAt = subscriptionWithPlan.Plan.CreatedAt,
                    UpdatedAt = subscriptionWithPlan.Plan.UpdatedAt
                } : null
            };

            return ApiResponse<TenantSubscriptionDto>.CreatedResponse(
                responseDto,
                "Subscription plan assigned successfully",
                201);
        }
    }
}
