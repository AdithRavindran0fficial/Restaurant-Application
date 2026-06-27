using Restaurant.Application.Common;
using Restaurant.Application.User.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.OtpService.OtpSendService
{
    public interface IOtpSendService
    {
        Task<ApiResponse<object>> SendOtpAsync(OtpSendRequestDTO? requestDTO);
    }
}
