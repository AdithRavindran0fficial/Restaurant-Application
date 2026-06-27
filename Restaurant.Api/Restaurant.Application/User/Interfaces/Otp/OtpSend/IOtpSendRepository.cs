using Restaurant.Application.Common;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace Restaurant.Application.User.Interfaces.Otp.OtpSend
{
    public interface IOtpSendRepository
    {

        Task<TableSession?> GetTableSession(string Session);
        Task AddOtpVerification(OtpVerification otpVerification);
        Task CloseTableSession(TableSession session);
        

    }
}
