using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMuxConfigEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_BannerMobileId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_BannerTvAppsId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_BannerWebsiteId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PosterMobileId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PosterTvAppsId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PosterWebId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PreviewMediaId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PrimaryMediaId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Series_SeriesId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentCategories_Asset_ContentId",
                table: "ContentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPeople_Asset_ContentId",
                table: "ContentPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTags_Asset_ContentId",
                table: "ContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Asset_ContentId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_Asset_ContentId",
                table: "Subtitles");

            migrationBuilder.DropForeignKey(
                name: "FK_SwimLaneContent_Asset_ContentId",
                table: "SwimLaneContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Asset",
                table: "Asset");

            migrationBuilder.RenameTable(
                name: "Asset",
                newName: "Content");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_SeriesId",
                table: "Content",
                newName: "IX_Content_SeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_PrimaryMediaId",
                table: "Content",
                newName: "IX_Content_PrimaryMediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_PreviewMediaId",
                table: "Content",
                newName: "IX_Content_PreviewMediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_PosterWebId",
                table: "Content",
                newName: "IX_Content_PosterWebId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_PosterTvAppsId",
                table: "Content",
                newName: "IX_Content_PosterTvAppsId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_PosterMobileId",
                table: "Content",
                newName: "IX_Content_PosterMobileId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_BannerWebsiteId",
                table: "Content",
                newName: "IX_Content_BannerWebsiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_BannerTvAppsId",
                table: "Content",
                newName: "IX_Content_BannerTvAppsId");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_BannerMobileId",
                table: "Content",
                newName: "IX_Content_BannerMobileId");

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VodStreamingService",
                table: "AccountInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Content",
                table: "Content",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_BannerMobileId",
                table: "Content",
                column: "BannerMobileId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_BannerTvAppsId",
                table: "Content",
                column: "BannerTvAppsId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_BannerWebsiteId",
                table: "Content",
                column: "BannerWebsiteId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_PosterMobileId",
                table: "Content",
                column: "PosterMobileId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_PosterTvAppsId",
                table: "Content",
                column: "PosterTvAppsId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_PosterWebId",
                table: "Content",
                column: "PosterWebId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_PreviewMediaId",
                table: "Content",
                column: "PreviewMediaId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Assets_PrimaryMediaId",
                table: "Content",
                column: "PrimaryMediaId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Series_SeriesId",
                table: "Content",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCategories_Content_ContentId",
                table: "ContentCategories",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPeople_Content_ContentId",
                table: "ContentPeople",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTags_Content_ContentId",
                table: "ContentTags",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Content_ContentId",
                table: "Series",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_Content_ContentId",
                table: "Subtitles",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SwimLaneContent_Content_ContentId",
                table: "SwimLaneContent",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_BannerMobileId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_BannerTvAppsId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_BannerWebsiteId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_PosterMobileId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_PosterTvAppsId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_PosterWebId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_PreviewMediaId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Assets_PrimaryMediaId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Series_SeriesId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentCategories_Content_ContentId",
                table: "ContentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPeople_Content_ContentId",
                table: "ContentPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTags_Content_ContentId",
                table: "ContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Content_ContentId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_Content_ContentId",
                table: "Subtitles");

            migrationBuilder.DropForeignKey(
                name: "FK_SwimLaneContent_Content_ContentId",
                table: "SwimLaneContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Content",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "VodStreamingService",
                table: "AccountInfo");

            migrationBuilder.RenameTable(
                name: "Content",
                newName: "Asset");

            migrationBuilder.RenameIndex(
                name: "IX_Content_SeriesId",
                table: "Asset",
                newName: "IX_Asset_SeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_PrimaryMediaId",
                table: "Asset",
                newName: "IX_Asset_PrimaryMediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_PreviewMediaId",
                table: "Asset",
                newName: "IX_Asset_PreviewMediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_PosterWebId",
                table: "Asset",
                newName: "IX_Asset_PosterWebId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_PosterTvAppsId",
                table: "Asset",
                newName: "IX_Asset_PosterTvAppsId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_PosterMobileId",
                table: "Asset",
                newName: "IX_Asset_PosterMobileId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_BannerWebsiteId",
                table: "Asset",
                newName: "IX_Asset_BannerWebsiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_BannerTvAppsId",
                table: "Asset",
                newName: "IX_Asset_BannerTvAppsId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_BannerMobileId",
                table: "Asset",
                newName: "IX_Asset_BannerMobileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asset",
                table: "Asset",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_BannerMobileId",
                table: "Asset",
                column: "BannerMobileId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_BannerTvAppsId",
                table: "Asset",
                column: "BannerTvAppsId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_BannerWebsiteId",
                table: "Asset",
                column: "BannerWebsiteId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_PosterMobileId",
                table: "Asset",
                column: "PosterMobileId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_PosterTvAppsId",
                table: "Asset",
                column: "PosterTvAppsId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_PosterWebId",
                table: "Asset",
                column: "PosterWebId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_PreviewMediaId",
                table: "Asset",
                column: "PreviewMediaId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Assets_PrimaryMediaId",
                table: "Asset",
                column: "PrimaryMediaId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Series_SeriesId",
                table: "Asset",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCategories_Asset_ContentId",
                table: "ContentCategories",
                column: "ContentId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPeople_Asset_ContentId",
                table: "ContentPeople",
                column: "ContentId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTags_Asset_ContentId",
                table: "ContentTags",
                column: "ContentId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Asset_ContentId",
                table: "Series",
                column: "ContentId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_Asset_ContentId",
                table: "Subtitles",
                column: "ContentId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SwimLaneContent_Asset_ContentId",
                table: "SwimLaneContent",
                column: "ContentId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
