using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class TenantSubscriptionConfiguration : IEntityTypeConfiguration<TenantSubscription>
    {
        public void Configure(EntityTypeBuilder<TenantSubscription> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(ts => ts.Id);

            // 📌 Properties
            builder.Property(ts => ts.BillingCycle)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(ts => ts.Status)
                .IsRequired()
                .HasMaxLength(20);

            // ✅ Fix 1: decimal(18,2) → numeric(18,2)
            builder.Property(ts => ts.Price)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(ts => ts.IsActive)
                .HasDefaultValue(true);

            builder.Property(ts => ts.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(ts => ts.StartDate)
                .IsRequired();

            // ✅ Fix 2: GETUTCDATE() → NOW()
            builder.Property(ts => ts.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(ts => ts.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🔗 Relationships
            builder.HasOne(ts => ts.Tenant)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(ts => ts.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ts => ts.Plan)
                .WithMany(p => p.TenantSubscriptions)
                .HasForeignKey(ts => ts.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Fix 3: SQL Server filter syntax → PostgreSQL
            builder.HasIndex(ts => new { ts.TenantId, ts.IsActive })
                .HasFilter("\"IsActive\" = true")
                .IsUnique();

            // 🧠 Soft delete filter
            builder.HasQueryFilter(ts => !ts.IsDeleted);
        }
    }
}