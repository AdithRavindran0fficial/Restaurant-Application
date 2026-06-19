using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Tables.GetTableById;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.GetTableById
{
    public class GetTableByIdService : IGetTableByIdService
    {
        private readonly IGetTableByIdRepository _repository;

        public GetTableByIdService(IGetTableByIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<DiningTableDto>> GetTableByIdAsync(int tenantId, int tableId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<DiningTableDto>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            if (tableId <= 0)
            {
                return ApiResponse<DiningTableDto>.ValidationErrorResponse(
                    "Invalid table ID",
                    new List<string> { "Table ID must be greater than 0" });
            }

            var table = await _repository.GetTableByIdAsync(tenantId, tableId);

            if (table == null)
            {
                return ApiResponse<DiningTableDto>.NotFoundResponse(
                    $"Table with ID {tableId} not found");
            }

            var dto = new DiningTableDto
            {
                Id = table.Id,
                TenantId = table.TenantId,
                TableNumber = table.TableNumber,
                QrToken = table.QrToken,
                QrUrl = table.QrUrl,
                QrCodeImageUrl = table.QrCodeImageUrl,
                Capacity = table.Capacity,
                IsActive = table.IsActive,
                CreatedAt = table.CreatedAt,
                UpdatedAt = table.UpdatedAt
            };

            return ApiResponse<DiningTableDto>.SuccessResponse(
                dto,
                "Table retrieved successfully");
        }
    }
}
