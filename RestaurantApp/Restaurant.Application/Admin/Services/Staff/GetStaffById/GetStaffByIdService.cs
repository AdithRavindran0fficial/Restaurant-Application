using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Staff.GetStaffById;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffEntity = Restaurant.Domain.Entities.Staff;

namespace Restaurant.Application.Admin.Services.Staff.GetStaffById
{
    public class GetStaffByIdService : IGetStaffByIdService
    {
        private readonly IGetStaffByIdRepository _repository;

        public GetStaffByIdService(IGetStaffByIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<StaffDto>> GetStaffByIdAsync(int tenantId, int staffId)
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

            StaffEntity? staff = await _repository.GetStaffByIdAsync(tenantId, staffId);
            if (staff == null)
            {
                return ApiResponse<StaffDto>.NotFoundResponse($"Staff with ID {staffId} not found");
            }

            var dto = new StaffDto
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

            return ApiResponse<StaffDto>.SuccessResponse(dto, "Staff retrieved successfully");
        }
    }
}
