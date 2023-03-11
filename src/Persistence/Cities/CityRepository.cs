using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Cities;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context) => _context = context;

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
            .FirstOrDefaultAsync(c => c.GeoNameId == geonameId);

        return Maybe.From<City>(city);
    }

    public async Task<Maybe<IEnumerable<City>>> GetByName(string name)
    {
        IQueryable<City> cities = _context
            .Cities
            .Where(c => c.Name.ToLower().Contains(name.ToLower()));

        var result = await cities.ToListAsync();
        return Maybe.From<IEnumerable<City>>(result);
    }

    public async Task<Maybe<IEnumerable<City>>> GetByCountry(string country)
    {
        IQueryable<City> cities = _context
            .Cities
            .Where(c => c.Country.ToLower().Contains(country.ToLower()));

        var result = await cities.ToListAsync();
        return Maybe.From<IEnumerable<City>>(result);
    }

    public async Task<Maybe<IEnumerable<City>>> GetBySubCountry(string subCountry)
    {
        IQueryable<City> cities = _context
            .Cities
            .Where(c => c.SubCountry.ToLower().Contains(subCountry.ToLower()));

        var result = await cities.ToListAsync();
        return Maybe.From<IEnumerable<City>>(result);
    }

    public async Task AddAsync(City city)
    {
        await _context
            .Cities
            .AddAsync(city);
    }

    public Task Delete(City course)
    {
        throw new NotImplementedException();
    }
}