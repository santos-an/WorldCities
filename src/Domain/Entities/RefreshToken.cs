using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    
    public string UserId { get; set; }
    public IdentityUser? User { get; set; }
    
    public string Value { get; set; }   // the actual token value   
    public string JwtId { get; set; }   // the id of the jwt token, that the refresh token belongs to
    
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime ExpiryDate { get; set; }
}