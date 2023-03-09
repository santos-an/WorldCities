using Domain;

namespace Persistence.Database;

public interface IDbInitializer
{
    public IReadOnlyList<City> GetCities();
}