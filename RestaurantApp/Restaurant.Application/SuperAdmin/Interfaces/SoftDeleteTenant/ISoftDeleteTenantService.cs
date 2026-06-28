using Restaurant.Application.Common;

namespace Restaurant.Application.SuperAdmin.Interfaces.SoftDeleteTenant;

public interface ISoftDeleteTenantService
{
    Task<ApiResponse<bool>> SoftDeleteTenantAsync(int tenantId);
}
