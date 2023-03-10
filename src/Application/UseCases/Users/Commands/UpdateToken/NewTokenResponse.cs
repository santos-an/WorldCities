namespace Application.UseCases.Users.Commands.UpdateToken;

public class NewTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}