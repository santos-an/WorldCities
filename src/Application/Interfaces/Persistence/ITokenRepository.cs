using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Application.Interfaces.Persistence;

public interface ITokenRepository
{
    Task<Maybe<RefreshToken>> GetRefreshTokenBy(string refreshToken);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
}