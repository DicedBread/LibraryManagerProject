using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library_manager_server.Migrations
{
    /// <inheritdoc />
    public partial class rename_NumAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumAvailable",
                table: "books",
                newName: "num_available");

            migrationBuilder.AlterColumn<long>(
                name: "num_available",
                table: "books",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "num_available",
                table: "books",
                newName: "NumAvailable");

            migrationBuilder.AlterColumn<long>(
                name: "NumAvailable",
                table: "books",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);
        }
    }
}
