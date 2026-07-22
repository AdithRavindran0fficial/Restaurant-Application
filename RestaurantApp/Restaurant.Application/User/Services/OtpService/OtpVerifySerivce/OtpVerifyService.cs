using Restaurant.Application.Common;
using Restaurant.Application.Common.Interface;
using Restaurant.Application.User.DTOs;
using Restaurant.Application.User.Interfaces.Otp.OtpVerify;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Services.OtpService.OtpVerifySerivce
{
    public class OtpVerifyService : IOtpVerifyService
    {

        private readonly ITwillioOtpService _service;   
        private readonly IOtpverifyRepository _otpverifyRepository; 
        public OtpVerifyService(ITwillioOtpService twillioOtpService, IOtpverifyRepository otpverifyRepository)
        {
            _service = twillioOtpService;   
            _otpverifyRepository = otpverifyRepository; 
        }

        public async Task<ApiResponse<object>> VerifyOtpAsync(OtpVerifyRequestDTO? requestDTO)
        {
            var list = new List<string>();  
            if (requestDTO == null)
            {
                list.Add("Request data is null");

            }
            if(requestDTO.Otp == null || requestDTO.PhoneNumber == null )
            {
                list.Add("Otp or PhoneNumber is null");

            }
            if(requestDTO.SessionToken==null)
            {
                list.Add("Session is null");
            }
            if (list.Any())
            {
                return ApiResponse<object>.ValidationErrorResponse("Validation failed",list);
            }

            var session = await _otpverifyRepository.GetTableSession(requestDTO.SessionToken);
            if (session == null)
            {
                return ApiResponse<object>.NotFoundResponse("Session not found , please rescan");
            }

            var validTime = session.CreatedAt.AddHours(4);
            if (validTime < DateTime.UtcNow)
            {
                return ApiResponse<object>.FailureResponse("Session has expired , please rescan the qr");

            }

            var result= await _service.VerifyOtpAsync(requestDTO.PhoneNumber, requestDTO.Otp,CancellationToken.None);
            if (result)
            {
                return ApiResponse<object>.SuccessResponse(null, "Otp verified successfully", 200);

            }
            else
            {
                return ApiResponse<object>.FailureResponse("Otp verification failed", 400);
            }
        }
    }
}
