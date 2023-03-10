namespace Application.UseCases.Users.Commands.Login;

public record UserLoginResponse
{
    public string Token { get; init; }
    public string RefreshToken { get; init; }
}