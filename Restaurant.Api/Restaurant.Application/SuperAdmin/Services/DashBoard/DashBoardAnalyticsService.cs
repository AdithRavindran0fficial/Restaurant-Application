using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics;
using Restaurant.Application.SuperAdmin.Interfaces.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Services.DashBoard
{
    public class DashBoardAnalyticsService : IDashBoardAnalyticsService
    {
        private readonly IDashBoardAnalyticsRepository _repository;

        public DashBoardAnalyticsService(IDashBoardAnalyticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<SuperAdminDashBoard>> GetDashBoardAnalyticsAsync()
        {
            try
            {
                // Tenant Stats
                var tenantStats = new TenantStatsDTO
                {
                    TotalRestaurants = await _repository.GetTotalTenantsAsync(),
                    ActiveRestaurants = await _repository.GetActiveTenantsAsync(),
                    InactiveRestaurants = await _repository.GetInactiveTenantsAsync(),
                    TrialAccounts = await _repository.GetTrialTenantsAsync(),
                    NewRestaurantsThisMonth = await _repository.GetNewTenantsThisMonthAsync()
                };

                // Revenue Stats
                var revenueStats = new RevenueStatsDTO
                {
                    TotalRevenueThisMonth = await _repository.GetTotalRevenueThisMonthAsync(),
                    TotalRevenueAllTime = await _repository.GetTotalRevenueAllTimeAsync(),
                    ActiveSubscriptions = await _repository.GetActiveSubscriptionsCountAsync(),
                    ExpiringSoon = await _repository.GetSubscriptionsExpiringSoonAsync(7)
                };

                // Recent Signups (last 5)
                var recentTenants = await _repository.GetRecentSignupsAsync(5);
                var recentSignups = new RecentSignupDTO
                {
                    RecentSignups = recentTenants.Select(t => new RecentSignupItemDTO
                    {
                        TenantId = t.Id,
                        Name = t.Name,
                        Email = t.PrimaryEmail,
                        PlanName = t.Subscriptions
                            .OrderByDescending(s => s.CreatedAt)
                            .FirstOrDefault()?.Plan?.Name ?? "No Plan",
                        JoinedDate = t.CreatedAt
                    }).ToList()
                };

                // Trial Accounts Expiring Soon (next 7 days)
                var expiringTrials = await _repository.GetTrialAccountsExpiringSoonAsync(7);
                var trialExpiringSoon = new TrialAccountExpiresSoonDTO
                {
                    TrialAccountsExpiringSoon = expiringTrials.Select(ts => new TrialExpiringItemDTO
                    {
                        TenantId = ts.TenantId,
                        Name = ts.Tenant?.Name ?? "",
                        Email = ts.Tenant?.PrimaryEmail ?? "",
                        TrialEndsDate = ts.TrialEndsAt ?? DateTime.UtcNow,
                        DaysRemaining = ts.TrialEndsAt.HasValue
                            ? Math.Max(0, (int)(ts.TrialEndsAt.Value - DateTime.UtcNow).TotalDays)
                            : 0
                    }).ToList()
                };

                var dashboard = new SuperAdminDashBoard
                {
                    TenantStats = tenantStats,
                    RevenueStats = revenueStats,
                    RecentSignup = recentSignups,
                    TrialAccountExpiresSoon = trialExpiringSoon
                };

                return ApiResponse<SuperAdminDashBoard>.SuccessResponse(
                    dashboard,
                    "Dashboard analytics retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<SuperAdminDashBoard>.ServerErrorResponse(
                    $"Failed to retrieve dashboard analytics: {ex.Message}");
            }
        }
    }
}
