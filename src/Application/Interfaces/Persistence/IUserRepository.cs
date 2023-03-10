using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Persistence;

public interface IUserRepository
{
    Task<Maybe<IdentityUser>> FindByEmailAsync(string email);
    Task<Result> CreateAsync(IdentityUser user, string password);

    Task<Result> AddToRoleAsync(IdentityUser user, string role);
}