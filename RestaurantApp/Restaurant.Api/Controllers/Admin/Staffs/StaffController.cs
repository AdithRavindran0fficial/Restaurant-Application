using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Staff.GetAllStaff;
using Restaurant.Application.Admin.Interfaces.Staff.GetStaffById;
using Restaurant.Application.Admin.Interfaces.Staff.UpdateStaff;
using Restaurant.Application.Admin.Interfaces.Staff.SoftDeleteStaff;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers.Admin.Staffs
{
    [Route("api/v1/admin/staffs")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StaffController : ControllerBase
    {
        private readonly IGetAllStaffService _getAllStaffService;
        private readonly IGetStaffByIdService _getStaffByIdService;
        private readonly IUpdateStaffService _updateStaffService;
        private readonly ISoftDeleteStaffService _softDeleteStaffService;

        public StaffController(
            IGetAllStaffService getAllStaffService,
            IGetStaffByIdService getStaffByIdService,
            IUpdateStaffService updateStaffService,
            ISoftDeleteStaffService softDeleteStaffService)
        {
            _getAllStaffService = getAllStaffService;
            _getStaffByIdService = getStaffByIdService;
            _updateStaffService = updateStaffService;
            _softDeleteStaffService = softDeleteStaffService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<StaffDto>>>> GetAllStaff()
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<List<StaffDto>>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getAllStaffService.GetAllStaffAsync(tenantId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{staffId}")]
        public async Task<ActionResult<ApiResponse<StaffDto>>> GetStaffById(int staffId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<StaffDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getStaffByIdService.GetStaffByIdAsync(tenantId, staffId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{staffId}")]
        public async Task<ActionResult<ApiResponse<StaffDto>>> UpdateStaff(int staffId, [FromForm] UpdateStaffDto dto)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<StaffDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _updateStaffService.UpdateStaffAsync(tenantId, staffId, dto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{staffId}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDeleteStaff(int staffId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<bool>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _softDeleteStaffService.SoftDeleteStaffAsync(tenantId, staffId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
