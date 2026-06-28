using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.GetTenantSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.AssignSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.CancelSubscription;

namespace Restaurant.Api.Controllers.SuperAdmin.TenantSubscriptions
{
    [Route("api/v1/super/tenants")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class TenantSubscriptionController : ControllerBase
    {
        private readonly IGetTenantSubscriptionService _getTenantSubscriptionService;
        private readonly IAssignSubscriptionService _assignSubscriptionService;
        private readonly ICancelSubscriptionService _cancelSubscriptionService;

        public TenantSubscriptionController(
            IGetTenantSubscriptionService getTenantSubscriptionService,
            IAssignSubscriptionService assignSubscriptionService,
            ICancelSubscriptionService cancelSubscriptionService)
        {
            _getTenantSubscriptionService = getTenantSubscriptionService;
            _assignSubscriptionService = assignSubscriptionService;
            _cancelSubscriptionService = cancelSubscriptionService;
        }

        [HttpGet("{id}/subscription")]
        public async Task<ActionResult<ApiResponse<TenantSubscriptionDto>>> GetTenantSubscription(int id)
        {
            var result = await _getTenantSubscriptionService.GetTenantSubscriptionAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/subscription")]
        public async Task<ActionResult<ApiResponse<TenantSubscriptionDto>>> AssignSubscription(int id, [FromBody] AssignSubscriptionDto dto)
        {
            var result = await _assignSubscriptionService.AssignSubscriptionAsync(id, dto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}/subscription")]
        public async Task<ActionResult<ApiResponse<TenantSubscriptionDto>>> CancelSubscription(int id)
        {
            var result = await _cancelSubscriptionService.CancelSubscriptionAsync(id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
