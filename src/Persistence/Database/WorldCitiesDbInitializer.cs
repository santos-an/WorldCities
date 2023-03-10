using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Domain;
using Domain.Entities;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Database;

public class WorldCitiesDbInitializer : IDbInitializer
{
    private readonly ICsvReader _csvReader;

    public WorldCitiesDbInitializer(ICsvReader csvReader)
    {
        _csvReader = csvReader;
    }

    public IReadOnlyList<City> GetCities()
    {
        var result = _csvReader.Read();
        if (result.IsFailure)
            throw new CsvReaderException("CSV reading error", result.Error);

        // the csv file does not set an id for the city
        foreach (var city in result.Value) 
            city.Id = Guid.NewGuid();

        return result.Value.ToList();
    }

    public IReadOnlyList<IdentityRole> GetRoles()
    {
        return new List<IdentityRole>()
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
}