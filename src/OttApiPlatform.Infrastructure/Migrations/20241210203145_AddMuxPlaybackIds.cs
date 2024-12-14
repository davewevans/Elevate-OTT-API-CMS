using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMuxPlaybackIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "StreamingServiceAssets",
            //    newName: "MuxAssets");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Assets_StreamingServiceAssets_MuxAssetId",
            //    table: "Assets");

            migrationBuilder.DropTable(
                name: "StreamingServiceAssets");

            migrationBuilder.DropColumn(
                name: "Passthrough",
                table: "Assets");

            migrationBuilder.CreateTable(
                name: "MuxAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MuxAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoQuality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MP4Support = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasterAccess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncodingTier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxResolutionTier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTestAsset = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuxAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MuxAssetTracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxWidth = table.Column<int>(type: "int", nullable: true),
                    MaxHeight = table.Column<int>(type: "int", nullable: true),
                    MaxFrameRate = table.Column<double>(type: "float", nullable: true),
                    MaxChannels = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MuxAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuxAssetTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuxAssetTracks_MuxAssets_MuxAssetId",
                        column: x => x.MuxAssetId,
                        principalTable: "MuxAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MuxPlaybackIds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Policy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaybackId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MuxAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuxPlaybackIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuxPlaybackIds_MuxAssets_MuxAssetId",
                        column: x => x.MuxAssetId,
                        principalTable: "MuxAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MuxAssetTracks_MuxAssetId",
                table: "MuxAssetTracks",
                column: "MuxAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_MuxPlaybackIds_MuxAssetId",
                table: "MuxPlaybackIds",
                column: "MuxAssetId");

            migrationBuilder.RenameColumn(
                name: "StreamingServiceAssetId",
                table: "Assets",
                newName: "MuxAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_MuxAssetId",
                table: "Assets",
                column: "MuxAssetId",
                unique: true,
                filter: "[MuxAssetId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_MuxAssets_MuxAssetId",
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
                name: "FK_Assets_MuxAssets_MuxAssetId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "MuxAssetTracks");

            migrationBuilder.DropTable(
                name: "MuxPlaybackIds");

            migrationBuilder.DropTable(
                name: "MuxAssets");

            migrationBuilder.AddColumn<string>(
                name: "Passthrough",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StreamingServiceAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingServiceAssets", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_StreamingServiceAssets_MuxAssetId",
                table: "Assets",
                column: "MuxAssetId",
                principalTable: "StreamingServiceAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
