using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics
{
    public class RevenueStatsDTO
    {
        public decimal TotalRevenueThisMonth { get; set; }
        public decimal TotalRevenueAllTime { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int ExpiringSoon { get; set; }
    }
}
