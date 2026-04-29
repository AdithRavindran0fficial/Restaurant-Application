using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Application.Common;

namespace Restaurant.Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("tenant")]
        public async Task<ActionResult<ApiResponse<object>>> RegisterTenant(
            [FromBody] RegisterTenantRequest request)
        {
            var result = await _registrationService.RegisterTenantAsync(
                request.TenantName,
                request.Slug,
                request.PrimaryEmail,
                request.Password,
                request.PrimaryPhone,
                request.CountryId);

            return StatusCode(result.StatusCode, result);
        }
    }

    public class RegisterTenantRequest
    {
        public string TenantName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string PrimaryEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PrimaryPhone { get; set; }
        public int? CountryId { get; set; }
    }
}
