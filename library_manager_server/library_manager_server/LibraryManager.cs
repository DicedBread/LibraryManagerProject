using library_manager_server;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

public class LibrayManager{

    NpgsqlDataSource dataSource;

    public LibrayManager(NpgsqlDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public List<Book> GetBooks()
    {
        List<Book> list = new List<Book>();
        using NpgsqlCommand command = dataSource.CreateCommand("SELECT * FROM books");
        using var reader = command.ExecuteReader();

        int i = 0;
        while (reader.Read())
        {
            Book book = new Book() {
                Id = reader.GetString(0),
                Title = reader.GetString(1),
                Authour = reader.GetString(2),
                ImgUrl = reader.GetString(3),
            };

            list.Add(book);
            
        }


        return list;
    }

    public List<Book> GetBooks(int limit, int offset)
    {
        List<Book> books = new List<Book>();
        if (limit <= 0 || offset < 0) return books;

        using NpgsqlCommand cmd = dataSource.CreateCommand("SELECT * FROM books limit @limit OFFSET @offset");
        cmd.Parameters.AddWithValue("limit", limit);
        cmd.Parameters.AddWithValue("offset", offset);
        using NpgsqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Book book = new Book()
            {
                Id = reader.GetString(0),
                Title = reader.GetString(1),
                Authour = reader.GetString(2),
                ImgUrl = reader.GetString(3),
            };
            books.Add(book);
        }

        return books;
    }

    // get book with isbn number 
    public Book? GetBook(int isbn)
    {
        using NpgsqlCommand cmd = dataSource.CreateCommand("SELECT * FROM books WHERE ISBN = @isbn");
        cmd.Parameters.AddWithValue("isbn", isbn);
        using NpgsqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            Book book = new Book()
            {
                Id = reader.GetString(0),
                Title = reader.GetString(1),
                Authour = reader.GetString(2),
                ImgUrl = reader.GetString(3)
            };
            return book;
        }
        return null;
    }
} 