using System.Reflection;
using Application.Interfaces.Persistence;
using Domain.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class WorldCitiesDbContext : IdentityDbContext
{
    private readonly IDbInitializer _initializer;
    
    public virtual DbSet<RefreshToken> RefreshTokens { set; get; }
    public virtual DbSet<City> Cities { set; get; }

    public WorldCitiesDbContext(DbContextOptions<WorldCitiesDbContext> options, IDbInitializer initializer) : base(options)
    {
        _initializer = initializer;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<City>().HasData(_initializer.GetCities());
    }
}