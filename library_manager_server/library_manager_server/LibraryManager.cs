using library_manager_server;
using library_manager_server.model;
using Microsoft.AspNetCore.Identity;
using Npgsql;

public class LibraryManager : ILibraryManger{

	private readonly NpgsqlDataSource _dataSource;

	//ILogger logger;


	// TODO add session cache
	public LibraryManager(NpgsqlDataSource dataSource)
	{
		this._dataSource = dataSource;
		//this.logger = log;
    }

	/// <summary>
	/// get book within range limit with offset 
	/// </summary>
	/// <param name="limit">number of books to retrive</param>
	/// <param name="offset">offset to start counting limit</param>
	/// <returns></returns>
	public List<Book> GetBooks(int limit, int offset)
	{
		List<Book> books = new List<Book>();
		if (limit <= 0 || offset < 0) return books;

		const string query = @"
			SELECT isbn, title, authour, publisher, img_url FROM books 
			NATURAL JOIN authours
			NATURAL JOIN publishers
			limit @limit offset @offset
		";
		
		using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
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
	/// <param name="isbn">isbn of book to retrieve</param>
	/// <returns>book with isbn number or null if not book assosiated</returns>
	public Book? GetBook(string isbn)
	{
        const string query = """
                             SELECT isbn, title, authour, publisher, img_url FROM books 
                             NATURAL JOIN authours
                             NATURAL JOIN publishers
                             WHERE isbn = @isbn
                             """;

        using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
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
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <returns>pass or failed</returns>
    public PasswordVerificationResult AuthenticateUser(string email, string password)
    {
		//if(email == "test" || password == "test") return PasswordVerificationResult.Success;

		PasswordHasher<string> ph = new PasswordHasher<string>();

		const string emailParamName = "email";
		const string query = $" SELECT password FROM users WHERE email = @{emailParamName};";
		using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(emailParamName, email);
		using NpgsqlDataReader reader = cmd.ExecuteReader();
		if (reader.HasRows)
		{
			if (reader.Read())
			{
				string hashedPswd = reader.GetString(0);
                return ph.VerifyHashedPassword(email, hashedPswd, password);
            }
		}
		return PasswordVerificationResult.Failed;
    }

	/// <summary>
	/// inserts new user into the db
	/// </summary>
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <param name="fname"></param>
	/// <param name="lname"></param>
	/// <returns>true if added successfully false if not</returns>
	public bool AddUser(string email, string password, string fname, string lname)
	{
		string hashedPassword = new PasswordHasher<string>().HashPassword(email, password);

		string emailParamName = "emailParam", pwParamName = "pwParamName", fnameParamName = "fNParam", lnameParamName = "lNParam";
		string query = $"""
		                INSERT INTO users (email, password, fname, lname) 
		                VALUES (@{emailParamName}, @{pwParamName}, @{fnameParamName}, @{lnameParamName});
		                """;

		using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(emailParamName, email);
		cmd.Parameters.AddWithValue(pwParamName, hashedPassword);
		cmd.Parameters.AddWithValue(fnameParamName, fname);
        cmd.Parameters.AddWithValue(lnameParamName, lname);

		int ret = cmd.ExecuteNonQuery();
		if(ret > 0)
		{
			return true;
		}
        return false;
	}

	public double? GetUserId(string email)
	{
		const string emailParamName = "email";
		const string query = $" SELECT userid FROM users WHERE email = @{emailParamName}; ";
		using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(emailParamName, email);
		using NpgsqlDataReader reader = cmd.ExecuteReader();
		if (reader.Read())
		{
			double id = reader.GetDouble(reader.GetOrdinal("userid"));
			return id;
		}
		return null;
	}
} 