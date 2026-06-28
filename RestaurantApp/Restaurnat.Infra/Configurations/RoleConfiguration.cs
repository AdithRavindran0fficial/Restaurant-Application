using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(r => r.Id);

            // 📌 Properties
            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.IsSystem)
                .HasDefaultValue(false);

            builder.Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            // ✅ Fix 1: GETUTCDATE() → NOW()
            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(r => r.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // 🔗 Relationships
            builder.HasOne(r => r.Tenant)
                .WithMany()
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Staffs)
                .WithOne(s => s.Role)
                .HasForeignKey(s => s.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🚀 Unique Constraints
            builder.HasIndex(r => new { r.TenantId, r.RoleName })
                .IsUnique();

            // ✅ Fix 2: [TenantId] → "TenantId" for PostgreSQL
            builder.HasIndex(r => r.RoleName)
                .HasFilter("\"TenantId\" IS NULL")
                .IsUnique();

            // 🧠 Soft delete filter
            builder.HasQueryFilter(r => !r.IsDeleted);

            // ✅ Fix 3: DateTimeKind.Utc required for Npgsql
            var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Role
                {
                    Id = 1,
                    TenantId = null,
                    RoleName = "Admin",
                    Description = "System Admin",
                    IsSystem = true,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                },
                new Role
                {
                    Id = 2,
                    TenantId = null,
                    RoleName = "Kitchen",
                    Description = "Kitchen Staff",
                    IsSystem = true,
                    IsDeleted = false,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                }
            );
        }
    }
}