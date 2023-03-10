using Application.Interfaces.Infrastructure;
using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;

namespace Application.UseCases.Users.Commands.Login;

public record LoginUserCommand : ICommand<Result<UserLoginResponse>>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, Result<UserLoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginUserCommandHandler(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<UserLoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.FindByEmailAsync(request.Email);
        if (existingUser.HasNoValue)
            return Result.Failure<UserLoginResponse>("There is no user for the given email");

        var passwordValidation = await _unitOfWork.Users.IsPasswordCorrectAsync(existingUser.Value, request.Password);
        if (passwordValidation.IsFailure)
            return Result.Failure<UserLoginResponse>(passwordValidation.Error);

        var newTokenResult = await _tokenGenerator.Generate(existingUser.Value);
        if (newTokenResult.IsFailure)
            return Result.Failure<UserLoginResponse>("Not possible to generate a new token");
        
        var accessToken = _tokenGenerator.AccessToken;
        var refreshToken = _tokenGenerator.RefreshToken;
        
        await _unitOfWork.Tokens.AddRefreshTokenAsync(refreshToken);
        await _unitOfWork.CommitAsync();
        
        var response = new UserLoginResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken.Value,
        };

        return Result.Success(response);
    }
}