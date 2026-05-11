using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.CancelSubscription
{
    public interface ICancelSubscriptionService
    {
        Task<ApiResponse<TenantSubscriptionDto>> CancelSubscriptionAsync(int tenantId);
    }
}
