using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.DTOs
{
    public class SessionDTO
    {
        public string SessionToken { get; set; } = string.Empty;
        public int TenantId { get; set; }

        public int TableNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

    }
}
