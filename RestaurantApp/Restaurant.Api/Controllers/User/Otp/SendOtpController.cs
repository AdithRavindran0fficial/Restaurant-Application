using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.User.DTOs;
using Restaurant.Application.User.Interfaces.Otp.OtpVerify;
using Restaurant.Application.User.Interfaces.OtpService.OtpSendService;
using Twilio.Rest.Trunking.V1;

namespace Restaurant.Api.Controllers.User.Otp
{
    [Route("api/v1/user/otp")]
    [ApiController]
    public class SendOtpController : ControllerBase
    {
        private readonly IOtpSendService otpSendService;
        private readonly IOtpVerifyService otpVerifyService;
        public SendOtpController(IOtpSendService otpSend,IOtpVerifyService otpVerify)
        {
            
            otpSendService = otpSend;
            otpVerifyService = otpVerify;
        }
        [HttpPost("send")]
        public async Task<IActionResult>SendOtpAsync(OtpSendRequestDTO otpSendRequestDTO)
        {
            var result = await otpSendService.SendOtpAsync(otpSendRequestDTO);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("verify")]
        public async Task <IActionResult>VerifyOtpAsync(OtpVerifyRequestDTO requestDTO)
        {
            var result = await otpVerifyService.VerifyOtpAsync(requestDTO);
            return StatusCode(result.StatusCode, result);
        }
    }
}
