using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Tables.GetAllTables;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Security.Claims;

namespace Restaurant.Api.Controllers.Admin.Tables
{
    [Route("api/v1/admin/tables")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TableController : ControllerBase
    {
        private readonly IGetAllTablesService _getAllTablesService;

        public TableController(IGetAllTablesService getAllTablesService)
        {
            _getAllTablesService = getAllTablesService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DiningTableDto>>>> GetAllTables()
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<List<DiningTableDto>>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getAllTablesService.GetAllTablesAsync(tenantId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
