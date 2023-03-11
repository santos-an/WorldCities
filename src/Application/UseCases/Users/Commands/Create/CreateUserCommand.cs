using Application.Interfaces.Infrastructure;
using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Domain.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Users.Commands.Create;

// returns token
public record CreateUserCommand : ICommand<Result<UserRegistrationResponse>>
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<UserRegistrationResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenGenerator _tokenGenerator;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<UserRegistrationResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.FindByEmailAsync(request.Email);
        if (existingUser.HasValue)
            return Result.Failure<UserRegistrationResponse>("Email already in use. There is a user for the given email. Use a different email");
        
        var user = new IdentityUser { UserName = request.Username, Email = request.Email };
        
        // register user
        var registrationResult = await _unitOfWork.Users.CreateAsync(user, request.Password);
        if (registrationResult.IsFailure)
            return Result.Failure<UserRegistrationResponse>($"Not possible to register user: {registrationResult.Error}");
        
        // add new role to user
        var addNewRoleResult = await _unitOfWork.Users.AddToRoleAsync(user, RoleType.Normal);
        if (addNewRoleResult.IsFailure)
            return Result.Failure<UserRegistrationResponse>($"Not possible to add the role {RoleType.Normal} to the user {request.Email}: {addNewRoleResult.Error}");
        
        // generate access & refresh token
        var newTokenResult = await _tokenGenerator.Generate(user);
        if (newTokenResult.IsFailure)
            return Result.Failure<UserRegistrationResponse>(newTokenResult.Error);
        
        var accessToken = _tokenGenerator.AccessToken;
        var refreshToken = _tokenGenerator.RefreshToken;
        
        await _unitOfWork.Tokens.AddRefreshTokenAsync(refreshToken);
        await _unitOfWork.CommitAsync();
        
        var response = new UserRegistrationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Value,
        };

        return Result.Success(response);
    }
}

