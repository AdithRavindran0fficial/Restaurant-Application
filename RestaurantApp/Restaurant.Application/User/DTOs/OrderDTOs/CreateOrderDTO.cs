using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public string SessionToken { get; set; }
        public string ? Notes { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; }
        public string Phone { get; set;  }
        public string Otp { get; set; }

        public string Name { get; set; }
    }
}
