using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces;
using Restaurant.Application.Authentication.Interfaces;

namespace Restaurant.Api.Controllers.SuperAdmin
{
    [Route("api/v1/super/auth")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminAuthController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;
        private readonly IJwtService _jwtService;

        public SuperAdminAuthController(ISuperAdminService superAdminService, IJwtService jwtService)
        {
            _superAdminService = superAdminService;
            _jwtService = jwtService;
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(
            [FromBody] ChangePasswordRequest request)
        {
            // Extract token from Authorization header
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Token is required"));
            }

            // Extract SuperAdmin ID from JWT token using service
            var superAdminId = _jwtService.ExtractUserIdFromToken(token);

            if (!superAdminId.HasValue)
            {
                return BadRequest(ApiResponse<object>.ValidationErrorResponse(
                    "Invalid user token",
                    new List<string> { "Unable to identify user from token" }));
            }

            var result = await _superAdminService.ChangePasswordAsync(
                superAdminId.Value,
                request.CurrentPassword,
                request.NewPassword,
                request.ConfirmPassword);

            return StatusCode(result.StatusCode, result);
        }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
