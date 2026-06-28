using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(s => s.Id);

            // 📌 Properties

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.PasswordHash)
                .IsRequired();

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.LastName)
                .HasMaxLength(100);

            builder.Property(s => s.ProfileImg)
                .HasMaxLength(500);

            // 🚀 Index (IMPORTANT)
            builder.HasIndex(s => new { s.TenantId, s.Email })
                .IsUnique(); // same email allowed in different tenants

            // 🔗 Relationships

            // ✅ Tenant relationship (YES, define it)
            builder.HasOne(s => s.Tenant)
                .WithMany(t => t.Staffs)
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Role relationship
            builder.HasOne(s => s.Role)
                .WithMany()
                .HasForeignKey(s => s.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧠 Soft delete
            builder.HasQueryFilter(s => !s.IsDeleted);

            // ⚙️ Defaults
            builder.Property(s => s.IsActive)
                .HasDefaultValue(true);

            builder.Property(s => s.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}