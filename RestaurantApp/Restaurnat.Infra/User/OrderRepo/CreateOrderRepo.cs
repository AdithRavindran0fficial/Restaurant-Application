using Microsoft.EntityFrameworkCore;
using Restaurant.Application.User.Interfaces.Order;
using Restaurant.Application.User.Interfaces.Orders;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.User.OrderRepo
{
    public class CreateOrderRepo : ICreateOrderRepo
    {
        private readonly MasterDbContext masterDbContext;
        public CreateOrderRepo(MasterDbContext masterDbContext)
        {
            this.masterDbContext = masterDbContext; 
        }

        public async Task<Customer?> CheckCustomer(string Phone, int TenantId)
        {
            var customer = await masterDbContext.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == Phone && c.TenantId == TenantId);
            return customer;
        }

        public async Task CreateCustomer(Customer customer)
        {
            await masterDbContext.Customers.AddAsync(customer);
            await masterDbContext.SaveChangesAsync();
        }

        public async Task CreateOrderAsync(Order orders)
        {
            await masterDbContext.Orders.AddAsync(orders);
            await masterDbContext.SaveChangesAsync();
        }

        public async Task<TableSession?> GetTableSession(string Session)
        {
            return await masterDbContext.TableSessions.FirstOrDefaultAsync(s => s.SessionToken == Session && s.IsActive);
        }

        public async Task<List<MenuItem>> GetItems(List<int?> variantIds , int tenantId)
        {
            return await masterDbContext.MenuItems.Where(item => item.TenantId == tenantId && variantIds.Contains(item.Id)).ToListAsync();
        }

        
    }
}
