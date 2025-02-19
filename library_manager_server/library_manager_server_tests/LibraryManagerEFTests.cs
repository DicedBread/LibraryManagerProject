using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using library_manager_server;
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
        var conStrB = new Npgsql.NpgsqlConnectionStringBuilder()
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
    
    
    
    [SetUp]
    public void Setup()
    {
        new LibraryContext(_options).Books.Add(new Book
        {
            Isbn = null,
            Title = null,
            ImgUrl = null,
            AuthourId = 0,
            PublisherId = 0,
            TextSearch = null,
            Authour = null,
            Loans = null,
            Publisher = null
        });

        // drop all data
        // reinsert data
    }

    [Test]
    public void test()
    {
        Assert.Fail();
    }
    
    [Test]
    public void test2()
    {
        Assert.Fail();
    }
    
    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        Console.WriteLine("db container teardown completed");
        _postgreSqlContainer.DisposeAsync();
    }


    
    
    
    
    
    
    
}