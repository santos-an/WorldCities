using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetByCountry;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;
using Domain.Cities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApplicationTests.Cities;

public class GetCitiesWithCountryQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly IQueryHandler<GetCitiesWithCountryQuery, Result<List<GetCityResponse>>> _handler;

    public GetCitiesWithCountryQueryTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetByCountryQueryHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_FindsCitiesWithCountries_ReturnsSuccess()
    {
        // arrange
        var query = new GetCitiesWithCountryQuery();
        var repositoryResponse = Maybe.From<IEnumerable<City>>(new List<City>());
        
        _unitOfWork.Setup(u => u.Cities.GetByCountry(It.IsAny<string>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetByCountry(It.IsAny<string>()), Times.Once());
    }
    
    [Fact]
    public async Task Handle_DoesNotFindCitiesWithCountries_ReturnsFailure()
    {
        // arrange
        var query = new GetCitiesWithCountryQuery();
        var repositoryResponse = Maybe.From<IEnumerable<City>>(null);
        
        _unitOfWork.Setup(u => u.Cities.GetByCountry(It.IsAny<string>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsFailure.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetByCountry(It.IsAny<string>()), Times.Once());
    }
}