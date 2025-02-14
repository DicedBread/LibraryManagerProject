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
        new LibraryContext(this.dbContextOptions).Books.Find(isbn);

        return null;
    }
    
    public List<Model.Book> GetBooks(int limit, int offset)
    {
        if(limit < 0 || offset < 0) throw new ArgumentException("Limit and offset cannot be negative");
        return new LibraryContext(dbContextOptions).Books.Skip(offset).Take(limit).ToArray().Select<Book, Model.Book>(e =>
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

    public PasswordVerificationResult AuthenticateUser(string email, string password)
    {
        throw new NotImplementedException();
    }

    public bool AddUser(string email, string password, string username)
    {
        throw new NotImplementedException();
    }

    public double? GetUserId(string email)
    {
        throw new NotImplementedException();
    }

    public List<Model.Loan> GetLoans(double userId)
    {
        throw new NotImplementedException();
    }

    public Model.Loan? GetLoan(double loanId)
    {
        throw new NotImplementedException();
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

    public List<Model.Book> SearchBooks(string search, int limit, int offset)
    {
        throw new NotImplementedException();
    }
}