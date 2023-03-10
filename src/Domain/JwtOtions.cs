namespace Domain;

public class JwtOtions
{
    public string Secret { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public int AccessTokenExpiration { get; init; }   // in minutes
    public int RefreshTokenExpiration { get; init; }    // in hours
}