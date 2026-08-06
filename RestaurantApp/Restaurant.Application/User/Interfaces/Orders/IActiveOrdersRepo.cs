using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.Orders
{
    public interface IActiveOrdersRepo
    {
        Task<TableSession?> GetTableSession(string SessionToken);
        Task<List<Restaurant.Domain.Entities.Order>> GetActiveOrder(int CustomerId,int tenantId);
        Task<Customer?> GetCustomer(string phone, int tenantId);
    }
}