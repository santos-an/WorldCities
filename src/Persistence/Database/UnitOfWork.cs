using Application.Interfaces.Persistence;

namespace Persistence.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context, ICityRepository cityRepository, ITokenRepository tokenRepository, IUserRepository userRepository)
    {
        _context = context;
        
        Cities = cityRepository;
        Tokens = tokenRepository;
        Users = userRepository;
    }

    public ICityRepository Cities { get; }
    public ITokenRepository Tokens { get; }
    public IUserRepository Users { get; }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
}