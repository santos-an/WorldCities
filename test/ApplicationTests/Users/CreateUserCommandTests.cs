using Application.Interfaces.Infrastructure;
using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Users.Commands.Create;
using CSharpFunctionalExtensions;
using Domain.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace ApplicationTests.Users;

public class CreateUserCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ITokenGenerator> _tokenGenerator;
    private readonly ICommandHandler<CreateUserCommand, Result<UserRegistrationResponse>> _handler;

    public CreateUserCommandTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _tokenGenerator = new Mock<ITokenGenerator>();
        
        _handler = new CreateUserCommandHandler(_unitOfWork.Object, _tokenGenerator.Object);
    }

    [Fact]
    public async Task Handle_FindsEmailInTheDatabase_ReturnsFailure()
    {
       // arrange
       var command = new CreateUserCommand();
       var existingUser = Maybe.From(new IdentityUser());

       _unitOfWork.Setup(u => u.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);
       
       // act
       var result = await _handler.Handle(command, CancellationToken.None);

       // assert
       result.IsSuccess.Should().Be(false);
       result.IsFailure.Should().Be(true);

       _unitOfWork.Verify(u => u.Users.FindByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NotPossibleToCreateUser_ReturnsFailure()
    {
        // arrange
        var command = new CreateUserCommand();
        var notExistingUser = Maybe<IdentityUser>.From(null);

        _unitOfWork.Setup(u => u.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(notExistingUser);
        _unitOfWork.Setup(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure("some error"));
        
        // act
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(false);
        result.IsFailure.Should().Be(true);

        _unitOfWork.Verify(u => u.Users.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _unitOfWork.Verify(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NotPossibleToAddToRoleUser_ReturnsFailure()
    {
        // arrange
        var command = new CreateUserCommand();
        var notExistingUser = Maybe<IdentityUser>.From(null);

        _unitOfWork.Setup(u => u.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(notExistingUser);
        _unitOfWork.Setup(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success());
        _unitOfWork.Setup(u => u.Users.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure("some error"));
        
        // act
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(false);
        result.IsFailure.Should().Be(true);

        _unitOfWork.Verify(u => u.Users.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _unitOfWork.Verify(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        _unitOfWork.Verify(u => u.Users.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NotPossibleToGenerateToken_ReturnsFailure()
    {
        // arrange
        var command = new CreateUserCommand();
        var notExistingUser = Maybe<IdentityUser>.From(null);

        _unitOfWork.Setup(u => u.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(notExistingUser);
        _unitOfWork.Setup(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success());
        _unitOfWork.Setup(u => u.Users.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success);
        _tokenGenerator.Setup(t => t.GenerateAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(Result.Failure("some error"));
        
        // act
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(false);
        result.IsFailure.Should().Be(true);

        _unitOfWork.Verify(u => u.Users.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _unitOfWork.Verify(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        _unitOfWork.Verify(u => u.Users.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        _tokenGenerator.Verify(t => t.GenerateAsync(It.IsAny<IdentityUser>()), Times.Once);
    }

    [Fact]
    public async Task Handle_StoresUserAndToken_ReturnsSuccess()
    {
        // arrange
        var command = new CreateUserCommand();
        var notExistingUser = Maybe<IdentityUser>.From(null);

        _unitOfWork.Setup(u => u.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(notExistingUser);
        _unitOfWork.Setup(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success());
        _unitOfWork.Setup(u => u.Users.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success);
        _tokenGenerator.Setup(t => t.GenerateAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(Result.Success());
        _unitOfWork.Setup(u => u.Tokens.AddRefreshTokenAsync(It.IsAny<RefreshToken>()));
        _unitOfWork.Setup(u => u.CommitAsync());

        var accessToken = string.Empty;
        var refreshToken = new RefreshToken(string.Empty, string.Empty, string.Empty, DateTime.Now);

        _tokenGenerator.Setup(t => t.AccessToken).Returns(accessToken);
        _tokenGenerator.Setup(t => t.RefreshToken).Returns(refreshToken);
        
        // act
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(true);
        result.IsFailure.Should().Be(false);

        _unitOfWork.Verify(u => u.Users.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _unitOfWork.Verify(u => u.Users.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        _unitOfWork.Verify(u => u.Users.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        
        _tokenGenerator.Verify(t => t.GenerateAsync(It.IsAny<IdentityUser>()), Times.Once);
        
        _unitOfWork.Verify(u => u.Tokens.AddRefreshTokenAsync(It.IsAny<RefreshToken>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
}