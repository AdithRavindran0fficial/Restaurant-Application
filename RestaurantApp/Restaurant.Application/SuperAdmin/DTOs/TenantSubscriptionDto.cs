namespace Restaurant.Application.SuperAdmin.DTOs;

public class TenantSubscriptionDto
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public string BillingCycle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsTrial { get; set; }
    public DateTime? TrialEndsAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Plan details
    public SubscriptionPlanDto? Plan { get; set; }
}
