using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces.SoftDeleteTenant;

namespace Restaurant.Application.SuperAdmin.Services.SoftDeleteTenant;

public class SoftDeleteTenantService : ISoftDeleteTenantService
{
    private readonly ISoftDeleteTenantRepository _repository;

    public SoftDeleteTenantService(ISoftDeleteTenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> SoftDeleteTenantAsync(int tenantId)
    {
        try
        {
            // Validation
            if (tenantId <= 0)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            // Get the tenant
            var tenant = await _repository.GetTenantByIdAsync(tenantId);

            if (tenant == null)
            {
                return ApiResponse<bool>.NotFoundResponse(
                    $"Tenant with ID {tenantId} not found");
            }

            // Check if already deleted
            if (tenant.IsDeleted)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Tenant already deleted",
                    new List<string> { $"Tenant with ID {tenantId} is already marked as deleted" });
            }

            // Soft delete the tenant
            var result = await _repository.SoftDeleteTenantAsync(tenant);

            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "Failed to delete tenant. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Tenant deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ServerErrorResponse(
                "An error occurred while deleting the tenant. Please try again later.");
        }
    }
}
