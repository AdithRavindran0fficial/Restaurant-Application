namespace Restaurant.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Tenant Tenant { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}