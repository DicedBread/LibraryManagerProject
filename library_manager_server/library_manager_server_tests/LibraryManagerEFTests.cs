using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using library_manager_server;
using library_manager_server.Persistency;
using library_manager_server.ServerContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace library_manager_server_tests;

[TestFixture]
public class LibraryManagerEFTests
{
    private const string TestDbHost = "localhost";
    private const string TestDbName = "library";
    private const string TestDbUser = "postgres";
    private const string TestDbPassword = "1234";
    private const int TestDbPort = 5432;

    private const string testUserPassword = "abc";

    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16.2")
        .WithName("testDB")
        .WithDatabase(TestDbName)
        .WithPassword(TestDbPassword)
        .WithPortBinding(TestDbPort, 5432)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(TestDbPort))
        .Build();

    private readonly DbContextOptionsBuilder<LibraryContext> _options = new DbContextOptionsBuilder<LibraryContext>()
        .UseNpgsql(new Npgsql.NpgsqlConnectionStringBuilder()
        {
            Host = TestDbHost,
            Port = TestDbPort,
            Database = TestDbName,
            Username = TestDbUser,
            Password = TestDbPassword,
        }.ConnectionString);

    [OneTimeSetUp]
    public void oneTimeSetup()
    {
        setupAsync().Wait();
    }

    public async Task setupAsync()
    {
        await _postgreSqlContainer.StartAsync();
        Console.WriteLine("db container setup completed");
        LibraryContext context = new LibraryContext(_options.Options);
        await context.Database.MigrateAsync();
        Console.WriteLine("db migrated");
    }

    private List<Author> _authors = [];
    private List<Publisher> _publishers = [];
    private List<User> _users = [];
    private List<Book> _books = [];
    private List<Loan> _loans = [];

    [SetUp]
    public void Setup()
    {
        const int numOfAuthors = 3;
        const int numOfPublishers = 3;
        const int numOfBooks = 10;
        const int numOfUsers = 2;
        const int numOfLoans = 2;

        LibraryContext context = new LibraryContext(_options.Options);
        for (int i = 0; i < numOfAuthors; i++)
        {
            var author = new Author { Name = $"Author_{i}" };
            context.Authors.Add(author);
            _authors.Add(author);
        }
        context.SaveChanges();

        for (int i = 0; i < numOfPublishers; i++)
        {
            var publisher = new Publisher { Name = $"Publisher_{i}" };
            context.Publishers.Add(publisher);
            _publishers.Add(publisher);
        }
        context.SaveChanges();

        for (int i = 0; i < numOfBooks; i++)
        {
            var book = new Book
            {
                Isbn = "Isbn_" + i.ToString(),
                Title = "Book_" + i.ToString(),
                ImgUrl = "imgUrl_" + i.ToString(),
                AuthorId = (long)Math.Clamp(Math.Round(((numOfBooks / numOfAuthors) * 0.1) * i, 0), 0, numOfAuthors),
                NumAvailable = 1,
                PublisherId = (long)Math.Clamp(Math.Round(((numOfBooks / numOfPublishers) * 0.1) * i, 0), 0, numOfPublishers),
                TextSearch = null,
            };
            context.Books.Add(book);
            _books.Add(book);
        }

        context.SaveChanges();

        for (int i = 0; i < numOfUsers; i++)
        {
            string username = $"User_{i}";
            string hashedPw = new PasswordHasher<string>().HashPassword(username, testUserPassword);
            User user = new User { Username = username, Email = $"email_{i}", Password = hashedPw };
            context.Users.Add(user);
            _users.Add(user);
        }
        context.SaveChanges();

        for (int i = 0; i < numOfLoans; i++)
        {
            Loan loan = new Loan
            {
                UserId = 1,
                Isbn = _books[i].Isbn,
                Date = DateOnly.FromDateTime(DateTime.MinValue),
            };
            context.Loans.Add(loan);
            _loans.Add(loan);
            loan.IsbnNavigation.NumAvailable--;
        }
        context.SaveChanges();
    }

    [TearDown]
    public void Teardown()
    {
        LibraryContext context = new LibraryContext(_options.Options);
        context.Loans.RemoveRange(context.Loans.ToList());
        context.Books.RemoveRange(context.Books.ToList());
        context.Users.RemoveRange(context.Users.ToList());
        context.Authors.RemoveRange(context.Authors.ToList());
        context.Publishers.RemoveRange(context.Publishers.ToList());

        context.Database.ExecuteSql($"SELECT setval('authors_author_id_seq', 1, false)");
        context.Database.ExecuteSql($"SELECT setval('publishers_publisher_id_seq', 1, false)");
        context.Database.ExecuteSql($"SELECT setval('users_user_id_seq', 1, false)");
        context.Database.ExecuteSql($"SELECT setval('loans_loan_id_seq', 1, false)");

        context.SaveChanges();

        _loans.Clear();
        _users.Clear();
        _books.Clear();
        _publishers.Clear();
        _authors.Clear();
        Console.WriteLine("db container teardown completed");
    }

    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        Console.WriteLine("db container teardown completed");
        _postgreSqlContainer.DisposeAsync();
    }

    [Test]
    public void GetBooks_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        List<library_manager_server.ClientContext.Book> ret = lm.GetBooks(3, 0);
        Assert.That(ret.Count == 3);
    }

    [Test]
    public void GetBooks_Invalid_Params()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        Assert.Throws<ArgumentException>(() =>
        {
            List<library_manager_server.ClientContext.Book> ret = lm.GetBooks(-3, 0);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            List<library_manager_server.ClientContext.Book> ret = lm.GetBooks(0, -1);
        });
    }

    [Test]
    public void GetBook_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);

        library_manager_server.ClientContext.Book book = new library_manager_server.ClientContext.Book
        {
            Isbn = _books[0].Isbn,
            Title = _books[0].Title,
            Author = _authors[0].Name,
            Publisher = _publishers[0].Name,
            NumAvailable = _books[0].NumAvailable,
            ImgUrl = _books[0].ImgUrl,
        };

        library_manager_server.ClientContext.Book? ret = lm.GetBook(book.Isbn);
        Assert.That(ret, Is.EqualTo(book).UsingPropertiesComparer());
    }

    [Test]
    public void GetBook_Invalid_NoMatchingBook()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        library_manager_server.ClientContext.Book? ret = lm.GetBook("invalidISBN");
        Assert.That(ret, Is.Null);
    }

    [Test]
    public void Search_Invalid_Params()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);

        Assert.Throws<ArgumentException>(() =>
        {
            lm.SearchBooks("1213", -1, 0);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            lm.SearchBooks("1213", 1, -1);
        });
    }

    [Test]
    public void AuthenticateUser_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        PasswordVerificationResult res = lm.AuthenticateUser("email_0", testUserPassword);
        Assert.That(res, Is.EqualTo(PasswordVerificationResult.Success));
    }

    [Test]
    public void AuthenticateUser_Invalid_Password()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        PasswordVerificationResult res = lm.AuthenticateUser("email_0", "invalid_password");
        Assert.That(res, Is.EqualTo(PasswordVerificationResult.Failed));
    }

    [Test]
    public void AuthenticateUser_Invalid_User()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        PasswordVerificationResult res = lm.AuthenticateUser("non existant email", testUserPassword);
        Assert.That(res, Is.EqualTo(PasswordVerificationResult.Failed));
    }

    [Test]
    public void AddUser_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        string testUserEmail = "test@email.com";
        string testUserFirstName = "testFirstName";
        bool ret = lm.AddUser(testUserEmail, testUserPassword, testUserFirstName);
        Assert.That(ret, Is.EqualTo(true));

        LibraryContext context = new LibraryContext(_options.Options);
        User? user = context.Users.FirstOrDefault(u => u.Email == testUserEmail);
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Email, Is.EqualTo(testUserEmail));
        Assert.That(user.Password, Is.Not.EqualTo(new PasswordHasher<string>().HashPassword(testUserEmail, testUserPassword)));
        Assert.That(user.Username, Is.EqualTo(testUserFirstName));
    }

    [Test]
    public void AddUser_Invalid_UserAlreadyExists()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        bool ret = lm.AddUser(_users[0].Email, testUserPassword, _users[0].Username);
        Assert.That(ret, Is.EqualTo(false));
    }

    [Test]
    public void GetUserId_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long? id = lm.GetUserId(_users[0].Email);
        LibraryContext context = new LibraryContext(_options.Options);
        User? u = context.Users.FirstOrDefault(u => u.Email == _users[0].Email);

        Assert.That(id, Is.Not.Null);
        Assert.That(u, Is.Not.Null);
        Assert.That(id, Is.EqualTo(u.UserId));
    }

    [Test]
    public void GetUserId_Invalid_UserDoesNotExist()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long? id = lm.GetUserId("nonExistingEmail");
        Assert.That(id, Is.Null);
    }

    [Test]
    public void GetLoans_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        List<library_manager_server.ClientContext.Loan> loans = lm.GetLoans(1);
        Assert.That(loans.Count, Is.EqualTo(2));
        for (int i = 0; i < loans.Count; i++)
        {
            Assert.That(loans[i].UserId, Is.EqualTo(1));
            Assert.That(loans[i].LoanId, Is.EqualTo(i + 1));
            Assert.That(loans[i].Date, Is.EqualTo(DateOnly.FromDateTime(DateTime.MinValue)));
        }
    }

    [Test]
    public void GetLoans_Invalid_UserDoesNotExist()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        List<library_manager_server.ClientContext.Loan> loans = lm.GetLoans(20);
        Assert.That(loans.Count, Is.LessThanOrEqualTo(0));
    }

    [Test]
    public void GetLoan_Valid()
    {
        long testId = 1;
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        library_manager_server.ClientContext.Loan? loan = lm.GetLoan(testId);
        Assert.That(loan, Is.Not.Null);
        Assert.That(loan.LoanId, Is.EqualTo(1));
        Assert.That(loan.Date, Is.EqualTo(DateOnly.FromDateTime(DateTime.MinValue)));
    }

    [Test]
    public void GetLoan_Invalid_LoanDoesNotExist()
    {
        long NoLoanTestId = 200;
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        library_manager_server.ClientContext.Loan? loan = lm.GetLoan(NoLoanTestId);
        Assert.That(loan, Is.Null);
    }

    [Test]
    public void CreateLoan_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long testUserId = 1;
        string testUserISBN = _books[5].Isbn;
        DateOnly testDate = DateOnly.FromDateTime(DateTime.MinValue);
        library_manager_server.ClientContext.Loan? createdLoan = lm.CreateLoan(testUserISBN, testUserId, testDate);
        Assert.That(createdLoan, Is.Not.Null);
        Assert.That(createdLoan.Book.Isbn, Is.EqualTo(testUserISBN));
        Assert.That(createdLoan.UserId, Is.EqualTo(testUserId));
        Assert.That(createdLoan.Date, Is.EqualTo(testDate));

        LibraryContext context = new LibraryContext(_options.Options);
        Loan? loan = context.Loans
            .Include(e => e.IsbnNavigation)
            .FirstOrDefault(u => u.IsbnNavigation.Isbn == testUserISBN);

        Assert.That(loan, Is.Not.Null);
        Assert.That(createdLoan.Book.Isbn, Is.EqualTo(testUserISBN));
        Assert.That(createdLoan.UserId, Is.EqualTo(testUserId));
        Assert.That(createdLoan.Date, Is.EqualTo(testDate));
        Assert.That(createdLoan.LoanId, Is.EqualTo(loan.LoanId));
        Assert.That(createdLoan.Book.Isbn, Is.EqualTo(loan.Isbn));
        Assert.That(createdLoan.Book.NumAvailable, Is.EqualTo(_books[5].NumAvailable - 1));
    }

    [Test]
    public void CreateLoan_Invalid_NonExistingBook()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long testUserId = 1;
        string testUserISBN = "nonExistingISBN";
        DateOnly testDate = DateOnly.FromDateTime(DateTime.MinValue);
        library_manager_server.ClientContext.Loan? createdLoan = lm.CreateLoan(testUserISBN, testUserId, testDate);
        Assert.That(createdLoan, Is.Null);
    }

    [Test]
    public void CreateLoan_Invalid_NonExistingUser()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        LibraryContext context = new LibraryContext(_options.Options);
        long nonExistingUserId = 999;
        string testIsbn = _books[0].Isbn;
        string testUserISBN = testIsbn;
        Book? book = context.Books.FirstOrDefault(b => b.Isbn == testIsbn);
        Assert.That(book, Is.Not.Null);
        long oldNumAvalible = book.NumAvailable;        
        DateOnly testDate = DateOnly.FromDateTime(DateTime.MinValue);
        library_manager_server.ClientContext.Loan? createdLoan = lm.CreateLoan(testUserISBN, nonExistingUserId, testDate);
        Assert.That(createdLoan, Is.Null);
        Assert.That(book.NumAvailable, Is.EqualTo(oldNumAvalible));
    }

    [Test]
    public void CreateLoan_Invalid_NoAvailableBooks()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long testUserId = 1;
        string testUserISBN = _books[0].Isbn;
        DateOnly testDate = DateOnly.FromDateTime(DateTime.MinValue);
        library_manager_server.ClientContext.Loan? createdLoan = lm.CreateLoan(testUserISBN, testUserId, testDate);
        Assert.That(createdLoan, Is.Null);
    }

    [Test]
    public void OwnsLoan_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long testUserId = 1;
        long testLoanId = _loans[0].LoanId;
        bool ownsLoan = lm.OwnsLoan(testUserId, testLoanId);
        Assert.That(ownsLoan, Is.True);
    }

    [Test]
    public void OwnsLoan_Invalid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long testUserId = 1;
        long invalidLoanId = 999;
        bool ownsLoan = lm.OwnsLoan(testUserId, invalidLoanId);
        Assert.That(ownsLoan, Is.False);
    }

    [Test]
    public void DeleteLoan_Valid()
    {
        LibraryContext context = new LibraryContext(_options.Options);
        Book? book = context.Books.FirstOrDefault(b => b.Isbn == _loans[0].Isbn);
        Assert.That(book, Is.Not.Null);
        long oldCount = book.NumAvailable; 
        Assert.That(oldCount, Is.EqualTo(0));

        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long testLoanId = _loans[0].LoanId;
        bool deleted = lm.DeleteLoan(testLoanId);
        Assert.That(deleted, Is.True);

        Loan? loan = context.Loans.FirstOrDefault(l => l.LoanId == testLoanId);
        Assert.That(loan, Is.Null);

        context.Entry(book).Reload();
        Assert.That(book, Is.Not.Null);
        Assert.That(book.NumAvailable, Is.EqualTo(oldCount + 1));
    }

    [Test]
    public void DeleteLoan_Invalid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        long invalidLoanId = 999;
        bool deleted = lm.DeleteLoan(invalidLoanId);
        Assert.That(deleted, Is.False);
    }

    [Test]
    public void HasActiveLoan_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        string IsbnWithLoan = _books[0].Isbn;
        bool hasActiveLoan = lm.HasActiveLoan(IsbnWithLoan);
        Assert.That(hasActiveLoan, Is.True);
    }

    [Test]
    public void HasActiveLoan_Invalid_NoActiveLoan()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        string IsbnWithoutLoan = "abc";
        bool hasActiveLoan = lm.HasActiveLoan(IsbnWithoutLoan);
        Assert.That(hasActiveLoan, Is.False);
    }
}