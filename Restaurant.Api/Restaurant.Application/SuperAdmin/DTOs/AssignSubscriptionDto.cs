using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.SuperAdmin.DTOs
{
    public class AssignSubscriptionDto
    {
        [Required(ErrorMessage = "PlanId is required")]
        public int PlanId { get; set; }

        [Required(ErrorMessage = "BillingCycle is required")]
        [RegularExpression("^(monthly|yearly)$", ErrorMessage = "BillingCycle must be either 'monthly' or 'yearly'")]
        public string BillingCycle { get; set; } = "monthly";

        public bool IsTrial { get; set; } = false;

        public int? TrialDays { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
