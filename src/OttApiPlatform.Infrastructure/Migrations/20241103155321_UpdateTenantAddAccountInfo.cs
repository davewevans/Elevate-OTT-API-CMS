using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTenantAddAccountInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomDomain",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "LicenseKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "StorageFileNamePrefix",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SubDomain",
                table: "Tenants");

            migrationBuilder.CreateTable(
                name: "AccountInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubDomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageFileNamePrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountInfo");

            migrationBuilder.AddColumn<string>(
                name: "CustomDomain",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseKey",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorageFileNamePrefix",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubDomain",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
