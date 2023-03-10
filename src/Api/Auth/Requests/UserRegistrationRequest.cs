namespace Api.Auth.Requests;

public record UserRegistrationRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}