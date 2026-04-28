using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class DiningTable
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int TableNumber { get; set; }

        public string QrToken { get; set; } = string.Empty;
        public string? QrUrl { get; set; }
        public string? QrCodeImageUrl { get; set; }
        public bool IsOccupied { get; set; } = false;

        public int? Capacity { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public Tenant Tenant { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
