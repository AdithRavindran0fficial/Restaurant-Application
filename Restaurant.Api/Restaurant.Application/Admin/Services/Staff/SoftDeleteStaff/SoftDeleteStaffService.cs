using Restaurant.Application.Admin.Interfaces.Staff.SoftDeleteStaff;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffEntity = Restaurant.Domain.Entities.Staff;

namespace Restaurant.Application.Admin.Services.Staff.SoftDeleteStaff
{
    public class SoftDeleteStaffService : ISoftDeleteStaffService
    {
        private readonly ISoftDeleteStaffRepository _repository;

        public SoftDeleteStaffService(ISoftDeleteStaffRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> SoftDeleteStaffAsync(int tenantId, int staffId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            if (staffId <= 0)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Invalid staff ID",
                    new List<string> { "Staff ID must be greater than 0" });
            }

            StaffEntity? staff = await _repository.GetStaffByIdAsync(tenantId, staffId);
            if (staff == null)
            {
                return ApiResponse<bool>.NotFoundResponse($"Staff with ID {staffId} not found");
            }

            if (staff.IsDeleted)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Staff already deleted",
                    new List<string> { $"Staff with ID {staffId} is already marked as deleted" });
            }

            var result = await _repository.SoftDeleteStaffAsync(staff);
            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse("Failed to delete staff. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(true, "Staff deleted successfully");
        }
    }
}
