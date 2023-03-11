using Domain.Cities;
using FluentAssertions;
using Xunit;

namespace Domain.Test;

public class CityTests
{
    private readonly City _city; 
    
    public CityTests()
    {
        _city = new City("city_name", "country", "sub_country", "geo_name_id");
    }
    
    [Fact]
    public void UpdateId_UpdatesEntity()
    {
        // arrange
        var expected = Guid.NewGuid();
        
        // act
        _city.UpdateId(expected);
        
        // assert
        expected.Should().Be(_city.Id);
    }
}