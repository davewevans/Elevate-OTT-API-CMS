using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration1142024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_FullName",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Tenants",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "References",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUserTokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetRoles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetPermissions",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_Name",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tenants",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "References",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUserTokens",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetRoles",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetPermissions",
                newName: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_FullName",
                table: "Tenants",
                column: "FullName",
                unique: true,
                filter: "[FullName] IS NOT NULL");
        }
    }
}
