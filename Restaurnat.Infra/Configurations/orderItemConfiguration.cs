using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(oi => oi.Id);

            // 📌 Properties
            builder.Property(oi => oi.Quantity)
                .IsRequired();

            // ✅ Fix 1: decimal(18,2) → numeric(18,2)
            builder.Property(oi => oi.Price)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(oi => oi.TotalPrice)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(oi => oi.Notes)
                .HasMaxLength(500);

            // ✅ Fix 2: GETUTCDATE() → NOW()
            builder.Property(oi => oi.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(oi => oi.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🚀 Indexes
            builder.HasIndex(oi => oi.OrderId);
            builder.HasIndex(oi => oi.MenuItemId);
            builder.HasIndex(oi => oi.TenantId);

            // 🔗 Relationships
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.MenuItem)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oi => oi.Tenant)
                .WithMany()
                .HasForeignKey(oi => oi.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}