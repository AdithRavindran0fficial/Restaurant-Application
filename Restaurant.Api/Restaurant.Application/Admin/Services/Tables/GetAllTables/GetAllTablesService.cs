using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Tables.GetAllTables;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.GetAllTables
{
    public class GetAllTablesService : IGetAllTablesService
    {
        private readonly IGetAllTablesRepository _repository;

        public GetAllTablesService(IGetAllTablesRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<DiningTableDto>>> GetAllTablesAsync(int tenantId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<List<DiningTableDto>>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            var tables = await _repository.GetAllTablesAsync(tenantId);

            var dtos = tables.Select(t => new DiningTableDto
            {
                Id = t.Id,
                TenantId = t.TenantId,
                TableNumber = t.TableNumber,
                QrToken = t.QrToken,
                QrUrl = t.QrUrl,
                QrCodeImageUrl = t.QrCodeImageUrl,
                Capacity = t.Capacity,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();

            return ApiResponse<List<DiningTableDto>>.SuccessResponse(
                dtos,
                $"{dtos.Count} table(s) retrieved successfully");
        }
    }
}
