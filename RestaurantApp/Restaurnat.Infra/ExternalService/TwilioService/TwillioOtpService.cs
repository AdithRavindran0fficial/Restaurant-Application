using Microsoft.Extensions.Options;
using Restaurant.Application.Common.Interface;
using Restaurant.Application.Common.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace Restaurnat.Infra.ExternalService.TwilioService
{
    public class TwillioOtpService : ITwillioOtpService
    {
        private readonly TwilioOption twilioOption;
        public TwillioOtpService(IOptions<TwilioOption> options)
        {
            twilioOption = options.Value;
            TwilioClient.Init(twilioOption.AccountSid, twilioOption.AuthToken);
            
        }
        public async Task SendOtpAsync(string phoneNumber, CancellationToken token)
        {
            var result = await VerificationResource.CreateAsync(
                to: phoneNumber,
                channel: "sms",
                pathServiceSid: twilioOption.VerifyServiceSid
                );
            if (result.Status != "pending")
            {
                throw new InvalidOperationException("Failed to send Otp");
            }
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp, CancellationToken token =default)
        {
            var result = await VerificationCheckResource.CreateAsync(
                to: phoneNumber,
                code: otp,
                pathServiceSid: twilioOption.VerifyServiceSid);
            return result.Status == "approved";
        }
    }
}
