using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal PriceMonthly { get; set; } = 0;

        public decimal? PriceYearly { get; set; }

        public int MaxTables { get; set; } = 10;

        public int MaxStaff { get; set; } = 5;

        public long StorageLimitMb { get; set; } = 1024;

        public bool HasNotifications { get; set; } = false;

        public bool HasAnalytics { get; set; } = false;

        public string? FeaturesJson { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public ICollection<TenantSubscription> TenantSubscriptions { get; set; } = new List<TenantSubscription>();
    }
}
