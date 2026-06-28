using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Restaurant.Domain.Entities;

    namespace Restaurnat.Infra.Configurations
    {
        public class OrderStatusHistoryConfiguration
            : IEntityTypeConfiguration<OrderStatusHistory>
        {
            public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
            {
                builder.HasKey(o => o.Id);

                builder.ToTable("OrderStatusHistories");

                builder.Property(o => o.FromStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(o => o.ToStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(o => o.ChangedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                builder.Property(o => o.ChangedByStaffId)
                    .IsRequired(false);

                // Index for fast lookup by order
                builder.HasIndex(o => o.OrderId);
                builder.HasIndex(o => o.TenantId);

                // Relationship → Order
                builder.HasOne(o => o.Order)
                    .WithMany(o => o.StatusHistories)
                    .HasForeignKey(o => o.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship → Staff (nullable)
                builder.HasOne(o => o.ChangedBy)
                    .WithMany()
                    .HasForeignKey(o => o.ChangedByStaffId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            }
        }
    }
}
