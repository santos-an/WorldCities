using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Cities;

public class CityRepository : ICityRepository
{
    private readonly WorldCitiesDbContext _context;

    public CityRepository(WorldCitiesDbContext context) => _context = context;

    public async Task<IEnumerable<City>> GetAll() => await _context.Cities.ToListAsync();

    public async Task<Maybe<City>> GetById(Guid id)
    {
        var city = await _context
            .Cities
            .FirstOrDefaultAsync(c => c.Id == id);

        return Maybe.From<City>(city);
    }

    public async Task<Maybe<City>> GetByGeonameId(string geonameId)
    {
        var city = await _context
            .Cities
            .FirstOrDefaultAsync(c => c.GeonameId == geonameId);

        return Maybe.From<City>(city);
    }

    public Task<Maybe<City>> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<City>> GetByCountry(string country)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<City>> GetBySubCountry(string subCountry)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(City course)
    {
        throw new NotImplementedException();
    }

    public Task Delete(City course)
    {
        throw new NotImplementedException();
    }
}