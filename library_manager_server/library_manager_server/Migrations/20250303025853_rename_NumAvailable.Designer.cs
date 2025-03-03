﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;
using library_manager_server.ServerContext;

#nullable disable

namespace library_manager_server.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20250303025853_rename_NumAvailable")]
    partial class rename_NumAvailable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("library_manager_server.ServerContext.Author", b =>
                {
                    b.Property<long>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("author_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("AuthorId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("author");

                    b.HasKey("AuthorId")
                        .HasName("authors_pkey");

                    b.ToTable("authors", (string)null);
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Book", b =>
                {
                    b.Property<string>("Isbn")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("isbn");

                    b.Property<long>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(1L)
                        .HasColumnName("author_id");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("img_url");

                    b.Property<long>("NumAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(1L)
                        .HasColumnName("num_available");

                    b.Property<long>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(1L)
                        .HasColumnName("publisher_id");

                    b.Property<NpgsqlTsVector>("TextSearch")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasColumnName("text_search")
                        .HasComputedColumnSql("to_tsvector('english'::regconfig, (title)::text)", true);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("title");

                    b.HasKey("Isbn")
                        .HasName("books_pkey");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PublisherId");

                    b.HasIndex(new[] { "TextSearch" }, "text_search_idx");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex(new[] { "TextSearch" }, "text_search_idx"), "gin");

                    b.ToTable("books", (string)null);
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Loan", b =>
                {
                    b.Property<long>("LoanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("loan_id")
                        .HasDefaultValueSql("nextval('loans_loan_id_seq1'::regclass)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("isbn");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("LoanId")
                        .HasName("loans_pkey1");

                    b.HasIndex("Isbn");

                    b.HasIndex(new[] { "UserId", "Isbn" }, "loans_user_id_isbn_key")
                        .IsUnique();

                    b.ToTable("loans", (string)null);
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Publisher", b =>
                {
                    b.Property<long>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("publisher_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("PublisherId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("publisher");

                    b.HasKey("PublisherId")
                        .HasName("publishers_pkey");

                    b.ToTable("publishers", (string)null);
                });

            modelBuilder.Entity("library_manager_server.ServerContext.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("username");

                    b.HasKey("UserId")
                        .HasName("users_pkey1");

                    b.HasIndex(new[] { "Email" }, "users_email_key")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Book", b =>
                {
                    b.HasOne("library_manager_server.ServerContext.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .IsRequired()
                        .HasConstraintName("fk_authors");

                    b.HasOne("library_manager_server.ServerContext.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .IsRequired()
                        .HasConstraintName("fk_publisher");

                    b.Navigation("Author");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Loan", b =>
                {
                    b.HasOne("library_manager_server.ServerContext.Book", "IsbnNavigation")
                        .WithMany("Loans")
                        .HasForeignKey("Isbn")
                        .IsRequired()
                        .HasConstraintName("loans_isbn_fkey1");

                    b.HasOne("library_manager_server.ServerContext.User", "User")
                        .WithMany("Loans")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("loans_user_id_fkey");

                    b.Navigation("IsbnNavigation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Book", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("library_manager_server.ServerContext.Publisher", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("library_manager_server.ServerContext.User", b =>
                {
                    b.Navigation("Loans");
                });
#pragma warning restore 612, 618
        }
    }
}
