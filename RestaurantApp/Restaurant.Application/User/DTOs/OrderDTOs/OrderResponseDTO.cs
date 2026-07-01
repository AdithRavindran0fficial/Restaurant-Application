using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs.OrderDTOs
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int EstimatedMinutes { get; set; }
        public List<OrderItemResponseDTO> Items { get; set; }
            = new List<OrderItemResponseDTO>();
        public DateTime CreatedAt { get; set; }
    }
}
