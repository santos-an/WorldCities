using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Tokens;

public class TokenRepository : ITokenRepository
{
    private readonly ApplicationDbContext _context;

    public TokenRepository(ApplicationDbContext context) => _context = context;

    public async Task<Maybe<RefreshToken>> GetRefreshTokenBy(string refreshToken)
    {
        var existingRefreshToken = await _context
            .RefreshTokens
            .FirstOrDefaultAsync(t => t.Value == refreshToken);
        return Maybe.From<RefreshToken>(existingRefreshToken);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }
}