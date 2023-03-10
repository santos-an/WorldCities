namespace Application.UseCases.Users.Commands.Create;

public record UserRegistrationResponse
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}