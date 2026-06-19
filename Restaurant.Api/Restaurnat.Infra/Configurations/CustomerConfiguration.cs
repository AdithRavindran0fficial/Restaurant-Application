using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurnat.Infra.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(c => c.Id);

            // 📌 Table name
            builder.ToTable("Customers");

            // 📌 Properties
            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(c => c.Name)
                .HasMaxLength(150);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(c => c.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // 🚀 Indexes
            builder.HasIndex(c => new { c.TenantId, c.PhoneNumber })
                .IsUnique();

            // 🔗 Relationships

            // Tenant
            builder.HasOne(c => c.Tenant)
                .WithMany()
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Orders
            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
