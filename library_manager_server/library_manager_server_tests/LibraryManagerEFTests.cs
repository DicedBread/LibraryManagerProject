using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using library_manager_server;
using library_manager_server.Persistency;

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
        String connectionString = new Npgsql.NpgsqlConnectionStringBuilder()
        {
            Host = TestDbHost,
            Port = TestDbPort,
            Database = TestDbName,
            Username = TestDbUser,
            Password = TestDbPassword,
        }.ConnectionString;
 
        LibraryContext context = new LibraryContext(_options.Options);
        await context.Database.MigrateAsync();
        Console.WriteLine("db migrated");
    }
   
    private List<Authour> _authours = [];
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
            var author = new Authour { Authour1 = $"Authour_{i}" };
            context.Authours.Add(author);
            _authours.Add(author);
        }
        context.SaveChanges();

        for (int i = 0; i < numOfPublishers; i++)
        {
            var publisher = new Publisher { Publisher1 = $"Publisher_{i}" };
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
                AuthourId = (long)Math.Clamp(Math.Round(((numOfBooks / numOfAuthors) * 0.1) * i, 0), 0, numOfAuthors),
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
            string hashedPw = new PasswordHasher<string>().HashPassword(username ,testUserPassword);
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
                Date = new DateOnly(),
            };
            context.Loans.Add(loan);
            _loans.Add(loan);
        }
        context.SaveChanges();
        context.Dispose();
    }

    [TearDown]
    public void Teardown()
    {
        LibraryContext context = new LibraryContext(_options.Options);
        context.Loans.RemoveRange(context.Loans.ToList());
        context.Books.RemoveRange(context.Books.ToList());
        context.Users.RemoveRange(context.Users.ToList());
        context.Authours.RemoveRange(context.Authours.ToList());
        context.Publishers.RemoveRange(context.Publishers.ToList());
        context.SaveChanges();
        context.Dispose();
        
        _loans.Clear();
        _users.Clear();
        _books.Clear();
        _publishers.Clear();
        _authours.Clear();
        Console.WriteLine("db container teardown completed");
    }
    
    [Test]
    public void GetBooks_Valid()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        List<library_manager_server.Model.Book> ret = lm.GetBooks(3, 0);
        
        Assert.That(ret.Count == 3);
        Assert.That(ret[0], 
            Is.EqualTo(
                new library_manager_server.Model.Book
                { Isbn = _books[0].Isbn,
                    Title = _books[0].Title,
                    Authour = _authours[0].Authour1,
                    Publisher = _publishers[0].Publisher1,
                    ImgUrl = _books[0].ImgUrl,
                }
            ).UsingPropertiesComparer()
            );
    }
 
    [Test]
    public void GetBooks_Invalid_Params()
    {
        LibraryManagerEF lm = new LibraryManagerEF(_options.Options);
        Assert.Throws<ArgumentException>(() => { 
            List<library_manager_server.Model.Book> ret = lm.GetBooks(-3, 0);
        });
        
        Assert.Throws<ArgumentException>(() => { 
            List<library_manager_server.Model.Book> ret = lm.GetBooks(0, -1); 
        });
    } 
    
    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        Console.WriteLine("db container teardown completed");
        _postgreSqlContainer.DisposeAsync();
    }


    
    
    
    
    
    
    
}