using Api.Cities;
using Api.Cities.Requests;
using Application.UseCases.Cities.Commands.Create;
using Application.UseCases.Cities.Queries.GetAll;
using Application.UseCases.Cities.Queries.GetByCountry;
using Application.UseCases.Cities.Queries.GetByGeoNameId;
using Application.UseCases.Cities.Queries.GetById;
using Application.UseCases.Cities.Queries.GetByName;
using Application.UseCases.Cities.Queries.GetBySubCountry;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Test;

public class CityControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly CityController _controller;

    public CityControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new CityController(_mediator.Object);
    }

    [Fact]
    public async Task GetAll_Succeeds_ReturnsOkResponse()
    {
        // arrange
        var expected = Result.Success(new List<GetCityResponse>());
        _mediator.Setup(m => m.Send(It.IsAny<GetAllCitiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetAll();
        var actual = (result as OkObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetAllCitiesQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        actual.Should().NotBeNull();
        
        expected.Value.Should().NotBeNull();
        expected.Value.Should().BeEmpty();
        expected.IsSuccess.Should().Be(true);
    }
    
    [Fact]
    public async Task GetAll_Fails_ReturnsBadRequestResponse()
    {
        // arrange
        var expected = Result.Failure<List<GetCityResponse>>("some error");
        _mediator.Setup(m => m.Send(It.IsAny<GetAllCitiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetAll();
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetAllCitiesQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        result.Should().NotBeNull();
        actual.Should().BeOfType<string>();

        expected.IsFailure.Should().Be(true);
        expected.IsSuccess.Should().Be(false);
    }

    [Fact]
    public async Task GetById_FindsCity_ReturnsOkResponse()
    {
        // arrange
        var expected = Result.Success(new GetCityResponse());
        _mediator.Setup(m => m.Send(It.IsAny<GetCityByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetById(It.IsAny<Guid>());
        var actual = (result as OkObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCityByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<GetCityResponse>();

        expected.Value.Should().NotBeNull();
        expected.IsSuccess.Should().Be(true);
    }

    [Fact]
    public async Task GetById_DoesNotFindCity_ReturnsBadRequestResponse()
    {
        var expected = Result.Failure<GetCityResponse>("some error");
        _mediator.Setup(m => m.Send(It.IsAny<GetCityByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetById(It.IsAny<Guid>());
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCityByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();

        expected.IsFailure.Should().Be(true);
        expected.IsSuccess.Should().Be(false);
    }

    [Fact]
    public async Task GetByGeonameId_FindsCity_ReturnsOkResponse()
    {
        // arrange
        var expected = Result.Success(new GetCityResponse());
        _mediator.Setup(m => m.Send(It.IsAny<GetCityByGeoNameIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetByGeonameId(It.IsAny<string>());
        var actual = (result as OkObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCityByGeoNameIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<GetCityResponse>();

        expected.Value.Should().NotBeNull();
        expected.IsSuccess.Should().Be(true);
    }
    
    [Fact]
    public async Task GetByGeonameId_DoesNotFindCity_ReturnsBadRequestObjectResponse()
    {
        var expected = Result.Failure<GetCityResponse>("some error");
        _mediator.Setup(m => m.Send(It.IsAny<GetCityByGeoNameIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetByGeonameId(It.IsAny<string>());
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCityByGeoNameIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();

        expected.IsFailure.Should().Be(true);
        expected.IsSuccess.Should().Be(false);
    }

    [Fact]
    public async Task GetWithName_FindsCity_ReturnsOkResponse()
    {
        // arrange
        var expected = Result.Success(new List<GetCityResponse>());
        _mediator.Setup(m => m.Send(It.IsAny<GetCitiesWithNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetWithName(It.IsAny<string>());
        var actual = (result as OkObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCitiesWithNameQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        actual.Should().NotBeNull();

        expected.Value.Should().NotBeNull();
        expected.IsSuccess.Should().Be(true);
    }
    
    [Fact]
    public async Task GetWithName_DoesNotFindCity_ReturnsBadRequestObjectResponse()
    {
        // arrange
        var expected = Result.Failure<List<GetCityResponse>>("some error");
        _mediator.Setup(m => m.Send(It.IsAny<GetCitiesWithNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetWithName(It.IsAny<string>());
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCitiesWithNameQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();

        expected.IsFailure.Should().Be(true);
        expected.IsSuccess.Should().Be(false);
    }

    [Fact]
    public async Task GetWithCountry_FindsCity_ReturnsOkResponse()
    {
        // arrange
        var expected = Result.Success(new List<GetCityResponse>());
        _mediator.Setup(m => m.Send(It.IsAny<GetCitiesWithCountryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetWithCountry(It.IsAny<string>());
        var actual = (result as OkObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCitiesWithCountryQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        actual.Should().NotBeNull();

        expected.Value.Should().NotBeNull();
        expected.IsSuccess.Should().Be(true);
    }
    
    [Fact]
    public async Task GetWithCountry_DoesNotFindCity_ReturnsBadRequestObjectResponse()
    {
        var expected = Result.Failure<List<GetCityResponse>>("some error");
        _mediator.Setup(m => m.Send(It.IsAny<GetCitiesWithCountryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetWithCountry(It.IsAny<string>());
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCitiesWithCountryQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();

        expected.IsFailure.Should().Be(true);
        expected.IsSuccess.Should().Be(false);
    }

    [Fact]
    public async Task GetWithSubCountry_FindsCity_ReturnsOkResponse()
    {
        // arrange
        var expected = Result.Success(new List<GetCityResponse>());
        _mediator.Setup(m => m.Send(It.IsAny<GetCitiesWithSubCountryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetWithSubCountry(It.IsAny<string>());
        var actual = (result as OkObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCitiesWithSubCountryQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        actual.Should().NotBeNull();

        expected.Value.Should().NotBeNull();
        expected.IsSuccess.Should().Be(true);
    }
    
    [Fact]
    public async Task GetWithSubCountry_DoesNotFindCity_ReturnsBadRequestObjectResponse()
    {
        var expected = Result.Failure<List<GetCityResponse>>("some error");
        _mediator.Setup(m => m.Send(It.IsAny<GetCitiesWithSubCountryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.GetWithSubCountry(It.IsAny<string>());
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<GetCitiesWithSubCountryQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();

        expected.IsFailure.Should().Be(true);
        expected.IsSuccess.Should().Be(false);
    }
    
    [Fact]
    public async Task Create_DoesNotSucceed_ReturnsBadRequestObjectResponse()
    {
        var expected = Result.Failure("some error");
        _mediator.Setup(m => m.Send(It.IsAny<CreateCityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // act
        var result = await _controller.Create(new CreateCityRequest());
        var actual = (result as BadRequestObjectResult).Value;

        // assert
        _mediator.Verify(m => m.Send(It.IsAny<CreateCityCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeOfType<BadRequestObjectResult>();
        result.Should().NotBeNull();
        
        actual.Should().NotBeNull();
        actual.Should().BeOfType<string>();

        expected.IsFailure.Should().Be(true);
        expected.IsSuccess.Should().Be(false);
    }
}