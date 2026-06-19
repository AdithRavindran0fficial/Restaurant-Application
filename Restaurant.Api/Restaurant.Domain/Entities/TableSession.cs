using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class TableSession
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int TableId { get; set; }
        public string SessionToken { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        // Navigation
        public Tenant Tenant { get; set; }
        public DiningTable Table { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
