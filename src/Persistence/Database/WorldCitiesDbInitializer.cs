using Application;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Persistence;
using Domain;
using Domain.Entities;

namespace Persistence.Database;

public class WorldCitiesDbInitializer : IDbInitializer
{
    private readonly IReader _reader;

    public WorldCitiesDbInitializer(IReader reader)
    {
        _reader = reader;
    }

    public IReadOnlyList<City> GetCities()
    {
        var result = _reader.Read();
        if (result.IsFailure)
        {
            
        }
        
        // the csv file does not set an id for the city
        foreach (var city in result.Value) 
            city.Id = Guid.NewGuid();

        return result.Value.ToList();
    }
}