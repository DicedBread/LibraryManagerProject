using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace library_manager_server;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Authour> Authours { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authour>(entity =>
        {
            entity.HasKey(e => e.AuthourId).HasName("authours_pkey");

            entity.ToTable("authours");

            entity.Property(e => e.AuthourId).HasColumnName("authour_id");
            entity.Property(e => e.Authour1)
                .HasMaxLength(500)
                .HasColumnName("authour");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("books_pkey");

            entity.ToTable("books");

            entity.HasIndex(e => e.TextSearch, "text_search_idx").HasMethod("gin");

            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("isbn");
            entity.Property(e => e.AuthourId)
                .HasDefaultValue(1L)
                .HasColumnName("authour_id");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(200)
                .HasColumnName("img_url");
            entity.Property(e => e.PublisherId)
                .HasDefaultValue(1L)
                .HasColumnName("publisher_id");
            entity.Property(e => e.TextSearch)
                .HasComputedColumnSql("to_tsvector('english'::regconfig, (title)::text)", true)
                .HasColumnName("text_search");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");

            entity.HasOne(d => d.Authour).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_authours");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_publisher");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("loans_pkey1");

            entity.ToTable("loans");

            entity.HasIndex(e => new { e.UserId, e.Isbn }, "loans_user_id_isbn_key").IsUnique();

            entity.Property(e => e.LoanId)
                .HasDefaultValueSql("nextval('loans_loan_id_seq1'::regclass)")
                .HasColumnName("loan_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("isbn");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Loans)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loans_isbn_fkey1");

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loans_user_id_fkey");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("publishers_pkey");

            entity.ToTable("publishers");

            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Publisher1)
                .HasMaxLength(200)
                .HasColumnName("publisher");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey1");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
