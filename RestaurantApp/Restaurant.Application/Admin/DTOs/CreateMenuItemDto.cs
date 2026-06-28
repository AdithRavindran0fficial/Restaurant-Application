using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Restaurant.Application.Admin.DTOs
{
    public class CreateMenuItemDto
    {
        [Required(ErrorMessage = "CategoryId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public bool IsVeg { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PreparationTime must be greater than 0")]
        public int? PreparationTime { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "DisplayOrder cannot be negative")]
        public int? DisplayOrder { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public IFormFile? Image { get; set; }
    }
}
