namespace Restaurant.Application.SuperAdmin.DTOs;

public class SubscriptionPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public int MaxTables { get; set; }
    public int MaxStaff { get; set; }
    public long StorageLimitMb { get; set; }
    public bool HasNotifications { get; set; }
    public bool HasAnalytics { get; set; }
    public string? FeaturesJson { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
