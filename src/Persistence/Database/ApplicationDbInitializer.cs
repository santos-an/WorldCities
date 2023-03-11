using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Bogus;
using Domain.Entities;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Database;

public class ApplicationDbInitializer : IDbInitializer
{
    private readonly ICsvReader _csvReader;

    public ApplicationDbInitializer(ICsvReader csvReader) => _csvReader = csvReader;

    public void GenerateData()
    {
        GenerateCities();
        GetRoles();
        GenerateUsers();
        GenerateUserRoles();
    }

    private void GenerateCities()
    {
        var result = _csvReader.Read();
        if (result.IsFailure)
            throw new CsvReaderException("CSV reading error", result.Error);

        // the csv file does not set an id for the city
        foreach (var city in result.Value) 
            city.UpdateId(Guid.NewGuid());

        Cities = result.Value.ToList();
    }

    private void GetRoles()
    {
        Roles = new List<IdentityRole>()
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleType.Normal,
                NormalizedName = RoleType.Normal.ToUpper()
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleType.Admin,
                NormalizedName = RoleType.Admin.ToUpper()
            }
        };
    }

    private void GenerateUsers()
    {
        IdentityUser user = new();
        const string password = "D;|o)46s__/0$Eb.";
        var hasher = new PasswordHasher<IdentityUser>();

        var userFaker = new Faker<IdentityUser>()
            .RuleFor(t => t.Id, f => Guid.NewGuid().ToString())
            .RuleFor(t => t.UserName, f => f.Person.UserName)
            .RuleFor(t => t.NormalizedUserName, f => f.Person.UserName.ToUpper())
            .RuleFor(t => t.Email, f => f.Person.Email)
            .RuleFor(t => t.NormalizedEmail, f => f.Person.Email.ToLower())
            .RuleFor(t => t.PasswordHash, f => hasher.HashPassword(user, password))
            .RuleFor(t => t.EmailConfirmed, f => f.Random.Bool())
            .RuleFor(t => t.PhoneNumber, f => f.Person.Phone)
            .RuleFor(t => t.PhoneNumberConfirmed, f => f.Random.Bool())
            .RuleFor(t => t.TwoFactorEnabled, f => f.Random.Bool());
        
        Users = userFaker.GenerateBetween(30, 50);
    }

    private void GenerateUserRoles()
    {
        // Some users will be Admin, others will be Standard
        var random = new Random();

        var userRoles = Users
            .Select(user =>
            {
                var role = Roles.ElementAt(random.Next(Roles.Count));
                user.Email = $"{role.Name}_{user.Email}";

                return new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
            })
            .ToList();

        UserRoles = userRoles;
    }

    public IReadOnlyList<City> Cities { get; private set; }
    public IReadOnlyList<IdentityRole> Roles { get; private set; }
    public IReadOnlyList<IdentityUser> Users { get; private set; }
    public IReadOnlyList<IdentityUserRole<string>> UserRoles { get; private set; }
}