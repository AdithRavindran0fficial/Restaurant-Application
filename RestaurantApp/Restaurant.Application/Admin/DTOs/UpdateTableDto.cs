using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.Admin.DTOs
{
    public class UpdateTableDto
    {
        [Required(ErrorMessage = "TableNumber is required")]
        [Range(1, int.MaxValue, ErrorMessage = "TableNumber must be greater than 0")]
        public int TableNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int? Capacity { get; set; }

        public bool IsActive { get; set; }
    }
}
