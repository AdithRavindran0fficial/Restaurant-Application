using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics;
using System.Threading.Tasks;

namespace Restaurant.Application.SuperAdmin.Interfaces.DashBoard
{
    public interface IDashBoardAnalyticsService
    {
        Task<ApiResponse<SuperAdminDashBoard>> GetDashBoardAnalyticsAsync();
    }
}
