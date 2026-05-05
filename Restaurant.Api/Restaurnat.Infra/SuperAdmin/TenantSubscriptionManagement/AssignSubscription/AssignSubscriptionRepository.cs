using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.AssignSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.SuperAdmin.TenantSubscriptionManagement.AssignSubscription
{
    public class AssignSubscriptionRepository : IAssignSubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignSubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
        {
            return await _context.Tenants
                .Where(t => t.Id == tenantId && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<SubscriptionPlan?> GetSubscriptionPlanByIdAsync(int planId)
        {
            return await _context.SubscriptionPlans
                .Where(sp => sp.Id == planId && !sp.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<TenantSubscription?> GetActiveSubscriptionByTenantIdAsync(int tenantId)
        {
            return await _context.TenantSubscriptions
                .Include(ts => ts.Plan)
                .Where(ts => ts.TenantId == tenantId && ts.IsActive && !ts.IsDeleted)
                .OrderByDescending(ts => ts.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<TenantSubscription> CreateTenantSubscriptionAsync(TenantSubscription tenantSubscription)
        {
            await _context.TenantSubscriptions.AddAsync(tenantSubscription);
            await _context.SaveChangesAsync();
            return tenantSubscription;
        }

        public async Task DeactivatePreviousSubscriptionsAsync(int tenantId)
        {
            var previousSubscriptions = await _context.TenantSubscriptions
                .Where(ts => ts.TenantId == tenantId && ts.IsActive && !ts.IsDeleted)
                .ToListAsync();

            foreach (var subscription in previousSubscriptions)
            {
                subscription.IsActive = false;
                subscription.Status = "cancelled";
                subscription.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
