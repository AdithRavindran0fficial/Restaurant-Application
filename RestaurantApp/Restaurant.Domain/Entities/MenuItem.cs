using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsVeg { get; set; } = false;

        public int? PreparationTime { get; set; } // in minutes

        public int? DisplayOrder { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public Tenant Tenant { get; set; }

        public Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
