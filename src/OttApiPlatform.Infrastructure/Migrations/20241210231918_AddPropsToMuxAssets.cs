using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropsToMuxAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Duration",
                table: "MuxAssets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxStoredFrameRate",
                table: "MuxAssets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MaxStoredResolution",
                table: "MuxAssets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionTier",
                table: "MuxAssets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "MuxAssets");

            migrationBuilder.DropColumn(
                name: "MaxStoredFrameRate",
                table: "MuxAssets");

            migrationBuilder.DropColumn(
                name: "MaxStoredResolution",
                table: "MuxAssets");

            migrationBuilder.DropColumn(
                name: "ResolutionTier",
                table: "MuxAssets");
        }
    }
}
