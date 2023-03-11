using System.Reflection;
using Application.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly IDbInitializer _initializer;
    
    public virtual DbSet<RefreshToken> RefreshTokens { set; get; }
    public virtual DbSet<City> Cities { set; get; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDbInitializer initializer) : base(options) => _initializer = initializer;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        _initializer.GenerateData();
        
        // populate db
        modelBuilder.Entity<City>().HasData(_initializer.Cities);
        modelBuilder.Entity<IdentityRole>().HasData(_initializer.Roles);
        modelBuilder.Entity<IdentityUser>().HasData(_initializer.Users);
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(_initializer.UserRoles);
    }
}