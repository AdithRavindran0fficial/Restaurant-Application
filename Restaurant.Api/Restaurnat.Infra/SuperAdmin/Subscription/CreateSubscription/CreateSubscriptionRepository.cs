using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.Subscription.CreateSubscription;

public class CreateSubscriptionRepository : ICreateSubscriptionRepository
{
    private readonly MasterDbContext _context;

    public CreateSubscriptionRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionPlan?> GetSubscriptionByNameAsync(string name)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower() && !s.IsDeleted);
    }

    public async Task<SubscriptionPlan> CreateSubscriptionAsync(SubscriptionPlan subscriptionPlan)
    {
        await _context.SubscriptionPlans.AddAsync(subscriptionPlan);
        await _context.SaveChangesAsync();
        return subscriptionPlan;
    }
}
