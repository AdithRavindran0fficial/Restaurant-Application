using System;

namespace Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics
{
    public class RecentSignupItemDTO
    {
        public int TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }
    }
}
