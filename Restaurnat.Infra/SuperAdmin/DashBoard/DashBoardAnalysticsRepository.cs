using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.DashBoard;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.SuperAdmin.DashBoard
{
    public class DashBoardAnalysticsRepository : IDashBoardAnalyticsRepository
    {
        private readonly MasterDbContext _context;

        public DashBoardAnalysticsRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalTenantsAsync()
        {
            return await _context.Tenants
                .Where(t => !t.IsDeleted)
                .CountAsync();
        }

        public async Task<int> GetActiveTenantsAsync()
        {
            return await _context.Tenants
                .Where(t => !t.IsDeleted && t.IsActive)
                .CountAsync();
        }

        public async Task<int> GetInactiveTenantsAsync()
        {
            return await _context.Tenants
                .Where(t => !t.IsDeleted && !t.IsActive)
                .CountAsync();
        }

        public async Task<int> GetTrialTenantsAsync()
        {
            return await _context.TenantSubscriptions
                .Where(ts => !ts.IsDeleted && ts.IsActive && ts.IsTrial)
                .CountAsync();
        }

        public async Task<int> GetNewTenantsThisMonthAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _context.Tenants
                .Where(t => !t.IsDeleted && t.CreatedAt >= firstDayOfMonth)
                .CountAsync();
        }

        public async Task<decimal> GetTotalRevenueThisMonthAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _context.TenantSubscriptions
                .Where(ts => !ts.IsDeleted && !ts.IsTrial && ts.CreatedAt >= firstDayOfMonth)
                .SumAsync(ts => ts.Price);
        }

        public async Task<decimal> GetTotalRevenueAllTimeAsync()
        {
            return await _context.TenantSubscriptions
                .Where(ts => !ts.IsDeleted && !ts.IsTrial)
                .SumAsync(ts => ts.Price);
        }

        public async Task<int> GetActiveSubscriptionsCountAsync()
        {
            return await _context.TenantSubscriptions
                .Where(ts => !ts.IsDeleted && ts.IsActive && ts.Status == "active")
                .CountAsync();
        }

        public async Task<int> GetSubscriptionsExpiringSoonAsync(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(days);
            return await _context.TenantSubscriptions
                .Where(ts => !ts.IsDeleted && ts.IsActive
                    && ts.EndDate.HasValue
                    && ts.EndDate.Value <= cutoffDate
                    && ts.EndDate.Value >= DateTime.UtcNow)
                .CountAsync();
        }

        public async Task<List<Tenant>> GetRecentSignupsAsync(int count)
        {
            return await _context.Tenants
                .Include(t => t.Subscriptions)
                    .ThenInclude(s => s.Plan)
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<TenantSubscription>> GetTrialAccountsExpiringSoonAsync(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(days);
            return await _context.TenantSubscriptions
                .Include(ts => ts.Tenant)
                .Where(ts => !ts.IsDeleted && ts.IsActive && ts.IsTrial
                    && ts.TrialEndsAt.HasValue
                    && ts.TrialEndsAt.Value <= cutoffDate
                    && ts.TrialEndsAt.Value >= DateTime.UtcNow)
                .OrderBy(ts => ts.TrialEndsAt)
                .ToListAsync();
        }
    }
}

