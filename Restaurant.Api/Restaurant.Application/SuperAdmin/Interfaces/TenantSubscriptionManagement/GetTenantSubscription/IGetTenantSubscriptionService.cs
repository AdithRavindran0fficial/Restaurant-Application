using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;

namespace Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.GetTenantSubscription;

public interface IGetTenantSubscriptionService
{
    Task<ApiResponse<TenantSubscriptionDto>> GetTenantSubscriptionAsync(int tenantId);
}
