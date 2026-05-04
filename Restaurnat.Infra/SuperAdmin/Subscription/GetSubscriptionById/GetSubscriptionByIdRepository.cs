using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetSubscriptionById;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.Subscription.GetSubscriptionById;

public class GetSubscriptionByIdRepository : IGetSubscriptionByIdRepository
{
    private readonly MasterDbContext _context;

    public GetSubscriptionByIdRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionPlan?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(s => s.Id == subscriptionId && !s.IsDeleted);
    }
}
