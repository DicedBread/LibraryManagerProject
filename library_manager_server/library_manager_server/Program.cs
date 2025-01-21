using library_manager_server;
using library_manager_server.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Npgsql;
using System.Diagnostics.Eventing.Reader;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder => {
                builder.SetIsOriginAllowed(origin =>
                    {
                        Console.WriteLine("uri " + new Uri(origin).Host);
                        return new Uri(origin).Host == "localhost";
                        
                    })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        string? user = builder.Configuration["library:dbUser"];
        string? pass = builder.Configuration["library:dbPassword"];
        string? address = builder.Configuration["library:dbAddress"];
        string? port = builder.Configuration["library:dbPort"];
        string? database = builder.Configuration["library:database"];
        
        if (user == null || pass == null || address == null || port == null)
        {
            Console.WriteLine("missing database info");
            return;
        }

        var conStrB = new Npgsql.NpgsqlConnectionStringBuilder()
        {
            Host = address,
            Port = int.Parse(port),
            Database = database,
            Username = user,
            Password = pass,
        };

        using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(conStrB.ConnectionString);
        

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton(dataSource);
        builder.Services.AddSingleton<ILibraryManager, LibraryManager>();
        builder.Services.AddSingleton<ISessionHandler, SessionHandler>();
        builder.Services.AddSingleton<IAuthorizationHandler, SessionAuthorizationHandler>();
        builder.Services.AddLogging();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            // prevent rederect to login/AccessDenied and just returns error codes
            options.Events = new CookieAuthenticationEvents()
            {
                OnRedirectToLogin = (ctx) =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                    {
                        ctx.Response.StatusCode = 401;
                    }
                    return Task.CompletedTask;
                },
                OnRedirectToAccessDenied = (ctx) =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                    {
                        ctx.Response.StatusCode = 403;
                    }
                    return Task.CompletedTask;
                }
            };
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            // options.Cookie.HttpOnly = false;
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ActiveSession", p =>
                p.Requirements.Add(new ActiveSessionRequirement())
            );
        });


        builder.Logging.AddConsole();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseCors();
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