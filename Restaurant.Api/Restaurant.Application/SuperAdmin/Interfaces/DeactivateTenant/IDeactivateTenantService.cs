using Restaurant.Application.Common;

namespace Restaurant.Application.SuperAdmin.Interfaces.DeactivateTenant;

public interface IDeactivateTenantService
{
    Task<ApiResponse<bool>> DeactivateTenantAsync(int tenantId);
}
