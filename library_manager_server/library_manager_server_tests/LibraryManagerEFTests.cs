using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using library_manager_server;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace library_manager_server_tests;

[TestFixture]
public class LibraryManagerEFTests
{
    const string testDBName = "library";
    const string testDBUser = "postgres";
    const string testDBPassword = "postgres";
    const int testDBPort = 5432;

    // #pragma warning disable NUnit1032
    private PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16.2")
        .WithName("testDB")
        .WithDatabase(testDBName)
        .WithPassword(testDBPassword)
        .WithPortBinding(testDBPort, testDBPort)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();
    // #pragma warning restore NUnit1032

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
            Host = "localhost",
            Port = testDBPort,
            Database = testDBName,
            Username = testDBUser,
            Password = testDBPassword,
        };
        
        DbContextOptionsBuilder<LibraryContext> options = new DbContextOptionsBuilder<LibraryContext>();
        options.UseNpgsql(conStrB.ConnectionString);
        
        LibraryContext context = new LibraryContext(options.Options);
        await context.Database.MigrateAsync();
        Console.WriteLine("db migrated");

    }
    
    
    
    [SetUp]
    public void Setup()
    {
        // drop all data
        // reinsert data
    }

    [Test]
    public void test()
    {
        Assert.Fail();
    }
    
    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        _postgreSqlContainer.DisposeAsync();
    }
    
    
    
    
    
    
}