using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace library_manager_server.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authours",
                columns: table => new
                {
                    authour_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    authour = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("authours_pkey", x => x.authour_id);
                });

            migrationBuilder.CreateTable(
                name: "publishers",
                columns: table => new
                {
                    publisher_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    publisher = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("publishers_pkey", x => x.publisher_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey1", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    isbn = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    img_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    authour_id = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    publisher_id = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    text_search = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true, computedColumnSql: "to_tsvector('english'::regconfig, (title)::text)", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("books_pkey", x => x.isbn);
                    table.ForeignKey(
                        name: "fk_authours",
                        column: x => x.authour_id,
                        principalTable: "authours",
                        principalColumn: "authour_id");
                    table.ForeignKey(
                        name: "fk_publisher",
                        column: x => x.publisher_id,
                        principalTable: "publishers",
                        principalColumn: "publisher_id");
                });

            // migrationBuilder.CreateSequence<long>(name: "loans_loan_id_seq1");

            migrationBuilder.CreateTable(
                name: "loans",
                columns: table => new
                {
                    loan_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    isbn = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("loans_pkey1", x => x.loan_id);
                    table.ForeignKey(
                        name: "loans_isbn_fkey1",
                        column: x => x.isbn,
                        principalTable: "books",
                        principalColumn: "isbn");
                    table.ForeignKey(
                        name: "loans_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });


            migrationBuilder.CreateIndex(
                name: "IX_books_authour_id",
                table: "books",
                column: "authour_id");

            migrationBuilder.CreateIndex(
                name: "IX_books_publisher_id",
                table: "books",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "text_search_idx",
                table: "books",
                column: "text_search")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_loans_isbn",
                table: "loans",
                column: "isbn");

            migrationBuilder.CreateIndex(
                name: "loans_user_id_isbn_key",
                table: "loans",
                columns: new[] { "user_id", "isbn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loans");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "authours");

            migrationBuilder.DropTable(
                name: "publishers");
        }
    }
}
