using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs
{
    public class OtpVerifyRequestDTO
    {
        public string? Otp { get; set; }
        public string? PhoneNumber { get; set; }
        public string ? SessionToken { get; set; }
        
    }
}
