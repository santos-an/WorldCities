using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Commands.Create;
using CSharpFunctionalExtensions;
using Domain.Cities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApplicationTests.Cities;

public class CreateCityCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ICommandHandler<CreateCityCommand, Result> _handler;

    public CreateCityCommandTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateCityCommandHandler(_unitOfWork.Object);
    }
    
    [Fact]
    public async Task Handle_Succeeds_ReturnsSuccess()
    {
        // arrange
        var command = new CreateCityCommand();
        
        _unitOfWork.Setup(u => u.Cities.AddAsync(It.IsAny<City>()));
        _unitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // act
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(true);
        
        _unitOfWork.Verify(u => u.Cities.AddAsync(It.IsAny<City>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_Fails_ReturnsFailure()
    {
        // arrange
        var command = new CreateCityCommand();
        
        _unitOfWork.Setup(u => u.Cities.AddAsync(It.IsAny<City>()));
        _unitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(0);

        // act
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(false);
        result.IsFailure.Should().Be(true);

        _unitOfWork.Verify(u => u.Cities.AddAsync(It.IsAny<City>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
}