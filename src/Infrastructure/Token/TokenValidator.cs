using Application.Interfaces.Infrastructure;
using CSharpFunctionalExtensions;

namespace Infrastructure.Token;

public class TokenValidator : ITokenValidator
{
    public Task<Result> ValidateAsync(string token, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public bool IsExpired(string token)
    {
        throw new NotImplementedException();
    }
}