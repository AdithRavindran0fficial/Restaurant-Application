using System;

namespace Restaurant.Application.Admin.DTOs
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVeg { get; set; }
        public int? PreparationTime { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
