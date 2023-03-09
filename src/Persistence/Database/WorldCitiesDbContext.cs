using System.Reflection;
using Application.Interfaces.Persistence;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class WorldCitiesDbContext : DbContext
{
    private readonly IDbInitializer _initializer;

    public WorldCitiesDbContext(DbContextOptions<WorldCitiesDbContext> options, IDbInitializer initializer) : base(options)
    {
        _initializer = initializer;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<City>().HasData(_initializer.GetCities());
    }

    public virtual DbSet<City> Cities { get; set; }
}