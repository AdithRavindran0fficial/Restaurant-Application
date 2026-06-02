using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin.Subscription.GetAllSubscriptions;

public class GetAllSubscriptionsRepository : IGetAllSubscriptionsRepository
{
    private readonly MasterDbContext _context;

    public GetAllSubscriptionsRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetAllSubscriptionsAsync()
    {
        return await _context.SubscriptionPlans
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }
}
