using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.SuperAdmin.DTOs.DashboardAnalytics
{
    public class TrialAccountExpiresSoonDTO
    {
        public List<TrialExpiringItemDTO> TrialAccountsExpiringSoon { get; set; } = new List<TrialExpiringItemDTO>();
    }
}
