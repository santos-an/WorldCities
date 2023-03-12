using Application.Interfaces.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Persistence.Users;
using Xunit;

namespace Persistence.Test;

public class UserRepositoryTests
{
    private readonly Mock<UserManager<IdentityUser>> _mockUsers;
    private readonly IUserRepository _repository;
    
    public UserRepositoryTests()
    {
        _mockUsers = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
        _repository = new UserRepository(_mockUsers.Object);
    }

    [Fact]
    public async Task FindByIdAsync_FindsUser_ReturnsExistingUser()
    {
        // Arrange
        _mockUsers.Setup( userManager => userManager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser());
        
        // act
        var actual = await _repository.FindByIdAsync(Guid.NewGuid().ToString());
        
        // assert
        actual.HasValue.Should().Be(true);
    }

    [Fact]
    public async Task FindByIdAsync_DoesNotFindUser_ReturnsExistingUser()
    {
        // Arrange
        _mockUsers.Setup( userManager => userManager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser?)null);
        
        // act
        var actual = await _repository.FindByIdAsync(Guid.NewGuid().ToString());
        
        // assert
        actual.HasValue.Should().Be(false);
        actual.HasNoValue.Should().Be(true);
    }

    [Fact]
    public async Task FindByEmailAsync_FindsUser_ReturnsExistingUser()
    {
        // Arrange
        _mockUsers.Setup( userManager => userManager.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser());
        
        // act
        var actual = await _repository.FindByEmailAsync(Guid.NewGuid().ToString());
        
        // assert
        actual.HasValue.Should().Be(true);
    }

    [Fact]
    public async Task FindByEmailAsync_DoesNotFindUser_ReturnsExistingUser()
    {
        // Arrange
        _mockUsers.Setup( userManager => userManager.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser?)null);
        
        // act
        var actual = await _repository.FindByEmailAsync(Guid.NewGuid().ToString());
        
        // assert
        actual.HasValue.Should().Be(false);
        actual.HasNoValue.Should().Be(true);
    }

    [Fact]
    public async Task IsPasswordCorrectAsync_ValidatesPassword_ReturnsSuccess()
    {
        // Arrange
        const bool expected = true;
        _mockUsers.Setup( userManager => userManager.CheckPasswordAsync( It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(expected);
        
        // act
        var actual = await _repository.IsPasswordCorrectAsync(new IdentityUser(), string.Empty);
        
        // assert
        expected.Should().Be(actual.IsSuccess);
    }

    [Fact]
    public async Task IsPasswordCorrectAsync_DoesNotValidatePassword_ReturnsFailure()
    {
        // arrange
        const bool expected = false;
        _mockUsers.Setup( userManager => userManager.CheckPasswordAsync( It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(expected);
        
        // act
        var actual = await _repository.IsPasswordCorrectAsync(new IdentityUser(), string.Empty);
        
        // assert
        expected.Should().Be(actual.IsSuccess);
    }
}