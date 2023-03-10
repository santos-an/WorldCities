namespace Api.Auth.Requests;

public class NewTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}