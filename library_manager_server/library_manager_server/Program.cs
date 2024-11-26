using library_manager_server;
using library_manager_server.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using System.Diagnostics.Eventing.Reader;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        var LocalhostHttp = "_LocalhostHttp";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: LocalhostHttp,
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "https://localhost:3000");
                });
        });

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
      
        
        LibrayManager libManager = new LibrayManager(dataSource);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton(libManager);
        builder.Services.AddSingleton<IAuthorizationHandler, SessionAuth>();

        builder.Services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ActiveSession", p =>
                p.Requirements.Add(new ActiveSessionRequirement(libManager))
            );
            //options.AddPolicy("ActiveSession", p => p.RequireClaim(Authentication.SESSION_ID_NAME));
        });



        builder.Logging.AddConsole();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseCors(LocalhostHttp);
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();


        app.Run();
    }
}