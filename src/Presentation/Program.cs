using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Domain;
using Infrastructure;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Cities;
using Persistence.Database;
using Presentation.Middlewares;

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
        
        RunMigrations(app);
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
        services.AddTransient<ICityRepository, CityRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

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
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        services.AddMediatR(assemblies);
        services.AddFluentValidation(assemblies);
    }

    private static void RunMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WorldCitiesDbContext>();

        context.Database.Migrate();
    }

    private static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddleware<ExceptionMiddleware>();
    }
}