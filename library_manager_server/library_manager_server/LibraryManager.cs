using library_manager_server;
using library_manager_server.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

public class LibrayManager : ILibraryManger{

	NpgsqlDataSource dataSource;

	public LibrayManager(NpgsqlDataSource dataSource)
	{
		this.dataSource = dataSource;
	}

	/// <summary>
	/// get book within range limit with offset 
	/// </summary>
	/// <param name="limit">number of books to retive</param>
	/// <param name="offset">offset to start counting limit</param>
	/// <returns></returns>
	public List<Book> GetBooks(int limit, int offset)
	{
		List<Book> books = new List<Book>();
		if (limit <= 0 || offset < 0) return books;

		string query = @"
			SELECT isbn, title, authour, publisher, img_url FROM books 
			NATURAL JOIN authours
			NATURAL JOIN publishers
			limit @limit offset @offset
		";
		
		using NpgsqlCommand cmd = dataSource.CreateCommand(query);
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
				Publisher = reader.GetString(3),
				ImgUrl = reader.GetString(4),
			};
			books.Add(book);
		}

		return books;
	}

	/// <summary>
	/// get book with isbn number 
	/// </summary>
	/// <param name="isbn">isbn of book to retrive</param>
	/// <returns>book with isbn number or null if not book assosiated</returns>
	public Book? GetBook(string isbn)
	{
        string query = @"
			SELECT isbn, title, authour, publisher, img_url FROM books 
			NATURAL JOIN authours
			NATURAL JOIN publishers
			WHERE isbn = @isbn
		";

        using NpgsqlCommand cmd = dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue("isbn", isbn);
		using NpgsqlDataReader reader = cmd.ExecuteReader();
		if (reader.HasRows)
		{
			Book book = new Book()
			{
				Id = reader.GetString(0),
				Title = reader.GetString(1),
				Authour = reader.GetString(2),
				Publisher = reader.GetString(3),
				ImgUrl = reader.GetString(4),
			};
			return book;
		}
		return null;
	}
	/// <summary>
	/// check if user has entered valid password
	/// </summary>
	/// <param name="username"></param>
	/// <param name="password"></param>
	/// <returns>pass or failed</returns>
    public PasswordVerificationResult AuthenticateUser(string username, string password)
    {
		PasswordHasher<string> ph = new PasswordHasher<string>();

		string usernameParamName = "username";
		string query = $@"
			SELECT password FROM customer WHERE username = @{usernameParamName};
		";

		using NpgsqlCommand cmd = dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(usernameParamName, username);
		using NpgsqlDataReader reader = cmd.ExecuteReader();
		if (reader.HasRows)
		{
			if (reader.Read())
			{
				string hashedPswd = reader.GetString(0);
                return ph.VerifyHashedPassword(username, hashedPswd, password);
            }
		}

		return PasswordVerificationResult.Failed;
    }
} 