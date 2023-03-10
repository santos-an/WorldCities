using System.Text;
using Api.Middlewares;
using Application.Behaviours;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Domain;
using Infrastructure;
using Infrastructure.Behaviours;
using Infrastructure.Csv;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Cities;
using Persistence.Database;

namespace Api;

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

        services.AddTransient<ICsvReader, CsvReader>();
        services.AddTransient<IDbInitializer, WorldCitiesDbInitializer>();
        services.AddTransient<ICityRepository, CityRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        var secret = configuration[JwtSecret];
        if (string.IsNullOrEmpty(secret))
            throw new Exception("Secret is null. Please check your app-settings.json");
        
        var key = Encoding.ASCII.GetBytes(secret);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidIssuer = configuration[JwtIssuer],
            ValidAudience = configuration[JwtAudience]
        };
        
        
        services.AddSingleton(validationParameters);
        services.Configure<Jwt>(configuration.GetSection(Jwt));

        
        
        
        
        var csvFileName = configuration[CsvFileName];
        if (string.IsNullOrEmpty(csvFileName))
            throw new Exception("CsvFileName is null. Please check your app-settings.json");

        services.Configure<Csv>(configuration.GetSection(Csv));

        services.AddDbContext<WorldCitiesDbContext>(options =>
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connection);
        });
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehaviour<,>));
        
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