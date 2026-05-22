using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics;
using Restaurant.Application.SuperAdmin.Interfaces.DashBoard;

namespace Restaurant.Api.Controllers.SuperAdmin.DashBoard
{
    [Route("api/v1/super/dashboard")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardAnalyticsService _dashBoardAnalyticsService;

        public DashBoardController(IDashBoardAnalyticsService dashBoardAnalyticsService)
        {
            _dashBoardAnalyticsService = dashBoardAnalyticsService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SuperAdminDashBoard>>> GetDashBoardAnalytics()
        {
            var result = await _dashBoardAnalyticsService.GetDashBoardAnalyticsAsync();

            return StatusCode(result.StatusCode, result);
        }
    }
}
