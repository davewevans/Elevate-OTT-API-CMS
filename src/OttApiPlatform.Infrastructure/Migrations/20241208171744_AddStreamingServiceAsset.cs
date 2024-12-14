using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStreamingServiceAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Series_SeriesId",
                table: "Content");

            migrationBuilder.DropIndex(
                name: "IX_Videos_AssetId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Content_SeriesId",
                table: "Content");

            migrationBuilder.DropIndex(
                name: "IX_Audios_AssetId",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "AudioId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PublicPlaybackId",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "VideoId",
                table: "Assets",
                newName: "MuxAssetId");

            migrationBuilder.RenameColumn(
                name: "SignedPlaybackId",
                table: "Assets",
                newName: "FileName");

            migrationBuilder.CreateTable(
                name: "StreamingServiceAssetModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingServiceAssetModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_AssetId",
                table: "Videos",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Audios_AssetId",
                table: "Audios",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_StreamingServiceAssetId",
                table: "Assets",
                column: "MuxAssetId",
                unique: true,
                filter: "[MuxAssetId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_StreamingServiceAssetModel_StreamingServiceAssetId",
                table: "Assets",
                column: "MuxAssetId",
                principalTable: "StreamingServiceAssetModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_StreamingServiceAssetModel_StreamingServiceAssetId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "StreamingServiceAssetModel");

            migrationBuilder.DropIndex(
                name: "IX_Videos_AssetId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Audios_AssetId",
                table: "Audios");

            migrationBuilder.DropIndex(
                name: "IX_Assets_StreamingServiceAssetId",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "MuxAssetId",
                table: "Assets",
                newName: "VideoId");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Assets",
                newName: "SignedPlaybackId");

            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Content",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AudioId",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicPlaybackId",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_AssetId",
                table: "Videos",
                column: "AssetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Content_SeriesId",
                table: "Content",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Audios_AssetId",
                table: "Audios",
                column: "AssetId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Series_SeriesId",
                table: "Content",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }
    }
}
