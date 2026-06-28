using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Restaurnat.Infra.Configurations
{
    public class SuperAdminConfiguration : IEntityTypeConfiguration<Restaurant.Domain.Entities.SuperAdmin>
    {
        public void Configure(EntityTypeBuilder<Restaurant.Domain.Entities.SuperAdmin> builder)
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

            // Seed SuperAdmin data
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var email = configuration["SuperAdmin:Email"] ?? "superadmin@restaurant.com";
            var password = configuration["SuperAdmin:Password"] ?? "SuperAdmin@123";
            var firstName = configuration["SuperAdmin:FirstName"] ?? "Super Admin";

            // Hash the password using BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            builder.HasData(new Restaurant.Domain.Entities.SuperAdmin
            {
                Id = 1,
                Email = email,
                PasswordHash = "$2a$12$GFGErZLAYIk/sKRLu0n3Heu9/pyq3HwvM4GIp8ucBlnnB9oXg4yam",
                FirstName = firstName,
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastLoginAt = null
            });
        }
    }
}
    