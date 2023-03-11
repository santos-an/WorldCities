using CSharpFunctionalExtensions;
using Domain.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces.Infrastructure;

public interface ITokenGenerator
{
    Task<Result> Generate(IdentityUser user);
    
    SecurityToken SecurityToken { get; }
    string AccessToken { get; }
    RefreshToken RefreshToken { get; }
}