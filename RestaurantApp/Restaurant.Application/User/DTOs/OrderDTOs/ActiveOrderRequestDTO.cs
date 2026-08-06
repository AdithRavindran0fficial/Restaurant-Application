using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs.OrderDTOs
{
    public class ActiveOrderRequestDTO
    {
        public string PhoneNumber { get; set; } 
        public string Otp { get; set; }
        public string SessionToken { get; set; }
    }
}
