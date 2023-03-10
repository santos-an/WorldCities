using System.Reflection;
using Application.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class WorldCitiesDbContext : IdentityDbContext
{
    private readonly IDbInitializer _initializer;

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

    public virtual DbSet<City> Cities { get; set; }
}