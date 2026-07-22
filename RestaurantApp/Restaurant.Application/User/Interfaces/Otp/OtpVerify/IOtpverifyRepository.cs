using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.Otp.OtpVerify
{
    public interface IOtpverifyRepository
    {
        Task<TableSession> GetTableSession(string Session);
    }
}
