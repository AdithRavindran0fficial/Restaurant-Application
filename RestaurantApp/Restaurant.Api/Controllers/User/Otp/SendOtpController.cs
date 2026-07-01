using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.User.DTOs;
using Restaurant.Application.User.Interfaces.OtpService.OtpSendService;

namespace Restaurant.Api.Controllers.User.Otp
{
    [Route("api/v1/user/otp")]
    [ApiController]
    public class SendOtpController : ControllerBase
    {
        private readonly IOtpSendService otpSendService;
        public SendOtpController(IOtpSendService otpSend)
        {
            
            otpSendService = otpSend;   

        }
        [HttpPost]
        public async Task<IActionResult>SendOtpAsync(OtpSendRequestDTO otpSendRequestDTO)
        {
            var result = await otpSendService.SendOtpAsync(otpSendRequestDTO);
            return StatusCode(result.StatusCode, result);
        }
    }
}
