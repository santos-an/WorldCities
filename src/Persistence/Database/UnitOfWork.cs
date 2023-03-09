using Application.Interfaces.Persistence;

namespace Persistence.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly WorldCitiesDbContext _context;

    public UnitOfWork(WorldCitiesDbContext context, ICityRepository cityRepository)
    {
        _context = context;
        
        Cities = cityRepository;
    }

    public ICityRepository Cities { get; }

    public async Task CommitAsync() => await _context.SaveChangesAsync();
}