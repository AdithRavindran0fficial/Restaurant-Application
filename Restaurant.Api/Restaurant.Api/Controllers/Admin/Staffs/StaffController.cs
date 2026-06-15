using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Staff.GetAllStaff;
using Restaurant.Application.Admin.Interfaces.Staff.GetStaffById;
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

        public StaffController(
            IGetAllStaffService getAllStaffService,
            IGetStaffByIdService getStaffByIdService)
        {
            _getAllStaffService = getAllStaffService;
            _getStaffByIdService = getStaffByIdService;
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
    }
}
