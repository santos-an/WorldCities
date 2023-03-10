using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Domain.Authentication;
using Persistence.Database;

namespace Persistence.Token;

public class TokenRepository : ITokenRepository
{
    private readonly WorldCitiesDbContext _context;

    public TokenRepository(WorldCitiesDbContext context) => _context = context;

    public async Task<Maybe<RefreshToken>> GetRefreshTokenBy(string refreshToken)
    {
        var existingRefreshToken = await _context
            .RefreshTokens
            .FirstOrDefaultAsync(t => t.Value == refreshToken);
        return Maybe.From<RefreshToken>(existingRefreshToken);
    }
}