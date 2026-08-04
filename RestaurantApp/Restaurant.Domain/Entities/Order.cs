using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int TableId { get; set; }

        // ✅ Add these two
        public int? TableSessionId { get; set; }
        public int? CustomerId { get; set; }
        public int? StaffId { get; set; } 

        public string OrderNumber { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";
        // Pending → Confirmed → Preparing → Ready → Served → Closed → Cancelled

        // ✅ Add these
        public string PaymentStatus { get; set; } = "Unpaid";
        // Unpaid, Paid, Refunded

        public string? PaymentMethod { get; set; } = "UPI";
        // Cash, Card, UPI

        public string OrderSource { get; set; } = "QR";
        // QR, POS, Manual

        public decimal TotalAmount { get; set; } = 0;
        public string? Notes { get; set; }

        // ✅ Add these timestamps
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Tenant Tenant { get; set; }
        public DiningTable Table { get; set; }

        // ✅ Add these navigation properties
        public TableSession? TableSession { get; set; }
        public Customer? Customer { get; set; }
        public Staff? Staff { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<OrderStatusHistory> StatusHistories { get; set; } = new List<OrderStatusHistory>();
    }
}