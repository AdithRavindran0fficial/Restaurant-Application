using System;

namespace Restaurant.Application.Admin.DTOs
{
    public class StaffDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public int RoleId { get; set; }
        public string? ProfileImg { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
