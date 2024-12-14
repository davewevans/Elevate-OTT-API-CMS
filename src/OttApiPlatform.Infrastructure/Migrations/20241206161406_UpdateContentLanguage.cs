using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContentLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Content");

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Content",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Content_LanguageId",
                table: "Content",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Languages_LanguageId",
                table: "Content",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Languages_LanguageId",
                table: "Content");

            migrationBuilder.DropIndex(
                name: "IX_Content_LanguageId",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Content");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Content",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
