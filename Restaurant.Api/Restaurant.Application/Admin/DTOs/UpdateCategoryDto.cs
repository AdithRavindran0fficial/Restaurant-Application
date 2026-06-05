using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.Admin.DTOs
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [MaxLength(500, ErrorMessage = "ImageUrl cannot exceed 500 characters")]
        public string? ImageUrl { get; set; }

        public int? DisplayOrder { get; set; }

        [MaxLength(150, ErrorMessage = "Slug cannot exceed 150 characters")]
        public string? Slug { get; set; }

        public bool IsActive { get; set; }
    }
}
