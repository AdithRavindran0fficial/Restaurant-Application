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

            // ✅ QrToken - required, unique, max length
            builder.Property(dt => dt.QrToken)
                .IsRequired()
                .HasMaxLength(64);

            // ✅ QrUrl - the frontend URL encoded in QR image
            builder.Property(dt => dt.QrUrl)
                .HasMaxLength(500);

            // ✅ QrCodeImageUrl - the stored QR image file URL
            builder.Property(dt => dt.QrCodeImageUrl)
                .HasMaxLength(500);

            builder.Property(dt => dt.Capacity);

            builder.Property(dt => dt.IsOccupied)
                .HasDefaultValue(false);

            builder.Property(dt => dt.IsActive)
                .HasDefaultValue(true);

            builder.Property(dt => dt.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(dt => dt.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(dt => dt.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🚀 Indexes
            builder.HasIndex(dt => new { dt.TenantId, dt.TableNumber })
                .IsUnique();

            // ✅ QrToken must be unique across entire table
            builder.HasIndex(dt => dt.QrToken)
                .IsUnique();

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