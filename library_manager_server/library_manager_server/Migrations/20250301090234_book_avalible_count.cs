using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library_manager_server.Migrations
{
    /// <inheritdoc />
    public partial class book_avalible_count : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NumAvailable",
                table: "books",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumAvailable",
                table: "books");
        }
    }
}
