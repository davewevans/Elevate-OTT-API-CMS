using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStreamingServiceAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_StreamingServiceAssetModel_StreamingServiceAssetId",
                table: "Assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamingServiceAssetModel",
                table: "StreamingServiceAssetModel");

            migrationBuilder.RenameTable(
                name: "StreamingServiceAssetModel",
                newName: "MuxAssets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamingServiceAssets",
                table: "MuxAssets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_StreamingServiceAssets_StreamingServiceAssetId",
                table: "Assets",
                column: "MuxAssetId",
                principalTable: "MuxAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_StreamingServiceAssets_StreamingServiceAssetId",
                table: "Assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamingServiceAssets",
                table: "MuxAssets");

            migrationBuilder.RenameTable(
                name: "MuxAssets",
                newName: "StreamingServiceAssetModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamingServiceAssetModel",
                table: "StreamingServiceAssetModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_StreamingServiceAssetModel_StreamingServiceAssetId",
                table: "Assets",
                column: "MuxAssetId",
                principalTable: "StreamingServiceAssetModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
