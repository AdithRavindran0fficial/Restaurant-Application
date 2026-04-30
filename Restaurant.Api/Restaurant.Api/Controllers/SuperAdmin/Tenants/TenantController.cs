using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;
using Restaurant.Application.SuperAdmin.Interfaces.SoftDeleteTenant;

namespace Restaurant.Api.Controllers.SuperAdmin.Tenants
{
    [Route("api/v1/super/tenants")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly ISoftDeleteTenantService _softDeleteTenantService;

        public TenantController(
            ITenantService tenantService, 
            ISoftDeleteTenantService softDeleteTenantService)
        {
            _tenantService = tenantService;
            _softDeleteTenantService = softDeleteTenantService;
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
    }
}
