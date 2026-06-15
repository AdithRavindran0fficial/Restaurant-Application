using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.Admin.DTOs
{
    public class UpdateStaffDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(100, ErrorMessage = "FirstName cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "LastName cannot exceed 100 characters")]
        public string? LastName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RoleId must be greater than 0")]
        public int RoleId { get; set; }

        public IFormFile? ProfileImg { get; set; }

        public bool IsActive { get; set; }
    }
}
