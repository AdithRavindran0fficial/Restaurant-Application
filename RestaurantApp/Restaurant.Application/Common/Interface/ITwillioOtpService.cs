using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Common.Interface
{
    public interface ITwillioOtpService
    {
        Task SendOtpAsync(string phoneNumber,CancellationToken token);

        Task<bool> VerifyOtpAsync(string phoneNumber, string otp,CancellationToken token);
    }
}
