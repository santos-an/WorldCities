using Application.Interfaces.Persistence;
using Domain.Tokens;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Persistence.Database;
using Persistence.Tokens;
using Xunit;

namespace Persistence.Test;

public class TokenRepositoryTests
{
    private readonly Mock<DbSet<RefreshToken>> _dbSetMock;
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly ITokenRepository _repository;
    
    public TokenRepositoryTests()
    {
        _dbSetMock = MockDbSet();
        _contextMock = new Mock<ApplicationDbContext>();
        _repository = new TokenRepository(_contextMock.Object);
    }

    [Fact]
    public async Task GetRefreshTokenBy_FindsToken_ReturnsExistingToken()
    {
        // arrange
        var refreshTokenValue = RefreshTokens().ElementAt(0).Value;
        _contextMock.Setup(c => c.RefreshTokens).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetRefreshTokenBy(refreshTokenValue);

        // assert
        actual.HasValue.Should().Be(true);
        actual.Value.Value.Should().Be(refreshTokenValue);
    }

    [Fact]
    public async Task GetRefreshTokenBy_DoesNotFindToken_ReturnsNone()
    {
        // arrange
        const string randomRefreshToken = "some_random_refresh_token";
        _contextMock.Setup(c => c.RefreshTokens).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetRefreshTokenBy(randomRefreshToken);

        // assert
        actual.HasValue.Should().Be(false);
        actual.HasNoValue.Should().Be(true);
    }
    
    private Mock<DbSet<RefreshToken>> MockDbSet()
    {
        return RefreshTokens()
            .AsQueryable()
            .BuildMockDbSet();
    }
    
    private static IEnumerable<RefreshToken> RefreshTokens()
    {
        return new List<RefreshToken>()
        {
            new("userID1", "jwt1", "value1", DateTime.Now),
            new("userID2", "jwt2", "value2", DateTime.Now)
        };
    }
}