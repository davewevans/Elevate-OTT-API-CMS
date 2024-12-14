using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameMuxConfigurationToMuxSettingsAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "MuxConfigurations",
                newName: "MuxSettings");

            migrationBuilder.AddColumn<string>(
                name: "Environment",
                table: "MuxSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnvironmentId",
                table: "MuxSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Environment",
                table: "MuxSettings");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "MuxSettings");
        }
    }
}
