using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs.OrderDTOs
{
    public class OrderItemResponseDTO
    {

        public string OrderId { get; set; } 
        public string OrderNumber { get; set; }   
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; }
    }
}
