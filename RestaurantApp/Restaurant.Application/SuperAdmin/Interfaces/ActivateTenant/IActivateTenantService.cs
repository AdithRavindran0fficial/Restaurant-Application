using Restaurant.Application.Common;

namespace Restaurant.Application.SuperAdmin.Interfaces.ActivateTenant;

public interface IActivateTenantService
{
    Task<ApiResponse<bool>> ActivateTenantAsync(int tenantId);
}
