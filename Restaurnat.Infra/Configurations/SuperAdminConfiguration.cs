using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.Configurations
{
    public class SuperAdminConfiguration : IEntityTypeConfiguration<SuperAdmin>
    {
        public void Configure(EntityTypeBuilder<SuperAdmin> builder)
        {
            builder.ToTable("SuperAdmins");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.LastLoginAt)
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
    