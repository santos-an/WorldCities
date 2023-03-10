using CSharpFunctionalExtensions;

namespace Application.Interfaces.Infrastructure;

public interface ITokenValidator
{
    Task<Result> ValidateAsync(string token, string refreshToken);
    bool IsExpired(string token);
}