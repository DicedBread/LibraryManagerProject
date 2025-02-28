using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using library_manager_server.ServerContext;
using library_manager_server.ClientContext;

namespace library_manager_server.Persistency;

public class LibraryManagerEF : ILibraryManager
{
    private readonly DbContextOptions<LibraryContext> dbContextOptions;

    public LibraryManagerEF(DbContextOptions<LibraryContext> libraryContext)
    {
        this.dbContextOptions = libraryContext;
    }

    public ClientContext.Book? GetBook(string isbn)
    {
        ServerContext.Book? b = new LibraryContext(this.dbContextOptions).Books
            .Include(e => e.Authour)
            .Include(e => e.Publisher)
            .FirstOrDefault(e => e.Isbn == isbn);

        if (b == null) return null;
        return new ClientContext.Book(b);
                
    }

    public List<ClientContext.Book> GetBooks(int limit, int offset)
    {
        if (limit < 0 || offset < 0) throw new ArgumentException("Limit and offset cannot be negative");
        return new LibraryContext(dbContextOptions).Books
            .Skip(offset).Take(limit)
            .Include(a => a.Authour)
            .Include(p => p.Publisher)
            .ToArray()
            .Select<ServerContext.Book, ClientContext.Book>(e =>
            {
                return new ClientContext.Book
                {
                    Isbn = e.Isbn,
                    Title = e.Title,
                    Authour = e.Authour.Name,
                    Publisher = e.Publisher.Name,
                    ImgUrl = e.ImgUrl,
                };
            }).ToList();
    }

    public List<ClientContext.Book> SearchBooks(string search, int limit, int offset)
    {
        if (limit < 0 || offset < 0) throw new ArgumentException("Limit and offset cannot be negative");
        string searchQuery = stringToStQuery(search);
        return new LibraryContext(dbContextOptions).Books
            .FromSql($"""
                      SELECT * FROM Books
                      WHERE text_search @@ to_tsquery({searchQuery})
                      """)
            .Skip(offset).Take(limit)
            .Include(a => a.Authour)
            .Include(p => p.Publisher)
            .ToArray()
            .Select<ServerContext.Book, ClientContext.Book>(e =>
            {
                return new ClientContext.Book
                {
                    Isbn = e.Isbn,
                    Title = e.Title,
                    Authour = e.Authour.Name,
                    Publisher = e.Publisher.Name,
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
        ServerContext.User? user = new LibraryContext(dbContextOptions).Users
            .FirstOrDefault(u => u.Email == email);
        if (user is null) return PasswordVerificationResult.Failed;
        return ph.VerifyHashedPassword(email, user.Password, password);
    }

    public bool AddUser(string email, string password, string username)
    {
        LibraryContext context = new LibraryContext(dbContextOptions);
        string hashedPw = new PasswordHasher<String>().HashPassword(email, password);
        if (null != context.Users.FirstOrDefault(E => E.Email == email)) return false;
        context.Add(new ServerContext.User { Email = email, Password = hashedPw, Username = username });
        int ret = context.SaveChanges();
        if (ret == 1) return true;
        return false;
    }

    public long? GetUserId(string email)
    {
        return new LibraryContext(dbContextOptions).Users.FirstOrDefault(u => u.Email == email)?.UserId;
    }

    public List<ClientContext.Loan> GetLoans(long userId)
    {
        return new LibraryContext(dbContextOptions).Loans
            .Include(e => e.IsbnNavigation)
            .Include(e => e.IsbnNavigation.Publisher)
            .Include(e => e.IsbnNavigation.Authour)
            .Where(l => l.UserId == userId)
            .ToArray()
            .Select<ServerContext.Loan, ClientContext.Loan>(l =>
            {
                return new ClientContext.Loan
                {
                    LoanId = l.LoanId,
                    UserId = l.UserId,
                    Date = l.Date,
                    Book = new ClientContext.Book
                    {
                        Isbn = l.IsbnNavigation.Isbn,
                        Title = l.IsbnNavigation.Title,
                        Authour = l.IsbnNavigation.Authour.Name,
                        Publisher = l.IsbnNavigation.Publisher.Name,
                        ImgUrl = l.IsbnNavigation.ImgUrl,
                    },
                };
            }).ToList();
    }

    public ClientContext.Loan? GetLoan(long loanId)
    {
        ServerContext.Loan? loan = new LibraryContext(dbContextOptions).Loans
            .Include(e => e.IsbnNavigation)
            .Include(e => e.IsbnNavigation.Publisher)
            .Include(e => e.IsbnNavigation.Authour)
            .FirstOrDefault(l => l.LoanId == loanId);
        if (loan is null) return null;
        return new ClientContext.Loan()
        {
            LoanId = loan.LoanId,
            UserId = loan.UserId,
            Date = loan.Date,
            Book = new ClientContext.Book()
            {
                Isbn = loan.IsbnNavigation.Isbn,
                Title = loan.IsbnNavigation.Title,
                Authour = loan.IsbnNavigation.Authour.Name,
                Publisher = loan.IsbnNavigation.Publisher.Name,
                ImgUrl = loan.IsbnNavigation.ImgUrl,
            },
        };
    }

    public ClientContext.Loan? CreateLoan(string isbn, long userId, DateOnly date)
    {
        try
        {
            LibraryContext context = new LibraryContext(dbContextOptions);
            EntityEntry<ServerContext.Loan> newLoan = context.Loans
                .Add(new ServerContext.Loan()
                {
                    Isbn = isbn,
                    UserId = (long)userId,
                    Date = date,
                });
            int ret = context.SaveChanges();

            if (ret == 1)
            {
                context.Entry(newLoan.Entity).Reference(e => e.IsbnNavigation).Load();
                context.Entry(newLoan.Entity.IsbnNavigation).Reference(e => e.Publisher).Load();
                context.Entry(newLoan.Entity.IsbnNavigation).Reference(e => e.Authour).Load();

                return new ClientContext.Loan
                {
                    LoanId = newLoan.Entity.LoanId,
                    UserId = newLoan.Entity.UserId,
                    Date = newLoan.Entity.Date,
                    Book = new ClientContext.Book()
                    {
                        Isbn = newLoan.Entity.Isbn,
                        Title = newLoan.Entity.IsbnNavigation.Title,
                        Authour = newLoan.Entity.IsbnNavigation.Authour.Name,
                        Publisher = newLoan.Entity.IsbnNavigation.Publisher.Name,
                        ImgUrl = newLoan.Entity.IsbnNavigation.ImgUrl,
                    },
                };
            }
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
        }
        return null;
    }

    public bool OwnsLoan(long loanId, long userId)
    {
        LibraryContext context = new LibraryContext(dbContextOptions);
        ServerContext.Loan? loan = context.Loans.FirstOrDefault(l => l.LoanId == loanId && l.UserId == userId);
        if (loan == null) return false;
        return true;
    }

    public bool DeleteLoan(long loanId)
    {
        LibraryContext context = new LibraryContext(dbContextOptions);
        ServerContext.Loan? loan = context.Loans.FirstOrDefault(l => l.LoanId == loanId);
        if (loan != null)
        {
            context.Loans.Remove(loan);
        }
        int ret = context.SaveChanges();
        if (ret == 1) return true;
        return false;
    }

    public bool HasActiveLoan(string isbn)
    {
        LibraryContext context = new LibraryContext(dbContextOptions);
        ServerContext.Loan? loan = context.Loans.FirstOrDefault(l => l.Isbn == isbn);
        if (loan == null) return false;
        return true;
    }
}