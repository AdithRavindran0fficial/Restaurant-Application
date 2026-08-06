using Restaurant.Application.Common;
using Restaurant.Application.User.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.Orders
{
    public interface IActiveOrdersService
    {
        Task<ApiResponse<ActiveOrdersResponseDTO>> GetActiveOrders(ActiveOrderRequestDTO activeOrderRequestDTO);
    }
}
