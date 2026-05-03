using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.Subscription.UpdateSubscription;

public class UpdateSubscriptionRepository : IUpdateSubscriptionRepository
{
    private readonly MasterDbContext _context;

    public UpdateSubscriptionRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task<SubscriptionPlan?> GetSubscriptionByNameAsync(string name, int excludeId)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower() 
                                   && s.Id != excludeId 
                                   && !s.IsDeleted);
    }

    public async Task<bool> UpdateSubscriptionAsync(SubscriptionPlan subscriptionPlan)
    {
        try
        {
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
