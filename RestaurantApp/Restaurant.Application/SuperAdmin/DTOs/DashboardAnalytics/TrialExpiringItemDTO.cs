using System;

namespace Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics
{
    public class TrialExpiringItemDTO
    {
        public int TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime TrialEndsDate { get; set; }
        public int DaysRemaining { get; set; }
    }
}
