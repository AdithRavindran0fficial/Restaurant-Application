using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int TenantId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; } // snapshot price

        public decimal TotalPrice { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public Order Order { get; set; }

        public MenuItem MenuItem { get; set; }

        public Tenant Tenant { get; set; }
    }
}
