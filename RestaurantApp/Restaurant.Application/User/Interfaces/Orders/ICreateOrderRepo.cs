using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Restaurant.Application.User.Interfaces.Orders
{
    public interface ICreateOrderRepo
    {
        Task CreateOrderAsync(Restaurant.Domain.Entities.Order orders);
        Task<TableSession?> GetTableSession(string Session);
        Task<Customer?> CheckCustomer(string Phone, int TenantId);

        Task CreateCustomer(Customer customer);

        Task<List<MenuItem>> GetItems(List<int?> variantIds ,int tenantId);
    }

}
