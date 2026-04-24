using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(t => t.Id);

            // 📌 Properties
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.PrimaryEmail)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.PrimaryPhone)
                .HasMaxLength(20);

            // 🚀 Unique Index (Important for SaaS)
            builder.HasIndex(t => t.Slug).IsUnique();
            builder.HasIndex(t => t.PrimaryEmail).IsUnique();

            // 🌍 Relationships

            builder.HasOne(t => t.Country)
                .WithMany()
                .HasForeignKey(t => t.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Staffs)
                .WithOne(s => s.Tenant)
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Categories)
                .WithOne(c => c.Tenant)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.MenuItems)
                .WithOne(m => m.Tenant)
                .HasForeignKey(m => m.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Orders)
                .WithOne(o => o.Tenant)
                .HasForeignKey(o => o.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Tables)
                .WithOne(t => t.Tenant)
                .HasForeignKey(t => t.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Subscriptions)
                .WithOne(s => s.Tenant)
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧠 Soft Delete Filter
            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}