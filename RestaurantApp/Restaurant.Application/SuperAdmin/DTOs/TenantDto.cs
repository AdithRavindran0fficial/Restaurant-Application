namespace Restaurant.Application.SuperAdmin.DTOs;

public class TenantDto
{
    public int Id { get; set; }
    public Guid TenantGuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string PrimaryEmail { get; set; } = string.Empty;
    public string? PrimaryPhone { get; set; }
    public int? CountryId { get; set; }
    public bool IsActive { get; set; }
    public long StorageUsedMb { get; set; }
    public DateTime? TrialEndsAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
