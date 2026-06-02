using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations  // ✅ Fix 1: Typo "Restaurnat" → "Restaurant"
{
    public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(sp => sp.Description)
                .HasMaxLength(500);

            // ✅ Fix 2: decimal(18,2) → numeric(18,2)
            builder.Property(sp => sp.PriceMonthly)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(sp => sp.PriceYearly)
                .HasColumnType("numeric(18,2)");

            builder.Property(sp => sp.MaxTables).IsRequired();
            builder.Property(sp => sp.MaxStaff).IsRequired();
            builder.Property(sp => sp.StorageLimitMb).IsRequired();

            // ✅ Fix 3: nvarchar(max) → text (PostgreSQL native)
            builder.Property(sp => sp.FeaturesJson)
                .HasColumnType("text");

            builder.Property(sp => sp.IsActive)
                .HasDefaultValue(true);

            builder.Property(sp => sp.IsDeleted)
                .HasDefaultValue(false);

            builder.HasIndex(sp => sp.Name)
                .IsUnique();

            builder.HasMany(sp => sp.TenantSubscriptions)
                .WithOne(ts => ts.Plan)
                .HasForeignKey(ts => ts.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(sp => !sp.IsDeleted);

            // ✅ Fix 4: DateTimeKind.Utc for Npgsql
            var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new SubscriptionPlan
                {
                    Id = 1,
                    Name = "Free",
                    PriceMonthly = 0,
                    PriceYearly = 0,
                    MaxTables = 5,
                    MaxStaff = 2,
                    StorageLimitMb = 512,
                    HasNotifications = false,
                    HasAnalytics = false,
                    FeaturesJson = "{\"support\":\"basic\"}",
                    Description = "Free plan with limited features",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                },
                new SubscriptionPlan
                {
                    Id = 2,
                    Name = "Basic",
                    PriceMonthly = 499,
                    PriceYearly = 4999,
                    MaxTables = 15,
                    MaxStaff = 5,
                    StorageLimitMb = 2048,
                    HasNotifications = true,
                    HasAnalytics = false,
                    FeaturesJson = "{\"support\":\"priority\"}",
                    Description = "Basic plan for small restaurants",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                },
                new SubscriptionPlan
                {
                    Id = 3,
                    Name = "Pro",
                    PriceMonthly = 999,
                    PriceYearly = 9999,
                    MaxTables = 50,
                    MaxStaff = 15,
                    StorageLimitMb = 5120,
                    HasNotifications = true,
                    HasAnalytics = true,
                    FeaturesJson = "{\"support\":\"premium\"}",
                    Description = "Advanced plan with analytics",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                }
            );
        }
    }
}