using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Authentication.DTOs;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Application.Common;

namespace Restaurant.Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(
            [FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            return StatusCode(result.StatusCode, result);
        }
    }
}
