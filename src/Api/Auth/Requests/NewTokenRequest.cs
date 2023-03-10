namespace Api.Auth.Requests;

public class NewTokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}