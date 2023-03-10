namespace Application.UseCases.Users.Commands.Create;

public record UserRegistrationResponse
{
    public string Token { get; init; }
    public string RefreshToken { get; init; }
}