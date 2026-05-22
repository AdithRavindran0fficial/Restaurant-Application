using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics
{
    public class RecentSignupDTO
    {
        public List<RecentSignupItemDTO> RecentSignups { get; set; } = new List<RecentSignupItemDTO>();
    }
}
