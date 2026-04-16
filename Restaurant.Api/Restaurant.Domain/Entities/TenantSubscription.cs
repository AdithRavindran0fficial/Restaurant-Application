using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class TenantSubscription
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int PlanId { get; set; }

        public string BillingCycle { get; set; } = "monthly"; // monthly / yearly

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsTrial { get; set; } = false;

        public DateTime? TrialEndsAt { get; set; }

        public string Status { get; set; } = "active"; // active, expired, cancelled

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation Properties
        public Tenant Tenant { get; set; }

        public SubscriptionPlan Plan { get; set; }
    }
}
