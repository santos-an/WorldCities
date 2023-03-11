using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string UserId { get; }
    public IdentityUser? User { get; private set; }
    public string Value { get; }   // the actual token value   
    public string JwtId { get; }   // the id of the jwt token, that the refresh token belongs to
    public bool IsUsed { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime Created { get; }
    public DateTime ExpiryDate { get; }

    public RefreshToken(string userId, string jwtId, string value, DateTime expiryDate)
    {
        UserId = userId;
        JwtId = jwtId;
        IsUsed = false;
        IsRevoked = false;
        Value = value;
        Created = DateTime.Now;
        ExpiryDate = expiryDate;
    }
    
    public void Revoke()
    {
        IsUsed = true;
        IsRevoked = true;
    }
}