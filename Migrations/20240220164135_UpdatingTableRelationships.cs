using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translator_Project_Management.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingTableRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_translations_file_id",
                table: "translations",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "IX_source_text_file_id",
                table: "source_text",
                column: "file_id");

            migrationBuilder.AddForeignKey(
                name: "FK_source_text_Files_file_id",
                table: "source_text",
                column: "file_id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_translations_Files_file_id",
                table: "translations",
                column: "file_id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_source_text_Files_file_id",
                table: "source_text");

            migrationBuilder.DropForeignKey(
                name: "FK_translations_Files_file_id",
                table: "translations");

            migrationBuilder.DropIndex(
                name: "IX_translations_file_id",
                table: "translations");

            migrationBuilder.DropIndex(
                name: "IX_source_text_file_id",
                table: "source_text");
        }
    }
}
