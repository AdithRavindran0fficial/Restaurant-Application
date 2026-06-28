using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Restaurnat.Infra.Migrations
{
    /// <inheritdoc />
    public partial class modifieddiningTablemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tables_QrCode",
                table: "Tables");

            migrationBuilder.RenameColumn(
                name: "QrCode",
                table: "Tables",
                newName: "QrUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsOccupied",
                table: "Tables",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QrCodeImageUrl",
                table: "Tables",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrToken",
                table: "Tables",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "Staffs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SuperAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdmins", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tables_QrToken",
                table: "Tables",
                column: "QrToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuperAdmins");

            migrationBuilder.DropIndex(
                name: "IX_Tables_QrToken",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "IsOccupied",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "QrCodeImageUrl",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "QrToken",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "Staffs");

            migrationBuilder.RenameColumn(
                name: "QrUrl",
                table: "Tables",
                newName: "QrCode");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_QrCode",
                table: "Tables",
                column: "QrCode",
                unique: true,
                filter: "\"QrCode\" IS NOT NULL");
        }
    }
}
