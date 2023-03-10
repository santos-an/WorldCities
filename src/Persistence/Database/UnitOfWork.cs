using Application.Interfaces.Persistence;

namespace Persistence.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly WorldCitiesDbContext _context;

    public UnitOfWork(WorldCitiesDbContext context, ICityRepository cityRepository, ITokenRepository tokenRepository)
    {
        _context = context;
        
        Cities = cityRepository;
        Tokens = tokenRepository;
    }

    public ICityRepository Cities { get; }
    public ITokenRepository Tokens { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
}