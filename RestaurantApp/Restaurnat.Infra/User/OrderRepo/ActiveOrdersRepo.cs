using Microsoft.EntityFrameworkCore;
using Restaurant.Application.User.Interfaces.Orders;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.User.OrderRepo
{
    public class ActiveOrdersRepo : IActiveOrdersRepo
    {
        private readonly MasterDbContext _masterDbContext;  
        public ActiveOrdersRepo(MasterDbContext masterDbContext)
        {
            this ._masterDbContext = masterDbContext;   
        }
        public async Task<List<Order>> GetActiveOrder(int CustomerId, int tenantId)
        {
            var orders = await _masterDbContext.Orders.Include(o => o.OrderItems).ThenInclude(c => c.MenuItem).Where(o => o.CustomerId == CustomerId && tenantId == o.TenantId && o.CreatedAt>DateTime.Now.AddHours(-4)).ToListAsync();
            return orders;
        }

        public async Task<Customer?> GetCustomer(string phone, int tenantId)
        {
            var customer = await _masterDbContext.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phone && c.TenantId==tenantId);
            return customer;
        }

        public async Task<TableSession?> GetTableSession(string SessionToken)
        {
            var session = await _masterDbContext.TableSessions.FirstOrDefaultAsync(s => s.SessionToken == SessionToken);
            return session;
        }
    }
}
