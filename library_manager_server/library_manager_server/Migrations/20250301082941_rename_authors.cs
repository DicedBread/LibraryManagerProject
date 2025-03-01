using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library_manager_server.Migrations
{
    /// <inheritdoc />
    public partial class rename_authors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_authours",
                table: "books");

            migrationBuilder.DropPrimaryKey(
                name: "authours_pkey",
                table: "authours");

            migrationBuilder.RenameTable(
                name: "authours",
                newName: "authors");

            migrationBuilder.RenameColumn(
                name: "authour_id",
                table: "books",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_books_authour_id",
                table: "books",
                newName: "IX_books_author_id");

            migrationBuilder.RenameColumn(
                name: "authour",
                table: "authors",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "authour_id",
                table: "authors",
                newName: "author_id");

            migrationBuilder.AddPrimaryKey(
                name: "authors_pkey",
                table: "authors",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_authors",
                table: "books",
                column: "author_id",
                principalTable: "authors",
                principalColumn: "author_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_authors",
                table: "books");

            migrationBuilder.DropPrimaryKey(
                name: "authors_pkey",
                table: "authors");

            migrationBuilder.RenameTable(
                name: "authors",
                newName: "authours");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "books",
                newName: "authour_id");

            migrationBuilder.RenameIndex(
                name: "IX_books_author_id",
                table: "books",
                newName: "IX_books_authour_id");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "authours",
                newName: "authour");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "authours",
                newName: "authour_id");

            migrationBuilder.AddPrimaryKey(
                name: "authours_pkey",
                table: "authours",
                column: "authour_id");

            migrationBuilder.AddForeignKey(
                name: "fk_authours",
                table: "books",
                column: "authour_id",
                principalTable: "authours",
                principalColumn: "authour_id");
        }
    }
}
