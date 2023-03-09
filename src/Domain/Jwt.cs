namespace Domain;

public class Jwt
{
    public string Secret { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public int TokenExpiration { get; init; }
    public int RefreshTokenExpiration { get; init; } 
}