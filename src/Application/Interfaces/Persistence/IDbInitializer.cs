using Domain.Entities;

namespace Application.Interfaces.Persistence;

public interface IDbInitializer
{
    public IReadOnlyList<City> GetCities();
}