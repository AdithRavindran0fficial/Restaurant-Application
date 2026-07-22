using Restaurant.Application.Common;
using Restaurant.Application.User.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.Otp.OtpVerify
{
    public interface IOtpVerifyService
    {
        Task<ApiResponse<object>> VerifyOtpAsync(OtpVerifyRequestDTO? requestDTO);  
    }
}
