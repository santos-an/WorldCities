using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;
using Domain.Cities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApplicationTests.Cities;

public class GetCityByIdQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly IQueryHandler<GetCityByIdQuery, Result<GetCityResponse>> _handler;

    public GetCityByIdQueryTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetCityByIdQueryHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_FindsCitiesWithCountries_ReturnsSuccess()
    {
        // arrange
        var query = new GetCityByIdQuery();
        var repositoryResponse = Maybe.From(new City());
        
        _unitOfWork.Setup(u => u.Cities.GetById(It.IsAny<Guid>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetById(It.IsAny<Guid>()), Times.Once());
    }
    
    [Fact]
    public async Task Handle_DoesNotFindCitiesWithCountries_ReturnsFailure()
    {
        // arrange
        var query = new GetCityByIdQuery();
        var repositoryResponse = Maybe.From<City>(null);
        
        _unitOfWork.Setup(u => u.Cities.GetById(It.IsAny<Guid>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsFailure.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetById(It.IsAny<Guid>()), Times.Once());
    }
}