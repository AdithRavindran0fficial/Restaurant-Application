using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;
using Restaurant.Application.SuperAdmin.Interfaces.SoftDeleteTenant;
using Restaurant.Application.SuperAdmin.Interfaces.ActivateTenant;
using Restaurant.Application.SuperAdmin.Interfaces.DeactivateTenant;

namespace Restaurant.Api.Controllers.SuperAdmin.Tenants
{
    [Route("api/v1/super/tenants")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly ISoftDeleteTenantService _softDeleteTenantService;
        private readonly IActivateTenantService _activateTenantService;
        private readonly IDeactivateTenantService _deactivateTenantService;

        public TenantController(
            ITenantService tenantService, 
            ISoftDeleteTenantService softDeleteTenantService,
            IActivateTenantService activateTenantService,
            IDeactivateTenantService deactivateTenantService)
        {
            _tenantService = tenantService;
            _softDeleteTenantService = softDeleteTenantService;
            _activateTenantService = activateTenantService;
            _deactivateTenantService = deactivateTenantService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TenantDto>>>> GetAllTenants()
        {
            var result = await _tenantService.GetAllTenantsAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TenantDto>>> GetTenantById(int id)
        {
            var result = await _tenantService.GetTenantByIdAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDeleteTenant(int id)
        {
            var result = await _softDeleteTenantService.SoftDeleteTenantAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateTenant(int id)
        {
            var result = await _activateTenantService.ActivateTenantAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<ApiResponse<bool>>> DeactivateTenant(int id)
        {
            var result = await _deactivateTenantService.DeactivateTenantAsync(id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
