using System;
using System.Collections.Generic;
using System.Text;

    namespace Restaurant.Domain.Entities
    {
        public class OrderStatusHistory
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public int TenantId { get; set; }
            public string FromStatus { get; set; } = string.Empty;
            public string ToStatus { get; set; } = string.Empty;
            public int? ChangedByStaffId { get; set; } // null if customer/system
            public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

            // Navigation
            public Order Order { get; set; }
            public Staff? ChangedBy { get; set; }
        }
    }
