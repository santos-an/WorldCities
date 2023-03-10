using Application.Interfaces.Persistence;
using Persistence.Database;

namespace Persistence.Token;

public class TokenRepository : ITokenRepository
{
    private readonly WorldCitiesDbContext _context;

    public TokenRepository(WorldCitiesDbContext context)
    {
        _context = context;
    }

    public Task GetRefreshTokenBy(string token)
    {
        throw new NotImplementedException();
    }
}