using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Domain.Cities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Persistence.Cities;
using Persistence.Database;
using Xunit;

namespace Persistence.Test;

public class CityRepositoryTests
{
    private readonly Mock<DbSet<City>> _dbSetMock;
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly ICityRepository _repository;
    
    public CityRepositoryTests()
    {
        _dbSetMock = MockDbSet();
        _contextMock = new Mock<ApplicationDbContext>();
        _repository = new CityRepository(_contextMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCities()
    {
        // arrange
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);

        // act
        var actual = await _repository.GetAll();

        // assert
        actual.Count().Should().BePositive();
        actual.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByGeonameId_FindsCity_ReturnsExistingCity()
    {
        // arrange
        var city = Cities().ElementAt(0);
        var geoNameId = city.GeoNameId;
        var expected = Maybe.From(city);
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetByGeonameId(geoNameId);
        
        // assert
        expected.HasValue.Should().Be(actual.HasValue);
        expected.Value.Name.Should().Be(actual.Value.Name);
        actual.HasValue.Should().Be(true);
    }

    [Fact]
    public async Task GetByGeonameId_DoesNotFindCity_ReturnsNone()
    {
        // arrange
        var expected = Maybe<City>.From(null);
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetByGeonameId(string.Empty);
        
        // assert
        actual.HasNoValue.Should().Be(true);
        expected.HasNoValue.Should().Be(actual.HasNoValue);
    }

    [Fact]
    public async Task GetByName_FindsCity_ReturnsExistingCity()
    {
        // arrange
        var city = Cities().ElementAt(0);
        var name = city.Name;
        var expected = Maybe.From(city);
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetByName(name);
        
        // assert
        expected.HasValue.Should().Be(actual.HasValue);
        expected.Value.Name.Should().Be(actual.Value.ElementAt(0).Name);
        actual.HasValue.Should().Be(true);
    }
    
    [Fact]
    public async Task GetByName_DoesNotFindCity_ReturnsNone()
    {
        // arrange
        var cityNameNotPresentInDb = "new_york";
        var expected = Maybe<IEnumerable<City>>.From(new List<City>());
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetByName(cityNameNotPresentInDb);
        
        actual.Value.Count().Should().Be(0);
        expected.HasNoValue.Should().Be(actual.HasNoValue);
    }

    [Fact]
    public async Task GetByCountry_FindsCity_ReturnsExistingCities()
    {
        // arrange
        var city = Cities().ElementAt(0);
        const string country = "portugal";
        var expected = Maybe.From(Cities().Take(4));
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetByCountry(country);
        
        // assert
        expected.HasValue.Should().Be(actual.HasValue);
        expected.Value.Count().Should().Be(actual.Value.Count());
        actual.HasValue.Should().Be(true);
        
        var countryPreCondition = actual.Value.All(c => c.Country == country);
        countryPreCondition.Should().Be(true);
    }

    [Fact]
    public async Task GetByCountry_DoesNotFindCity_ReturnsNone()
    {
        // arrange
        const string country = "usa";
        var expected = Maybe.From(new List<City>());
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetByCountry(country);
        
        // assert
        expected.HasValue.Should().Be(actual.HasValue);
        expected.Value.Count().Should().Be(0);
        actual.Value.Count().Should().Be(0);
    }

    [Fact]
    public async Task GetBySubCountry_FindsCity_ReturnsExistingCities()
    {
        // arrange
        const string subCountry = "porto";
        var expected = Maybe.From(Cities().Skip(2).Take(2));
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetBySubCountry(subCountry);
        
        expected.HasValue.Should().Be(actual.HasValue);
        expected.Value.Count().Should().Be(actual.Value.Count());
        actual.HasValue.Should().Be(true);
        
        var countryPreCondition = actual.Value.All(c => c.SubCountry == subCountry);
        countryPreCondition.Should().Be(true);
    }

    [Fact]
    public async Task GetBySubCountry_DoesNotFindCity_ReturnsNone()
    {
        // arrange
        const string subCountry = "barcelona";
        var expected = Maybe.From(new List<City>());
        
        _contextMock.Setup(c => c.Cities).Returns(_dbSetMock.Object);
        
        // act
        var actual = await _repository.GetBySubCountry(subCountry);
        
        // assert
        expected.HasValue.Should().Be(actual.HasValue);
        expected.Value.Count().Should().Be(0);
        actual.Value.Count().Should().Be(0);
    }

    private Mock<DbSet<City>> MockDbSet()
    {
        return Cities()
            .AsQueryable()
            .BuildMockDbSet();
    }

    private static IEnumerable<City> Cities()
    {
        return new List<City>()
        {
            new("faro", "portugal", "faro", "faro_geonameId"),
            new("lisbon", "portugal", "lisbon", "lisbon_geonameId"),
            new("braga", "portugal", "porto", "braga_geonameId"),
            new("porto", "portugal", "porto", "porto_geonameId"),
            new("madrid", "spain", "madrid", "madrid_geonameId"),
            new("amsterdam", "netherlands", "amsterdam", "amsterdam_geonameId"),
            new("thehague", "netherlands", "hague", "hague_geonameId")
        };
    }
}