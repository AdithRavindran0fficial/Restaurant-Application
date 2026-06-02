using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(c => c.Id);

            // 📌 Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.Slug)
                .HasMaxLength(150);

            builder.Property(c => c.DisplayOrder)
                .HasDefaultValue(0);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            // ✅ Fix 1: GETUTCDATE() → NOW() for PostgreSQL
            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(c => c.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🚀 Indexes
            builder.HasIndex(c => new { c.TenantId, c.Name })
                .IsUnique();

            // ✅ Fix 2: [Slug] → "Slug" for PostgreSQL
            builder.HasIndex(c => new { c.TenantId, c.Slug })
                .IsUnique()
                .HasFilter("\"Slug\" IS NOT NULL");

            // 🔗 Relationships
            builder.HasOne(c => c.Tenant)
                .WithMany(t => t.Categories)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.MenuItems)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧠 Soft delete filter
            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}