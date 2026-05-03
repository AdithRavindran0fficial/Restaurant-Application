using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurnat.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageUsedMbToTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StorageUsedMb",
                table: "Tenants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageUsedMb",
                table: "Tenants");
        }
    }
}
