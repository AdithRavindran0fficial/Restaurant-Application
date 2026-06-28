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
        public class OtpVerificationConfiguration : IEntityTypeConfiguration<OtpVerification>
        {
            public void Configure(EntityTypeBuilder<OtpVerification> builder)
            {
                // Primary Key
                builder.HasKey(o => o.Id);

                // Table name
                builder.ToTable("OtpVerifications");

                // Properties
                builder.Property(o => o.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                builder.Property(o => o.OtpCode)
                    .IsRequired()
                    .HasMaxLength(6); // supports 4 or 6 digit OTPs

                builder.Property(o => o.IsUsed)
                    .HasDefaultValue(false);

                builder.Property(o => o.ExpiresAt)
                    .IsRequired();

                builder.Property(o => o.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Indexes
                // Find OTP quickly by phone number
                builder.HasIndex(o => o.PhoneNumber);

                // Find OTP by phone + used status
                builder.HasIndex(o => new { o.PhoneNumber, o.IsUsed });

                // Relationship → Tenant
                builder.HasOne(o => o.Tenant)
                    .WithMany()
                    .HasForeignKey(o => o.TenantId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}
