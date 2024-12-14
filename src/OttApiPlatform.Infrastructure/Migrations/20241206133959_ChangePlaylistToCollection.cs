using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlaylistToCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistAssets");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SeoTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SeoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Permalink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BannerWebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BannerMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BannerTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosterWebId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosterMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosterTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collections_Assets_BannerMobileId",
                        column: x => x.BannerMobileId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Collections_Assets_BannerTvAppsId",
                        column: x => x.BannerTvAppsId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Collections_Assets_BannerWebsiteId",
                        column: x => x.BannerWebsiteId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Collections_Assets_ImageAssetId",
                        column: x => x.ImageAssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Collections_Assets_PosterMobileId",
                        column: x => x.PosterMobileId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Collections_Assets_PosterTvAppsId",
                        column: x => x.PosterTvAppsId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Collections_Assets_PosterWebId",
                        column: x => x.PosterWebId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MuxSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SigningSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorsOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RtmpUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RtmpsUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuxConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollectionsAssets",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionsAssets", x => new { x.AssetId, x.CollectionId });
                    table.ForeignKey(
                        name: "FK_CollectionsAssets_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionsAssets_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collections_BannerMobileId",
                table: "Collections",
                column: "BannerMobileId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_BannerTvAppsId",
                table: "Collections",
                column: "BannerTvAppsId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_BannerWebsiteId",
                table: "Collections",
                column: "BannerWebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_ImageAssetId",
                table: "Collections",
                column: "ImageAssetId",
                unique: true,
                filter: "[ImageAssetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_PosterMobileId",
                table: "Collections",
                column: "PosterMobileId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_PosterTvAppsId",
                table: "Collections",
                column: "PosterTvAppsId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_PosterWebId",
                table: "Collections",
                column: "PosterWebId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionsAssets_CollectionId",
                table: "CollectionsAssets",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionsAssets_Order",
                table: "CollectionsAssets",
                column: "Order",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionsAssets");

            migrationBuilder.DropTable(
                name: "MuxSettings");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BannerMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BannerTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BannerWebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosterMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosterTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosterWebId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Permalink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SeoTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlists_Assets_BannerMobileId",
                        column: x => x.BannerMobileId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Playlists_Assets_BannerTvAppsId",
                        column: x => x.BannerTvAppsId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Playlists_Assets_BannerWebsiteId",
                        column: x => x.BannerWebsiteId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Playlists_Assets_ImageAssetId",
                        column: x => x.ImageAssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playlists_Assets_PosterMobileId",
                        column: x => x.PosterMobileId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Playlists_Assets_PosterTvAppsId",
                        column: x => x.PosterTvAppsId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Playlists_Assets_PosterWebId",
                        column: x => x.PosterWebId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistAssets",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistAssets", x => new { x.AssetId, x.PlaylistId });
                    table.ForeignKey(
                        name: "FK_PlaylistAssets_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistAssets_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistAssets_Order",
                table: "PlaylistAssets",
                column: "Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistAssets_PlaylistId",
                table: "PlaylistAssets",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_BannerMobileId",
                table: "Playlists",
                column: "BannerMobileId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_BannerTvAppsId",
                table: "Playlists",
                column: "BannerTvAppsId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_BannerWebsiteId",
                table: "Playlists",
                column: "BannerWebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_ImageAssetId",
                table: "Playlists",
                column: "ImageAssetId",
                unique: true,
                filter: "[ImageAssetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_PosterMobileId",
                table: "Playlists",
                column: "PosterMobileId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_PosterTvAppsId",
                table: "Playlists",
                column: "PosterTvAppsId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_PosterWebId",
                table: "Playlists",
                column: "PosterWebId");
        }
    }
}
