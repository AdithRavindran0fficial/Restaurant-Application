using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeactivateSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.Subscription.DeactivateSubscription;

public class DeactivateSubscriptionRepository : IDeactivateSubscriptionRepository
{
    private readonly MasterDbContext _context;

    public DeactivateSubscriptionRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task<bool> DeactivateSubscriptionAsync(SubscriptionPlan subscriptionPlan)
    {
        try
        {
            subscriptionPlan.IsActive = false;
            subscriptionPlan.UpdatedAt = DateTime.UtcNow;

            _context.SubscriptionPlans.Update(subscriptionPlan);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch
        {
            return false;
        }
    }
}
