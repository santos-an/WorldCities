using Api.Auth;
using Api.Auth.Requests;
using Application.UseCases.Users.Commands.Create;
using Application.UseCases.Users.Commands.Login;
using Application.UseCases.Users.Commands.UpdateToken;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Test;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly AuthController _controller;
    
    public AuthControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new AuthController(_mediator.Object);
    }

    [Fact]
    public async Task Register_Succeeds_ReturnsOkResponse()
    {
        // arrange
        var response = new UserRegistrationResponse { AccessToken = string.Empty, RefreshToken = string.Empty };
        var expected = Result.Success(response);

        _mediator.Setup(m => 
                m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.Register(new UserRegistrationRequest());
        var actual = (result as OkObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        actual.Should().NotBeNull();
        actual.Should().BeOfType<UserRegistrationResponse>();
    }
    
    [Fact]
    public async Task Register_Fails_ReturnsBadRequestResponse()
    {
        // arrange
        var expected = Result.Failure<UserRegistrationResponse>("some error");

        _mediator.Setup(m => 
                m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.Register(new UserRegistrationRequest());
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();
        actual.Should().Be(expected.Error);
    }

    [Fact]
    public async Task Login_Succeeds_ReturnsOkResponse()
    {
        // arrange
        var response = new UserLoginResponse { AccessToken = string.Empty, RefreshToken = string.Empty };
        var expected = Result.Success(response);
        
        _mediator.Setup(m => 
                m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _controller.Login(new UserLoginRequest());
        var actual = (result as OkObjectResult).Value;
        
        // assert
        _mediator.Verify(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        actual.Should().NotBeNull();
        actual.Should().BeOfType<UserLoginResponse>();
    }
    
    [Fact]
    public async Task Login_Fails_ReturnsBadRequestResponse()
    {
        // arrange
        var expected = Result.Failure<UserLoginResponse>("some error");
        
        _mediator.Setup(m => 
                m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _controller.Login(new UserLoginRequest());
        var actual = (result as BadRequestObjectResult).Value;
        
        // assert
        _mediator.Verify(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();
        actual.Should().Be(expected.Error);
    }

    [Fact]
    public async Task UpdateToken_Succeeds_ReturnsOkResponse()
    {
        // arrange
        var response = new UpdateTokenResponse() { AccessToken = string.Empty, RefreshToken = string.Empty };
        var expected = Result.Success(response);
        
        _mediator.Setup(m => 
                m.Send(It.IsAny<UpdateTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _controller.UpdateToken(new NewTokenRequest());
        var actual = (result as OkObjectResult).Value;
        
        // assert
        _mediator.Verify(m => m.Send(It.IsAny<UpdateTokenCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        actual.Should().NotBeNull();
        actual.Should().BeOfType<UpdateTokenResponse>();
    }
    
    [Fact]
    public async Task UpdateToken_Fails_ReturnsBadRequestResponse()
    {
        // arrange
        var expected = Result.Failure<UpdateTokenResponse>("some error");
        
        _mediator.Setup(m => 
                m.Send(It.IsAny<UpdateTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        // act
        var result = await _controller.UpdateToken(new NewTokenRequest());
        var actual = (result as BadRequestObjectResult).Value;
        
        // assert
        _mediator.Verify(m => m.Send(It.IsAny<UpdateTokenCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();
        actual.Should().Be(expected.Error);
    }
}