using Npgsql;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        string? user = builder.Configuration["library:testuser"];
        string? pass = builder.Configuration["library:testPassword"];
        string? address = builder.Configuration["library:address"];
        string? port = builder.Configuration["library:port"];
        string? database = builder.Configuration["library:database"];

        if (user == null || pass == null || address == null || port == null)
        {
            Console.WriteLine("missing database info");
        }

        var conStrB = new Npgsql.NpgsqlConnectionStringBuilder()
        {
            Host = address,
            Port = int.Parse(port),
            Database = database,
            Username = user,
            Password = pass,
        };

        using var dataSource = NpgsqlDataSource.Create(conStrB.ConnectionString);
        LibrayManager lm = new LibrayManager(dataSource);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton(lm);
        //builder.Services.AddTransient<LibrayManager, LibrayManager>();

        builder.Logging.AddConsole();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();


        app.Run();
    }
}