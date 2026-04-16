using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(c => c.Country).WithMany().HasForeignKey(e => e.CountryId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Staffs).WithOne(s => s.Tenant).HasForeignKey(s => s.TenantId);
            builder.

        }
    }
}

