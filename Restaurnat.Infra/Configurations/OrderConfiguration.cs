using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(o => o.Id);

            // 📌 Properties
            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(20);

            // ✅ Fix 1: decimal(18,2) → numeric(18,2)
            builder.Property(o => o.TotalAmount)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(o => o.Notes)
                .HasMaxLength(500);

            // ✅ Fix 2: GETUTCDATE() → NOW()
            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(o => o.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🚀 Indexes
            builder.HasIndex(o => new { o.TenantId, o.OrderNumber })
                .IsUnique();

            builder.HasIndex(o => new { o.TenantId, o.Status });
            builder.HasIndex(o => new { o.TenantId, o.TableId });

            // 🔗 Relationships
            builder.HasOne(o => o.Tenant)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Table)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}