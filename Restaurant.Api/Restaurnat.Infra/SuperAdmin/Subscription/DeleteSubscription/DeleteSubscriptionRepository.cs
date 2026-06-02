using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeleteSubscription;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.Subscription.DeleteSubscription;

public class DeleteSubscriptionRepository : IDeleteSubscriptionRepository
{
    private readonly MasterDbContext _context;

    public DeleteSubscriptionRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task<bool> DeleteSubscriptionAsync(SubscriptionPlan subscriptionPlan)
    {
        try
        {
            subscriptionPlan.IsDeleted = true;
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
