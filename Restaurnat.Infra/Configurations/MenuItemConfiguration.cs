using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(m => m.Id);

            // 📌 Properties
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.Description)
                .HasMaxLength(500);

            // ✅ Fix 1: decimal(18,2) → numeric(18,2) for PostgreSQL
            builder.Property(m => m.Price)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(m => m.ImageUrl)
                .HasMaxLength(500);

            builder.Property(m => m.PreparationTime);

            builder.Property(m => m.DisplayOrder)
                .HasDefaultValue(0);

            builder.Property(m => m.IsAvailable)
                .HasDefaultValue(true);

            builder.Property(m => m.IsActive)
                .HasDefaultValue(true);

            builder.Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            // ✅ Fix 2: GETUTCDATE() → NOW()
            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🚀 Indexes
            builder.HasIndex(m => new { m.TenantId, m.CategoryId, m.Name })
                .IsUnique();

            builder.HasIndex(m => new { m.TenantId, m.CategoryId, m.DisplayOrder });

            // 🔗 Relationships
            builder.HasOne(m => m.Tenant)
                .WithMany(t => t.MenuItems)
                .HasForeignKey(m => m.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.OrderItems)
                .WithOne(oi => oi.MenuItem)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧠 Soft delete filter
            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}