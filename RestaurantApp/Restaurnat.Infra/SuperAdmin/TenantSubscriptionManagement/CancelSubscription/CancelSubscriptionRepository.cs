using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.CancelSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.SuperAdmin.TenantSubscriptionManagement.CancelSubscription
{
    public class CancelSubscriptionRepository : ICancelSubscriptionRepository
    {
        private readonly MasterDbContext _context;

        public CancelSubscriptionRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
        {
            return await _context.Tenants
                .Where(t => t.Id == tenantId && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<TenantSubscription?> GetActiveSubscriptionByTenantIdAsync(int tenantId)
        {
            return await _context.TenantSubscriptions
                .Include(ts => ts.Plan)
                .Where(ts => ts.TenantId == tenantId && !ts.IsDeleted)
                .OrderByDescending(ts => ts.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CancelSubscriptionAsync(TenantSubscription subscription)
        {
            try
            {
                subscription.Status = "cancelled";
                subscription.IsActive = false;
                subscription.EndDate = DateTime.UtcNow;
                subscription.UpdatedAt = DateTime.UtcNow;

                _context.TenantSubscriptions.Update(subscription);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
