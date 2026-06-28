using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.ActivateSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.Subscription.ActivateSubscription;

public class ActivateSubscriptionRepository : IActivateSubscriptionRepository
{
    private readonly MasterDbContext _context;

    public ActivateSubscriptionRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task<bool> ActivateSubscriptionAsync(SubscriptionPlan subscriptionPlan)
    {
        try
        {
            subscriptionPlan.IsActive = true;
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
