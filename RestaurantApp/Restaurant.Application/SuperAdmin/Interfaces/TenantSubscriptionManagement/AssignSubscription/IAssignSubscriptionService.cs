using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.AssignSubscription
{
    public interface IAssignSubscriptionService
    {
        Task<ApiResponse<TenantSubscriptionDto>> AssignSubscriptionAsync(int tenantId, AssignSubscriptionDto dto);
    }
}
