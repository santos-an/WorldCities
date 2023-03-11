using CSharpFunctionalExtensions;
using Domain.Cities;

namespace Application.Interfaces.Persistence;

public interface ICityRepository
{
    public Task<IEnumerable<City>> GetAll();
    public Task<Maybe<City>> GetById(Guid id);
    public Task<Maybe<City>> GetByGeonameId(string geonameId);
    
    public Task<Maybe<IEnumerable<City>>> GetByName(string name);
    public Task<Maybe<IEnumerable<City>>> GetByCountry(string country);
    public Task<Maybe<IEnumerable<City>>> GetBySubCountry(string subCountry);

    public Task AddAsync(City course);
    public Task Delete(City course);
}