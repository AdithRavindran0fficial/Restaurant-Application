using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class DiningTableConfiguration : IEntityTypeConfiguration<DiningTable>
    {
        public void Configure(EntityTypeBuilder<DiningTable> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(dt => dt.Id);

            // 📌 Properties
            builder.Property(dt => dt.TableNumber)
                .IsRequired();

            builder.Property(dt => dt.QrCode)
                .HasMaxLength(500);

            builder.Property(dt => dt.Capacity);

            builder.Property(dt => dt.IsActive)
                .HasDefaultValue(true);

            builder.Property(dt => dt.IsDeleted)
                .HasDefaultValue(false);

            // ✅ Fix 1: GETUTCDATE() → NOW()
            builder.Property(dt => dt.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(dt => dt.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🚀 Indexes
            builder.HasIndex(dt => new { dt.TenantId, dt.TableNumber })
                .IsUnique();

            // ✅ Fix 2: [QrCode] → "QrCode"
            builder.HasIndex(dt => dt.QrCode)
                .IsUnique()
                .HasFilter("\"QrCode\" IS NOT NULL");

            // 🔗 Relationships
            builder.HasOne(dt => dt.Tenant)
                .WithMany(t => t.Tables)
                .HasForeignKey(dt => dt.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(dt => dt.Orders)
                .WithOne(o => o.Table)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧠 Soft delete filter
            builder.HasQueryFilter(dt => !dt.IsDeleted);
        }
    }
}