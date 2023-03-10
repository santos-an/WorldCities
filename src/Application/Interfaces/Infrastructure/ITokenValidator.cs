using CSharpFunctionalExtensions;

namespace Application.Interfaces.Infrastructure;

public interface ITokenValidator
{
    Task<Result> ValidateAsync(string accessToken, string refreshToken);
    bool IsExpired(string token);
}