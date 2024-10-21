using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTenantModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Tenants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
