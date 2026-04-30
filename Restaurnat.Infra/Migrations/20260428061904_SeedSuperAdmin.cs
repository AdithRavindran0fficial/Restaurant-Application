using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurnat.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SuperAdmins",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastLoginAt", "PasswordHash" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "superadmin@restaurant.com", "Super Admin", true, null, "$2a$12$GFGErZLAYIk/sKRLu0n3Heu9/pyq3HwvM4GIp8ucBlnnB9oXg4yam" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmins",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
