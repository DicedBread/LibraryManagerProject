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

    public bool AddUser(string email, string password, string username)
    {
        throw new NotImplementedException();
    }

    public PasswordVerificationResult AuthenticateUser(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Loan? CreateLoan(string isbn, double userId, DateTime date)
    {
        throw new NotImplementedException();
    }

    public bool DeleteLoan(double loanId)
    {
        throw new NotImplementedException();
    }

    public Book? GetBook(string isbn)
    {
        throw new NotImplementedException();
        
    }

    public List<Book> GetBooks(int limit, int offset)
    {
        if(limit < 0 || offset < 0) throw new ArgumentException("Limit and offset cannot be negative");
        return new LibraryContext(dbContextOptions).Books.Skip(offset).Take(limit).ToList();
    }

    public Loan? GetLoan(double loanId)
    {
        throw new NotImplementedException();
    }

    public List<Loan> GetLoans(double userId)
    {
        throw new NotImplementedException();
    }

    public double? GetUserId(string email)
    {
        throw new NotImplementedException();
    }

    public bool HasActiveLoan(string isbn)
    {
        throw new NotImplementedException();
    }

    public bool OwnsLoan(double loanId, double userId)
    {
        throw new NotImplementedException();
    }

    public List<Book> SearchBooks(string search, int limit, int offset)
    {
        throw new NotImplementedException();
    }
}