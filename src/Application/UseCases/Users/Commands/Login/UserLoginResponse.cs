namespace Application.UseCases.Users.Commands.Login;

public record UserLoginResponse
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}