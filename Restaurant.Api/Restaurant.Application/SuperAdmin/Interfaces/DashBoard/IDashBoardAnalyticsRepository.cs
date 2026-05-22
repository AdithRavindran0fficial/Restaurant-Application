using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Interfaces.DashBoard
{
    public interface IDashBoardAnalyticsRepository
    {
        Task<int> GetTotalTenantsAsync();
        Task<int> GetActiveTenantsAsync();
        Task<int> GetInactiveTenantsAsync();
        Task<int> GetTrialTenantsAsync();
        Task<int> GetNewTenantsThisMonthAsync();
        Task<decimal> GetTotalRevenueThisMonthAsync();
        Task<decimal> GetTotalRevenueAllTimeAsync();
        Task<int> GetActiveSubscriptionsCountAsync();
        Task<int> GetSubscriptionsExpiringSoonAsync(int days);
        Task<List<Tenant>> GetRecentSignupsAsync(int count);
        Task<List<TenantSubscription>> GetTrialAccountsExpiringSoonAsync(int days);
    }
}
