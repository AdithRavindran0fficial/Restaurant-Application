using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Tenant
    {
        public int Id { get; set; }

        public Guid TenantGuid { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string PrimaryEmail { get; set; } = string.Empty;

        public string? PrimaryPhone { get; set; }

        public int? CountryId { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime? TrialEndsAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation Properties
        public Country? Country { get; set; }

        public ICollection<Staff> Staffs { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<DiningTable> Tables { get; set; }

        public ICollection<TenantSubscription> Subscriptions { get; set; }
    }
}
