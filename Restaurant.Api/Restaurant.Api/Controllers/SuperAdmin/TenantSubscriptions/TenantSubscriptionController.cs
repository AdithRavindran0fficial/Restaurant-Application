using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.GetTenantSubscription;

namespace Restaurant.Api.Controllers.SuperAdmin.TenantSubscriptions
{
    [Route("api/v1/super/tenants")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class TenantSubscriptionController : ControllerBase
    {
        private readonly IGetTenantSubscriptionService _getTenantSubscriptionService;

        public TenantSubscriptionController(IGetTenantSubscriptionService getTenantSubscriptionService)
        {
            _getTenantSubscriptionService = getTenantSubscriptionService;
        }

        [HttpGet("{id}/subscription")]
        public async Task<ActionResult<ApiResponse<TenantSubscriptionDto>>> GetTenantSubscription(int id)
        {
            var result = await _getTenantSubscriptionService.GetTenantSubscriptionAsync(id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
