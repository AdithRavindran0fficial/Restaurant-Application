using Microsoft.Extensions.Configuration;
using QRCoder;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Tables.RegenerateTableQr;
using Restaurant.Application.Common;
using Restaurant.Application.Common.ImageServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.RegenerateTableQr
{
    public class RegenerateTableQrService : IRegenerateTableQrService
    {
        private readonly IRegenerateTableQrRepository _repository;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly IConfiguration _configuration;

        public RegenerateTableQrService(
            IRegenerateTableQrRepository repository,
            IImageUploaderService imageUploaderService,
            IConfiguration configuration)
        {
            _repository = repository;
            _imageUploaderService = imageUploaderService;
            _configuration = configuration;
        }

        public async Task<ApiResponse<DiningTableDto>> RegenerateTableQrAsync(int tenantId, int tableId)
        {
            try
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
                    return ApiResponse<DiningTableDto>.NotFoundResponse($"Table with ID {tableId} not found");
                }

                if (table.IsDeleted)
                {
                    return ApiResponse<DiningTableDto>.ValidationErrorResponse(
                        "Cannot regenerate QR for deleted table",
                        new List<string> { $"Table with ID {tableId} is marked as deleted" });
                }

                var menuBaseUrl = _configuration["FrontEnd:MenuUrl"];
                if (string.IsNullOrWhiteSpace(menuBaseUrl))
                {
                    return ApiResponse<DiningTableDto>.ServerErrorResponse(
                        "FrontEnd:MenuUrl is not configured");
                }

                var qrToken = Guid.NewGuid().ToString("N");
                var qrUrl = $"{menuBaseUrl.TrimEnd('/')}/{qrToken}";

                var qrGenerator = new QRCodeGenerator();
                var qrData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q);
                var pngQrCode = new PngByteQRCode(qrData);
                var qrImageBytes = pngQrCode.GetGraphic(10);

                var fileName = $"table-{tenantId}-{tableId}-{qrToken}.png";
                var qrCodeImageUrl = await _imageUploaderService.UploadImageAsync(
                    qrImageBytes,
                    fileName,
                    tenantId.ToString(),
                    "qr-codes",
                    "image/png");

                table.QrToken = qrToken;
                table.QrUrl = qrUrl;
                table.QrCodeImageUrl = qrCodeImageUrl;
                table.UpdatedAt = DateTime.UtcNow;

                var updated = await _repository.UpdateTableAsync(table);
                if (!updated)
                {
                    return ApiResponse<DiningTableDto>.ServerErrorResponse(
                        "Failed to regenerate table QR. Please try again later.");
                }

                var dto = new DiningTableDto
                {
                    Id = table.Id,
                    TenantId = table.TenantId,
                    TableNumber = table.TableNumber,
                    QrToken = table.QrToken,
                    QrUrl = table.QrUrl,
                    QrCodeImageUrl = table.QrCodeImageUrl,
                    IsOccupied = table.IsOccupied,
                    Capacity = table.Capacity,
                    IsActive = table.IsActive,
                    CreatedAt = table.CreatedAt,
                    UpdatedAt = table.UpdatedAt
                };

                return ApiResponse<DiningTableDto>.SuccessResponse(dto, "Table QR regenerated successfully");
            }
            catch
            {
                return ApiResponse<DiningTableDto>.ServerErrorResponse(
                    "An error occurred while regenerating the table QR. Please try again later.");
            }
        }
    }
}
