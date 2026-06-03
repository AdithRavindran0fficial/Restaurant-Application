using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Tables.GetAllTables;
using Restaurant.Application.Admin.Interfaces.Tables.GetTableById;
using Restaurant.Application.Admin.Interfaces.Tables.CreateTable;
using Restaurant.Application.Admin.Interfaces.Tables.UpdateTable;
using Restaurant.Application.Admin.Interfaces.Tables.SoftDeleteTable;
using Restaurant.Application.Admin.Interfaces.Tables.ActivateTable;
using Restaurant.Application.Admin.Interfaces.Tables.DeactivateTable;
using Restaurant.Application.Admin.Interfaces.Tables.RegenerateTableQr;
using Restaurant.Application.Common;
using System.Collections.Generic;

namespace Restaurant.Api.Controllers.Admin.Tables
{
    [Route("api/v1/admin/tables")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TableController : ControllerBase
    {
        private readonly IGetAllTablesService _getAllTablesService;
        private readonly IGetTableByIdService _getTableByIdService;
        private readonly ICreateTableService _createTableService;
        private readonly IUpdateTableService _updateTableService;
        private readonly ISoftDeleteTableService _softDeleteTableService;
        private readonly IActivateTableService _activateTableService;
        private readonly IDeactivateTableService _deactivateTableService;
        private readonly IRegenerateTableQrService _regenerateTableQrService;

        public TableController(
            IGetAllTablesService getAllTablesService,
            IGetTableByIdService getTableByIdService,
            ICreateTableService createTableService,
            IUpdateTableService updateTableService,
            ISoftDeleteTableService softDeleteTableService,
            IActivateTableService activateTableService,
            IDeactivateTableService deactivateTableService)
        {
            _getAllTablesService = getAllTablesService;
            _getTableByIdService = getTableByIdService;
            _createTableService = createTableService;
            _updateTableService = updateTableService;
            _softDeleteTableService = softDeleteTableService;
            _activateTableService = activateTableService;
            _deactivateTableService = deactivateTableService;
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

        [HttpGet("{tableId}")]
        public async Task<ActionResult<ApiResponse<DiningTableDto>>> GetTableById(int tableId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<DiningTableDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getTableByIdService.GetTableByIdAsync(tenantId, tableId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DiningTableDto>>> CreateTable([FromBody] CreateTableDto dto)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<DiningTableDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _createTableService.CreateTableAsync(tenantId, dto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{tableId}")]
        public async Task<ActionResult<ApiResponse<DiningTableDto>>> UpdateTable(int tableId, [FromBody] UpdateTableDto dto)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<DiningTableDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _updateTableService.UpdateTableAsync(tenantId, tableId, dto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{tableId}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDeleteTable(int tableId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<bool>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _softDeleteTableService.SoftDeleteTableAsync(tenantId, tableId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{tableId}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateTable(int tableId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<bool>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _activateTableService.ActivateTableAsync(tenantId, tableId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{tableId}/deactivate")]
        public async Task<ActionResult<ApiResponse<bool>>> DeactivateTable(int tableId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<bool>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _deactivateTableService.DeactivateTableAsync(tenantId, tableId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{tableId}/qr/regenerate")]
        public async Task<ActionResult<ApiResponse<DiningTableDto>>> RegenerateTableQr(int tableId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<DiningTableDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _regenerateTableQrService.RegenerateTableQrAsync(tenantId, tableId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
