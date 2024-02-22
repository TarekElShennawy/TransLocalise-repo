using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Translator_Project_Management.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserSourceLineMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSourceLineMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true),
                    SourceLineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSourceLineMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSourceLineMapping_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSourceLineMapping_source_text_SourceLineId",
                        column: x => x.SourceLineId,
                        principalTable: "source_text",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_translations_lang_id",
                table: "translations",
                column: "lang_id");

            migrationBuilder.CreateIndex(
                name: "IX_source_text_lang_id",
                table: "source_text",
                column: "lang_id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_language",
                table: "AspNetUsers",
                column: "language");

            migrationBuilder.CreateIndex(
                name: "IX_UserSourceLineMapping_SourceLineId",
                table: "UserSourceLineMapping",
                column: "SourceLineId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSourceLineMapping_UserId",
                table: "UserSourceLineMapping",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Languages_language",
                table: "AspNetUsers",
                column: "language",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_source_text_Languages_lang_id",
                table: "source_text",
                column: "lang_id",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_translations_Languages_lang_id",
                table: "translations",
                column: "lang_id",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Languages_language",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_source_text_Languages_lang_id",
                table: "source_text");

            migrationBuilder.DropForeignKey(
                name: "FK_translations_Languages_lang_id",
                table: "translations");

            migrationBuilder.DropTable(
                name: "UserSourceLineMapping");

            migrationBuilder.DropIndex(
                name: "IX_translations_lang_id",
                table: "translations");

            migrationBuilder.DropIndex(
                name: "IX_source_text_lang_id",
                table: "source_text");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_language",
                table: "AspNetUsers");
        }
    }
}
