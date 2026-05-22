using System;

namespace Restaurant.Application.Admin.DTOs
{
    public class DiningTableDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int TableNumber { get; set; }
        public string QrToken { get; set; } = string.Empty;
        public string? QrUrl { get; set; }
        public string? QrCodeImageUrl { get; set; }
        public bool IsOccupied { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
