using CSharpFunctionalExtensions;
using Domain;
using Domain.Entities;

namespace Application.Interfaces.Persistence;

public interface ICityRepository
{
    public Task<IEnumerable<City>> GetAll();
    public Task<Maybe<City>> GetById(Guid id);
    public Task<Maybe<City>> GetByGeonameId(string geonameId);
    
    public Task<Maybe<City>> GetByName(string name);
    public Task<Maybe<City>> GetByCountry(string country);
    public Task<Maybe<City>> GetBySubCountry(string subCountry);

    public Task AddAsync(City course);
    public Task Delete(City course);
}