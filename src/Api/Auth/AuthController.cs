using Api.Auth.Requests;
using Application.Interfaces.Infrastructure;
using Application.UseCases.Users.Commands.Create;
using Application.UseCases.Users.Commands.Login;
using Application.UseCases.Users.Commands.UpdateToken;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(UserRegistrationRequest request)
    {
        var command = new CreateUserCommand
            { Username = request.Username, Email = request.Email, Password = request.Password };

        var response = await _mediator.Send(command);
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var command = new LoginUserCommand { Email = request.Email, Password = request.Password };

        var response = await _mediator.Send(command);
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateToken(NewTokenRequest request)
    {
        var command = new NewTokenCommand { AccessToken = request.AccessToken, RefreshToken = request.RefreshToken };

        var response = await _mediator.Send(command);
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }
}