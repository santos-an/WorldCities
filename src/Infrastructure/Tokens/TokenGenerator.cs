using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Infrastructure;
using CSharpFunctionalExtensions;
using Domain.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Tokens;

public class TokenGenerator : ITokenGenerator
{
    private readonly JwtOptions _jwt;
    private readonly JwtSecurityTokenHandler _handler;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public TokenGenerator(
        IOptionsMonitor<JwtOptions> options,
        JwtSecurityTokenHandler handler,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _jwt = options.CurrentValue;
        _handler = handler;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> Generate(IdentityUser user)
    {
        var key = Encoding.ASCII.GetBytes(_jwt.Secret);
        var claims = await GetAllValidClaimsFor(user);
        var mySecurityKey = new SymmetricSecurityKey(key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwt.Issuer,
            Audience = _jwt.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpiration),
            SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken = _handler.CreateToken(tokenDescriptor);
        AccessToken = _handler.WriteToken(SecurityToken);
        RefreshToken = new RefreshToken(user.Id, SecurityToken.Id, RandomString(35) + Guid.NewGuid(), DateTime.UtcNow.AddHours(_jwt.RefreshTokenExpiration));

        return Result.Success();
    }

    private async Task<List<Claim>> GetAllValidClaimsFor(IdentityUser user)
    {
        var claims = new List<Claim>()
        {
            new("id", user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // id to be used for the refresh token
        };
        
        // getting the claims from DB
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        
        // get the USER_ROLES, from the DB
        var userRoles = await _userManager.GetRolesAsync(user);
        
        // attach the roles from the DB
        foreach (var userRole in userRoles)
        {
            var existingRole = await _roleManager.FindByNameAsync(userRole);
            if (existingRole is null) 
                continue;
            
            // add the ROLE
            claims.Add(new Claim(ClaimTypes.Role, userRole));
                
            // get all claims from the ROLE
            var existingRoleClaims = await _roleManager.GetClaimsAsync(existingRole);
            claims.AddRange(existingRoleClaims);
        }
        
        return claims;
    }

    private static string RandomString(int length)
    {
        var random = new Random();
        var builder = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var randValue = random.Next(0, 26);
            var letter = Convert.ToChar(randValue + 65);

            builder.Append(letter);
        }

        return builder.ToString();
    }

    public SecurityToken SecurityToken { get; private set; }
    public string AccessToken { get; private set; }
    public RefreshToken RefreshToken { get; private set; }
}