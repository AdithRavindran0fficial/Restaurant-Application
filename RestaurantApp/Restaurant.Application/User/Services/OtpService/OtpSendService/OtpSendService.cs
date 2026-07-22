using Restaurant.Application.Common;
using Restaurant.Application.Common.Interface;
using Restaurant.Application.User.DTOs;
using Restaurant.Application.User.Interfaces.Otp.OtpSend;
using Restaurant.Application.User.Interfaces.OtpService.OtpSendService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Services.OtpService.OtpSendService
{
    public class OtpSendService : IOtpSendService
    {
        private readonly IOtpSendRepository repository;
        private readonly ITwillioOtpService twillioOtpService;
        public OtpSendService(IOtpSendRepository otpSendRepository,ITwillioOtpService twillioOtp)
        {
            repository = otpSendRepository;
            twillioOtpService  = twillioOtp;    
            
        }


        public async Task<ApiResponse<object>> SendOtpAsync(OtpSendRequestDTO? requestDTO)
        {
            var ValidationErrors = new List<string>();
            if (requestDTO == null) return ApiResponse<object>.ValidationErrorResponse("request is Null");

            if (string.IsNullOrEmpty(requestDTO.PhoneNumber))
            {
                ValidationErrors.Add("Phone number is Empty");
            }
            if (string.IsNullOrEmpty(requestDTO.SessionToken))
            {
                ValidationErrors.Add("SessionToken is empty");
            }
            if (ValidationErrors.Any())
            {
                return ApiResponse<object>.ValidationErrorResponse("Validation failed", ValidationErrors);
            }

            var session = await repository.GetTableSession(requestDTO.SessionToken);
            if (session == null)
            {
                return ApiResponse<object>.NotFoundResponse("Session not found , please rescan");
            }

            var validTime = session.CreatedAt.AddHours(4);
            if (validTime < DateTime.UtcNow)
            {
                return ApiResponse<object>.FailureResponse("Session has expired , please rescan the qr");

            }

            //todo otp implementation 
            await twillioOtpService.SendOtpAsync(requestDTO.PhoneNumber,CancellationToken.None);

            return ApiResponse.SuccessResponse("OTP send successfully", 200);
            
        }
    }
}
