using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Users;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Maybe<IdentityUser>> FindByEmailAsync(string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        return Maybe.From<IdentityUser>(existingUser);
    }

    public async Task<Result> CreateAsync(IdentityUser user, string password)
    {
        var commandResult = await _userManager.CreateAsync(user, password);

        if (commandResult.Succeeded)
            return Result.Success();

        var errors = commandResult.Errors.Select(e => e.Description);
        return Result.Failure(string.Join(",", errors.ToArray() ));
    }

    public async Task<Result> AddToRoleAsync(IdentityUser user, string role)
    {
        Result result = default;
        
        try
        {
            var commandResult = await _userManager.AddToRoleAsync(user, role);
            if (commandResult.Succeeded)
                result = Result.Success();
        }
        catch (Exception e)
        {
            var errors = e.Message;
            result = Result.Failure(errors);
        }

        return result;
    }
}