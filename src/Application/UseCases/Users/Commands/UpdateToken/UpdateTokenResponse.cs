namespace Application.UseCases.Users.Commands.UpdateToken;

public class UpdateTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}