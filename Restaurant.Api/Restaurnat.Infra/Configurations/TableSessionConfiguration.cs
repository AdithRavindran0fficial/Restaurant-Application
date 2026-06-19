using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.Configurations
{
    internal class TableSessionConfiguration : IEntityTypeConfiguration<TableSession>
    {
        public void Configure(EntityTypeBuilder<TableSession> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SessionToken)
                .IsRequired()
                .HasMaxLength(64);

            // Unique index — no two active sessions with same token
            builder.HasIndex(s => s.SessionToken)
                .IsUnique();

            builder.Property(s => s.IsActive)
                .HasDefaultValue(true);

            builder.Property(s => s.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(s => s.ClosedAt)
                .IsRequired(false);

            // Relationship → Tenant
            builder.HasOne(s => s.Tenant)
                .WithMany()
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship → DiningTable
            builder.HasOne(s => s.Table)
                .WithMany(t => t.Sessions)
                .HasForeignKey(s => s.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship → Orders
            builder.HasMany(s => s.Orders)
                .WithOne(o => o.TableSession)
                .HasForeignKey(o => o.TableSessionId)
                .OnDelete(DeleteBehavior.Restrict);
        
    }
}
}
