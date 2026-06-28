using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics
{
    public class SuperAdminDashBoard
    {
        public TenantStatsDTO? TenantStats { get; set; }
        public RevenueStatsDTO? RevenueStats { get; set; }
        public RecentSignupDTO? RecentSignup { get; set; }
        public TrialAccountExpiresSoonDTO? TrialAccountExpiresSoon { get; set; }
    }
}
