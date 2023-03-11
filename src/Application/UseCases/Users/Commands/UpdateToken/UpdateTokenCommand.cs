using Application.Interfaces.Infrastructure;
using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;

namespace Application.UseCases.Users.Commands.UpdateToken;

public record UpdateTokenCommand : ICommand<Result<UpdateTokenResponse>>
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}

public class UpdateTokenCommandHandler : ICommandHandler<UpdateTokenCommand, Result<UpdateTokenResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ITokenValidator _tokenValidator;

    public UpdateTokenCommandHandler(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, ITokenValidator tokenValidator)
    {
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
        _tokenValidator = tokenValidator;
    }

    public async Task<Result<UpdateTokenResponse>> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
    {
        // Validation of access & refresh token
        var tokenValidationResult = await _tokenValidator.ValidateAsync(request.AccessToken, request.RefreshToken);
        if (tokenValidationResult.IsFailure)
        {
            // Lifetime of access token needs to be invalid, in order to be renewed
            var lifetimeFailed = tokenValidationResult.Error.ToLower().Contains("lifetime validation failed");
            if (!lifetimeFailed)
                return Result.Failure<UpdateTokenResponse>($"Tokens are invalid! You cannot use this token to generate a new one");
        }
        
        // Expired
        var isTokenExpired = _tokenValidator.IsExpired(request.AccessToken);
        if (!isTokenExpired)
            return Result.Failure<UpdateTokenResponse>($"You are trying to re-new a token that is still valid. Token can not be renewed because is not expired. Only expired tokens can be renewed");
        
        // against the database
        var existingRefreshTokenOnDb = await _unitOfWork.Tokens.GetRefreshTokenBy(request.RefreshToken);
        if (existingRefreshTokenOnDb.HasNoValue)
            return Result.Failure<UpdateTokenResponse>("Refresh token is not present in the database.");

        var refreshToken = existingRefreshTokenOnDb.Value;
        refreshToken.Revoke();
        
        // get the user that is referenced on the token
        var existingUser = await _unitOfWork.Users.FindByIdAsync(refreshToken.UserId);
        if (existingUser.HasNoValue)
            return Result.Failure<UpdateTokenResponse>("There is no user in the database, for the given token.");
        
        // create a new access token & refresh token
        var newTokenResult = await _tokenGenerator.Generate(existingUser.Value);
        if (newTokenResult.IsFailure)
            return Result.Failure<UpdateTokenResponse>($"Not possible to generate a new token");
        
        var newAccessToken = _tokenGenerator.AccessToken;
        var newRefreshToken = _tokenGenerator.RefreshToken;
        
        await _unitOfWork.Tokens.AddRefreshTokenAsync(newRefreshToken);
        await _unitOfWork.CommitAsync();

        var response = new UpdateTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Value
        };

        return Result.Success(response);
    }
}