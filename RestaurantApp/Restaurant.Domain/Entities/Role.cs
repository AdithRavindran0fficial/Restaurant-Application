using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }  // null for system roles

        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsSystem { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public Tenant? Tenant { get; set; }

        public ICollection<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
