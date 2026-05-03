using System;
using System.Collections.Generic;

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

        public long StorageUsedMb { get; set; } = 0;

        public DateTime? TrialEndsAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation Properties
        public Country? Country { get; set; }

        public ICollection<Staff> Staffs { get; set; } = new List<Staff>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<DiningTable> Tables { get; set; } = new List<DiningTable>();

        public ICollection<TenantSubscription> Subscriptions { get; set; } = new List<TenantSubscription>();
    }
}