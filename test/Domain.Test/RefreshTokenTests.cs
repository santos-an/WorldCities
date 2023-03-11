using System.Text;
using Domain.Tokens;
using FluentAssertions;
using Xunit;

namespace Domain.Test;

public class RefreshTokenTests
{
    private readonly RefreshToken _refreshToken;

    public RefreshTokenTests()
    {
        var userId = Guid.NewGuid().ToString();
        var jwtId = Guid.NewGuid().ToString();
        var value = RandomValue(30);
        var expiryDate = DateTime.Now.AddDays(1);
        
        _refreshToken = new RefreshToken(userId, jwtId, value, expiryDate);
    }

    [Fact]
    public void Revoke_UpdatesEntity()
    {
        // arrange
        const bool expected = true;

        // act
        _refreshToken.Revoke();

        // assert
        expected.Should().Be(_refreshToken.IsUsed);
        expected.Should().Be(_refreshToken.IsRevoked);
    } 

    private string RandomValue(int size)
    {
        const char offset = 'a';
        const int lettersOffset = 26;

        var random = new Random();
        var builder = new StringBuilder(size);

        for (var i = 0; i < size; i++)
        {
            var @char = (char)random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return builder.ToString();
    }
}