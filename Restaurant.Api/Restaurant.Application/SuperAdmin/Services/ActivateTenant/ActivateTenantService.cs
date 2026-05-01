using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces.ActivateTenant;

namespace Restaurant.Application.SuperAdmin.Services.ActivateTenant;

public class ActivateTenantService : IActivateTenantService
{
    private readonly IActivateTenantRepository _repository;

    public ActivateTenantService(IActivateTenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> ActivateTenantAsync(int tenantId)
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
                    "Cannot activate deleted tenant",
                    new List<string> { $"Tenant with ID {tenantId} is marked as deleted. Please restore it first." });
            }

            // Check if already active
            if (tenant.IsActive)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Tenant already active",
                    new List<string> { $"Tenant with ID {tenantId} is already active" });
            }

            // Activate the tenant
            var result = await _repository.ActivateTenantAsync(tenant);

            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "Failed to activate tenant. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Tenant activated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ServerErrorResponse(
                "An error occurred while activating the tenant. Please try again later.");
        }
    }
}
