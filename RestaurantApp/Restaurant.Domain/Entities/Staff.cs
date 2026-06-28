using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Staff
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; }

        public int FailedLoginAttempts { get; set; } = 0;
        public int RoleId { get; set; }

        public string? ProfileImg { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime? LastLoginAt { get; set; }

        public DateTime? LockedUntil { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpiry { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public Tenant Tenant { get; set; }

        public Role Role { get; set; }
    }
}
