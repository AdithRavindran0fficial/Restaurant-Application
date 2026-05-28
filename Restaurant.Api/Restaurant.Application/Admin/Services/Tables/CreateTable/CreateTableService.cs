using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Tables.CreateTable;
using Restaurant.Application.Common;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.CreateTable
{
    public class CreateTableService : ICreateTableService
    {
        private readonly ICreateTableRepository _repository;

        public CreateTableService(ICreateTableRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<DiningTableDto>> CreateTableAsync(int tenantId, CreateTableDto dto)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<DiningTableDto>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            var tableNumberExists = await _repository.TableNumberExistsAsync(tenantId, dto.TableNumber);
            if (tableNumberExists)
            {
                return ApiResponse<DiningTableDto>.ConflictResponse(
                    $"Table number {dto.TableNumber} already exists for this tenant");
            }

            var table = new DiningTable
            {
                TenantId = tenantId,
                TableNumber = dto.TableNumber,
                Capacity = dto.Capacity,
                QrToken = Guid.NewGuid().ToString("N"),
                IsOccupied = false,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateTableAsync(table);

            var responseDto = new DiningTableDto
            {
                Id = created.Id,
                TenantId = created.TenantId,
                TableNumber = created.TableNumber,
                QrToken = created.QrToken,
                QrUrl = created.QrUrl,
                QrCodeImageUrl = created.QrCodeImageUrl,
                IsOccupied = created.IsOccupied,
                Capacity = created.Capacity,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };

            return ApiResponse<DiningTableDto>.CreatedResponse(
                responseDto,
                "Table created successfully");
        }
    }
}
