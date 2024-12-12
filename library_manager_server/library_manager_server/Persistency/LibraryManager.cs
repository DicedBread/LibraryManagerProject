using library_manager_server.model;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace library_manager_server;

public class LibraryManager : ILibraryManager
{

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
	/// <param name="username"></param>
	/// <returns>true if added successfully false if not</returns>
	public bool AddUser(string email, string password, string username)
	{
		string hashedPassword = new PasswordHasher<string>().HashPassword(email, password);

		string emailParamName = "emailParam", pwParamName = "pwParamName", userNameParamName = "usernameParam";
		string query = $"""
		                INSERT INTO users (email, password, username) 
		                VALUES (@{emailParamName}, @{pwParamName}, @{userNameParamName});
		                """;

		using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(emailParamName, email);
		cmd.Parameters.AddWithValue(pwParamName, hashedPassword);
		cmd.Parameters.AddWithValue(userNameParamName, username);

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
		const string query = $" SELECT user_id FROM users WHERE email = @{emailParamName}; ";
		using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(emailParamName, email);
		using NpgsqlDataReader reader = cmd.ExecuteReader();
		if (reader.Read())
		{
			double id = reader.GetDouble(reader.GetOrdinal("user_id"));
			return id;
		}
		return null;
	}

	
	public List<Loan> GetLoans(double userId)
	{
		List<Loan> loans = new List<Loan>();

		const string userIdParamName = "userIdParam";
		const string query = $@"
			SELECT loans.loan_id, loans.user_id, loans.isbn, loans.date
			FROM loans
			WHERE loans.user_id = @{userIdParamName}
			;";
		using NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(userIdParamName, userId);
		NpgsqlDataReader reader = cmd.ExecuteReader();
		while (reader.Read())
		{
			Loan loan = new Loan()
			{
				Loan_id = reader.GetInt32(reader.GetOrdinal("loan_id")),
				Isbn = reader.GetString(reader.GetOrdinal("isbn")),
				// User_id = reader.GetInt32(reader.GetOrdinal("user_id")),
				Date = reader.GetDateTime(reader.GetOrdinal("date")),
			};
			loans.Add(loan);
		}
		return loans;
	}

	public Loan? GetLoan(double loanId)
	{
		const string loanIdParamName = "loanId";
		const string query = $@"SELECT loan_id, user_id, isbn, date FROM loans WHERE loan_id = @{loanIdParamName};";
		NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(loanIdParamName, loanId);
		NpgsqlDataReader reader = cmd.ExecuteReader();
		if (reader.Read())
		{
			Loan loan = new Loan()
			{
				Loan_id = reader.GetInt32(reader.GetOrdinal("loan_id")),
				// User_id = reader.GetInt32(reader.GetOrdinal("user_id")),
				Isbn = reader.GetString(reader.GetOrdinal("isbn")),
				Date = reader.GetDateTime(reader.GetOrdinal("date"))
			};
			return loan;
		}
		return null;
	}

	/// <summary>
	/// create loan 
	/// </summary>
	/// <param name="isbn">loaned book</param>
	/// <param name="userId">user id</param>
	/// <param name="date"></param>
	/// <returns>true if succsessful otherwise false</returns>
	public bool CreateLoan(string isbn, double userId, DateTime date)
	{
		const string isbnPN = "isbn", userIdPN = "user_id", datePN = "date"; 
		const string query = $@"
			INSERT INTO loans (isbn, user_id, date)
			VALUES (@{isbnPN}, @{userIdPN}, @{datePN});
		";
		NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(isbnPN, isbn);
		cmd.Parameters.AddWithValue(userIdPN, userId);
		cmd.Parameters.AddWithValue(datePN, date);
		int ret = cmd.ExecuteNonQuery();
		if (ret == 0) { return false; }
		return true;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="loanId"></param>
	/// <returns></returns>
	public bool DeleteLoan(double loanId)
	{
		const string loanIdPn = "loan_id";
		const string query = $@"DELETE FROM loans WHERE loan_id = @{loanIdPn}"; 
		NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(loanIdPn, loanId);
		int ret = cmd.ExecuteNonQuery();
		if (ret == 0) { return false; }
		return true;
	}

	public bool OwnsLoan(double loanId, double userId)
	{
		const string loanIdPn = "loan_id";
		const string userIdPn = "user_id";
		const string query = $@"SElECT loan_id, user_id FROM loans WHERE loan_id = @{loanIdPn} AND user_id = @{userIdPn}";
		
		NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(loanIdPn, loanId);
		cmd.Parameters.AddWithValue(userIdPn, userId);
		int ret = cmd.ExecuteNonQuery();

		if (ret == 0) { return false; }
		return true;
	}
	
	/// <summary>
	/// is the book with isbn already loaned out
	/// </summary>
	/// <param name="isbn"></param>
	/// <returns></returns>
	public bool HasActiveLoan(string isbn)
	{
		const string isbnPn = "isbn";
		const string query = $@"SELECT * FROM loans WHERE isbn = @{isbnPn}";
		NpgsqlCommand cmd = _dataSource.CreateCommand(query);
		cmd.Parameters.AddWithValue(isbnPn, isbn);
		NpgsqlDataReader reader = cmd.ExecuteReader();
		if (reader.HasRows)
		{
			return true;
		}
		return false;
	}
}