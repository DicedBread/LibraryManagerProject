using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace library_manager_server.Persistency;

class LibraryManagerEF : ILibraryManager
{
    private readonly DbContextOptions<LibraryContext> dbContextOptions;
    
    public LibraryManagerEF(DbContextOptions<LibraryContext> libraryContext)
    {
        this.dbContextOptions = libraryContext;
    }
    
    public Model.Book? GetBook(string isbn)
    {
        Book? b = new LibraryContext(this.dbContextOptions).Books
            .Include(e => e.Authour)
            .Include(e => e.Publisher)
            .First(e => e.Isbn == isbn);
        
        if(b == null) return null;
        return new Model.Book
        {
            Isbn = b.Isbn,
            Title = b.Title,
            Authour = b.Authour.Authour1,
            Publisher = b.Publisher.Publisher1,
            ImgUrl = b.ImgUrl
        };
    }
    
    public List<Model.Book> GetBooks(int limit, int offset)
    {
        if(limit < 0 || offset < 0) throw new ArgumentException("Limit and offset cannot be negative");
        return new LibraryContext(dbContextOptions).Books
            .Skip(offset).Take(limit)
            .Include(a => a.Authour)
            .Include(p => p.Publisher)
            .ToArray()
            .Select<Book, Model.Book>(e =>
            {
                return new Model.Book
                {
                    Isbn = e.Isbn,
                    Title = e.Title,
                    Authour = e.Authour.Authour1,
                    Publisher = e.Publisher.Publisher1,
                    ImgUrl = e.ImgUrl,
                };
            }).ToList();
    }
    
    public List<Model.Book> SearchBooks(string search, int limit, int offset)
    {
        throw new NotImplementedException();
        if(limit < 0 || offset < 0) throw new ArgumentException("Limit and offset cannot be negative");
        return new LibraryContext(dbContextOptions).Books
            .Skip(offset).Take(limit)
            .Include(a => a.Authour)
            .Include(p => p.Publisher)
            .ToArray()
            .Select<Book, Model.Book>(e =>
            {
                return new Model.Book
                {
                    Isbn = e.Isbn,
                    Title = e.Title,
                    Authour = e.Authour.Authour1,
                    Publisher = e.Publisher.Publisher1,
                    ImgUrl = e.ImgUrl,
                };
            }).ToList();     
    }
    
    private string stringToStQuery(string query)
	{ 
        string[] arr = query.Split(' ');
		string output = "";
		for (int i = 0; i < arr.Length; i++)
		{
			string v = arr[i];
			if (i != 0) output += "<->";
			output += v + ":*";	
		}
		return output;
	} 

    public PasswordVerificationResult AuthenticateUser(string email, string password)
    {
		PasswordHasher<string> ph = new PasswordHasher<string>();
        User? user = new LibraryContext(dbContextOptions).Users
            .First(u => u.Email == email);
        if (user is null) return PasswordVerificationResult.Failed;
        return ph.VerifyHashedPassword(email, user.Password, password);
    }

    public bool AddUser(string email, string password, string username)
    {
        throw new NotImplementedException();
    }

    public double? GetUserId(string email)
    {
        return new LibraryContext(dbContextOptions).Users.First(u => u.Email == email).UserId;
    }

    public List<Model.Loan> GetLoans(double userId)
    {
        return new LibraryContext(dbContextOptions).Loans
            .Include(e => e.IsbnNavigation)
            .Include(e => e.IsbnNavigation.Publisher)
            .Include(e => e.IsbnNavigation.Authour)
            .Where(l => l.UserId == userId)
            .ToArray()
            .Select<Loan, Model.Loan>(l =>
            {
                return new Model.Loan
                {
                    LoanId = l.LoanId,
                    UserId = l.UserId,
                    Date = l.Date,
                    Book = new Model.Book
                    {
                        Isbn = l.IsbnNavigation.Isbn,
                        Title = l.IsbnNavigation.Title,
                        Authour = l.IsbnNavigation.Authour.Authour1,
                        Publisher = l.IsbnNavigation.Publisher.Publisher1,
                        ImgUrl = l.IsbnNavigation.ImgUrl,
                    },
                };
            }).ToList();
    }

    public Model.Loan? GetLoan(double loanId)
    {
        Loan? loan = new LibraryContext(dbContextOptions).Loans.First(l => l.LoanId == loanId);
        if (loan is null) return null;
        return new Model.Loan()
        {
            LoanId = loan.LoanId,
            UserId = loan.UserId,
            Date = loan.Date,
            Book = new Model.Book()
            {
                Isbn = loan.IsbnNavigation.Isbn,
                Title = loan.IsbnNavigation.Title,
                Authour = loan.IsbnNavigation.Authour.Authour1,
                Publisher = loan.IsbnNavigation.Publisher.Publisher1,
                ImgUrl = loan.IsbnNavigation.ImgUrl,
            },
        };
    }

    public Model.Loan? CreateLoan(string isbn, double userId, DateTime date)
    {
        throw new NotImplementedException();
    }

    public bool DeleteLoan(double loanId)
    {
        throw new NotImplementedException();
    }

    public bool OwnsLoan(double loanId, double userId)
    {
        throw new NotImplementedException();
    }

    public bool HasActiveLoan(string isbn)
    {
        throw new NotImplementedException();
    }
}