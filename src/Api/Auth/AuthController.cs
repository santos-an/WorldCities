using Api.Auth.Requests;
using Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ITokenValidator _validator;

    public AuthController(ITokenGenerator tokenGenerator, ITokenValidator validator)
    {
        _tokenGenerator = tokenGenerator;
        _validator = validator;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(UserRegistrationRequest request)
    {
        var user = new IdentityUser { UserName = request.Username, Email = request.Email };
        var tokenResult = await _tokenGenerator.Generate(user);
        
        if (tokenResult.IsFailure)
        {
            return BadRequest(new 
            {
                Success = false,
                Errors = new List<string> { "Not possible to generate a new token" }
            });
        }
        
        var token = _tokenGenerator.AccessToken;
        var refreshToken = _tokenGenerator.RefreshToken;
        var response = new 
        {
            Token = token,
            RefreshToken = refreshToken.Value,
            Success = true
        };
        
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateToken(NewTokenRequest request)
    {
        throw new NotImplementedException();
    }
}