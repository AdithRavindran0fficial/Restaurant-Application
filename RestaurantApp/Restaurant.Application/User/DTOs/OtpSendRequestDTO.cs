using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs
{
    public class OtpSendRequestDTO
    {
        public string? PhoneNumber { get; set; } 
        public string? SessionToken { get; set; }
    }
}
