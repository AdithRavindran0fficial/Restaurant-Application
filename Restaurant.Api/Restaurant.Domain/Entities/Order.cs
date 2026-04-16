using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int TableId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";
        // Pending, Preparing, Ready, Completed, Cancelled

        public decimal TotalAmount { get; set; } = 0;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public Tenant Tenant { get; set; }

        public DiningTable Table { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
