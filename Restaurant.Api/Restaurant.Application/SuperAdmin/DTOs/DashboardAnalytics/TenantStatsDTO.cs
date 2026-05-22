using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics
{
    public class TenantStatsDTO
    {
        public int TotalRestaurants { get; set; }
        public int ActiveRestaurants { get; set; }
        public int InactiveRestaurants { get; set; }
        public int TrialAccounts { get; set; }
        public int NewRestaurantsThisMonth { get; set; }
    }
}
