using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs.OrderDTOs
{
    public class CreateOrderItemDTO
    {
        public string? Note { get; set; }
        public int? OrderID { get; set; }
        public int? Quantity { get; set; } = 1;
    }
}
