using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(c => c.Id);

            // 📌 Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.IsoCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.PhoneCode)
                .HasMaxLength(10);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            builder.Property(c => c.UpdatedAt)
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            // 🚀 Unique Constraints
            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasIndex(c => c.IsoCode)
                .IsUnique();

            // 🔗 Relationships
            builder.HasMany(c => c.Tenants)
                .WithOne(t => t.Country)
                .HasForeignKey(t => t.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Fix: DateTimeKind.Utc required for Npgsql
            var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Country { Id = 1, Name = "India", IsoCode = "IN", PhoneCode = "+91", IsActive = true, CreatedAt = createdAt, UpdatedAt = createdAt },
                new Country { Id = 2, Name = "United States", IsoCode = "US", PhoneCode = "+1", IsActive = true, CreatedAt = createdAt, UpdatedAt = createdAt },
                new Country { Id = 3, Name = "United Kingdom", IsoCode = "GB", PhoneCode = "+44", IsActive = true, CreatedAt = createdAt, UpdatedAt = createdAt },
                new Country { Id = 4, Name = "Canada", IsoCode = "CA", PhoneCode = "+1", IsActive = true, CreatedAt = createdAt, UpdatedAt = createdAt },
                new Country { Id = 5, Name = "Australia", IsoCode = "AU", PhoneCode = "+61", IsActive = true, CreatedAt = createdAt, UpdatedAt = createdAt }
            );
        }
    }
}