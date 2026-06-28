using Microsoft.Extensions.Configuration;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Staff.UpdateStaff;
using Restaurant.Application.Common;
using Restaurant.Application.Common.ImageServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using StaffEntity = Restaurant.Domain.Entities.Staff;

namespace Restaurant.Application.Admin.Services.Staff.UpdateStaff
{
    public class UpdateStaffService : IUpdateStaffService
    {
        private readonly IUpdateStaffRepository _repository;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly IConfiguration _configuration;

        public UpdateStaffService(
            IUpdateStaffRepository repository,
            IImageUploaderService imageUploaderService,
            IConfiguration configuration)
        {
            _repository = repository;
            _imageUploaderService = imageUploaderService;
            _configuration = configuration;
        }

        public async Task<ApiResponse<StaffDto>> UpdateStaffAsync(int tenantId, int staffId, UpdateStaffDto dto)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<StaffDto>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            if (staffId <= 0)
            {
                return ApiResponse<StaffDto>.ValidationErrorResponse(
                    "Invalid staff ID",
                    new List<string> { "Staff ID must be greater than 0" });
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return ApiResponse<StaffDto>.ValidationErrorResponse(
                    "Validation failed",
                    new List<string> { "Email is required" });
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                return ApiResponse<StaffDto>.ValidationErrorResponse(
                    "Validation failed",
                    new List<string> { "FirstName is required" });
            }

            var staff = await _repository.GetStaffByIdAsync(tenantId, staffId);
            if (staff == null)
            {
                return ApiResponse<StaffDto>.NotFoundResponse($"Staff with ID {staffId} not found");
            }

            var role = await _repository.GetRoleByIdAsync(dto.RoleId);
            if (role == null)
            {
                return ApiResponse<StaffDto>.ValidationErrorResponse(
                    "Invalid role",
                    new List<string> { $"Role with ID {dto.RoleId} not found" });
            }

            if (!string.Equals(staff.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = await _repository.StaffEmailExistsAsync(tenantId, dto.Email, staffId);
                if (emailExists)
                {
                    return ApiResponse<StaffDto>.ConflictResponse(
                        $"Staff with email '{dto.Email}' already exists for this tenant");
                }
            }

            string? profileImg = staff.ProfileImg;
            if (dto.ProfileImg != null && dto.ProfileImg.Length > 0)
            {
                await using var ms = new MemoryStream();
                await dto.ProfileImg.CopyToAsync(ms);
                var imageBytes = ms.ToArray();
                var fileName = $"staff-{tenantId}-{Guid.NewGuid():N}{Path.GetExtension(dto.ProfileImg.FileName)}";
                profileImg = await _imageUploaderService.UploadImageAsync(
                    imageBytes,
                    fileName,
                    tenantId.ToString(),
                    "staff-profiles",
                    dto.ProfileImg.ContentType);
            }

            staff.Email = dto.Email;
            staff.FirstName = dto.FirstName;
            staff.LastName = dto.LastName;
            staff.RoleId = dto.RoleId;
            staff.ProfileImg = profileImg;
            staff.IsActive = dto.IsActive;
            staff.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateStaffAsync(staff);
            if (!updated)
            {
                return ApiResponse<StaffDto>.ServerErrorResponse("Failed to update staff. Please try again later.");
            }

            var result = new StaffDto
            {
                Id = staff.Id,
                TenantId = staff.TenantId,
                Email = staff.Email,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                RoleId = staff.RoleId,
                ProfileImg = staff.ProfileImg,
                IsActive = staff.IsActive,
                LastLoginAt = staff.LastLoginAt,
                CreatedAt = staff.CreatedAt,
                UpdatedAt = staff.UpdatedAt
            };

            return ApiResponse<StaffDto>.SuccessResponse(result, "Staff updated successfully");
        }
    }
}
