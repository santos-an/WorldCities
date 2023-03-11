using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetByGeoNameId;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;
using Domain.Cities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApplicationTest.Cities;

public class GetCityByGeoNameIdQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly IQueryHandler<GetCityByGeoNameIdQuery, Result<GetCityResponse>> _handler;

    public GetCityByGeoNameIdQueryTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetCityByGeoNameIdQueryHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_FindsCitiesWithCountries_ReturnsSuccess()
    {
        // arrange
        var query = new GetCityByGeoNameIdQuery();
        var repositoryResponse = Maybe.From(new City());
        
        _unitOfWork.Setup(u => u.Cities.GetByGeonameId(It.IsAny<string>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetByGeonameId(It.IsAny<string>()), Times.Once());
    }
    
    [Fact]
    public async Task Handle_DoesNotFindCitiesWithCountries_ReturnsFailure()
    {
        // arrange
        var query = new GetCityByGeoNameIdQuery();
        var repositoryResponse = Maybe.From<City>(null);
        
        _unitOfWork.Setup(u => u.Cities.GetByGeonameId(It.IsAny<string>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsFailure.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetByGeonameId(It.IsAny<string>()), Times.Once());
    }
}