using Restaurant.Application.Common;
using Restaurant.Application.User.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.Order
{
    public interface ICreateOrderService
    {
        Task<ApiResponse<OrderItemResponseDTO>> CreateOrderAsync(CreateOrderDTO createOrderDTO);    
    }
}
