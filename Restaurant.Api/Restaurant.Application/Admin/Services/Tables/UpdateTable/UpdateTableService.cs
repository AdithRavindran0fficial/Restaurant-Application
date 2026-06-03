using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Tables.UpdateTable;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.UpdateTable
{
    public class UpdateTableService : IUpdateTableService
    {
        private readonly IUpdateTableRepository _repository;

        public UpdateTableService(IUpdateTableRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<DiningTableDto>> UpdateTableAsync(int tenantId, int tableId, UpdateTableDto dto)
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

            var validationErrors = new List<string>();

            if (dto.TableNumber <= 0)
            {
                validationErrors.Add("TableNumber must be greater than 0");
            }

            if (dto.Capacity.HasValue && dto.Capacity.Value <= 0)
            {
                validationErrors.Add("Capacity must be greater than 0");
            }

            if (validationErrors.Count > 0)
            {
                return ApiResponse<DiningTableDto>.ValidationErrorResponse("Validation failed", validationErrors);
            }

            var existing = await _repository.GetTableByIdAsync(tenantId, tableId);
            if (existing == null)
            {
                return ApiResponse<DiningTableDto>.NotFoundResponse($"Table with ID {tableId} not found");
            }

            if (existing.TableNumber != dto.TableNumber)
            {
                var tableNumberExists = await _repository.TableNumberExistsAsync(tenantId, dto.TableNumber, tableId);
                if (tableNumberExists)
                {
                    return ApiResponse<DiningTableDto>.ConflictResponse(
                        $"Table number {dto.TableNumber} already exists for this tenant");
                }
            }

            existing.TableNumber = dto.TableNumber;
            existing.Capacity = dto.Capacity;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateTableAsync(existing);
            if (!updated)
            {
                return ApiResponse<DiningTableDto>.ServerErrorResponse("Failed to update table. Please try again later.");
            }

            var responseDto = new DiningTableDto
            {
                Id = existing.Id,
                TenantId = existing.TenantId,
                TableNumber = existing.TableNumber,
                QrToken = existing.QrToken,
                QrUrl = existing.QrUrl,
                QrCodeImageUrl = existing.QrCodeImageUrl,
                IsOccupied = existing.IsOccupied,
                Capacity = existing.Capacity,
                IsActive = existing.IsActive,
                CreatedAt = existing.CreatedAt,
                UpdatedAt = existing.UpdatedAt
            };

            return ApiResponse<DiningTableDto>.SuccessResponse(responseDto, "Table updated successfully");
        }
    }
}
