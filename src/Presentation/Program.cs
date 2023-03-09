using Application;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Presentation;

public static class Program
{
    private const string Jwt = nameof(Jwt);
    private const string JwtSecret = $"{Jwt}:Secret";
    private const string JwtIssuer = $"{Jwt}:Issuer";
    private const string JwtAudience = $"{Jwt}:Audience";
    
    private const string Csv = nameof(Csv);
    private const string CsvFileName = $"{Csv}:FileName";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        Configure(app);
        
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddTransient<IReader, CsvReader>();
        services.AddTransient<IDbInitializer, WorldCitiesDbInitializer>();
        
        var secret = configuration[JwtSecret];
        if (string.IsNullOrEmpty(secret))
            throw new Exception("Secret is null. Please check your app-settings.json");

        var csvFileName = configuration[CsvFileName];
        if (string.IsNullOrEmpty(csvFileName))
            throw new Exception("CsvFileName is null. Please check your app-settings.json");

        services.Configure<Csv>(configuration.GetSection(Csv));

        services.AddDbContext<WorldCitiesDbContext>(options =>
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connection);
        });
    }

    private static void Configure(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        
        RunMigrations(app);
    }

    private static void RunMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WorldCitiesDbContext>();

        context.Database.Migrate();
    }
}