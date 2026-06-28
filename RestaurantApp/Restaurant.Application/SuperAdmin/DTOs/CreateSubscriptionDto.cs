namespace Restaurant.Application.SuperAdmin.DTOs;

public class CreateSubscriptionDto
{
    public string Name { get; set; } = string.Empty;
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public int MaxTables { get; set; } = 10;
    public int MaxStaff { get; set; } = 5;
    public long StorageLimitMb { get; set; } = 1024;
    public bool HasNotifications { get; set; } = false;
    public bool HasAnalytics { get; set; } = false;
    public string? FeaturesJson { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
