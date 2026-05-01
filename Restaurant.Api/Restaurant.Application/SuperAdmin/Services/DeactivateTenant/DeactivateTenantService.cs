using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces.DeactivateTenant;

namespace Restaurant.Application.SuperAdmin.Services.DeactivateTenant;

public class DeactivateTenantService : IDeactivateTenantService
{
    private readonly IDeactivateTenantRepository _repository;

    public DeactivateTenantService(IDeactivateTenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> DeactivateTenantAsync(int tenantId)
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

            // Check if tenant is deleted
            if (tenant.IsDeleted)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Cannot deactivate deleted tenant",
                    new List<string> { $"Tenant with ID {tenantId} is marked as deleted" });
            }

            // Check if already inactive
            if (!tenant.IsActive)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Tenant already inactive",
                    new List<string> { $"Tenant with ID {tenantId} is already inactive" });
            }

            // Deactivate the tenant
            var result = await _repository.DeactivateTenantAsync(tenant);

            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "Failed to deactivate tenant. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Tenant deactivated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ServerErrorResponse(
                "An error occurred while deactivating the tenant. Please try again later.");
        }
    }
}
