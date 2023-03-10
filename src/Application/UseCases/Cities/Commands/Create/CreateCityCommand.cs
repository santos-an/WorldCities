using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Application.UseCases.Cities.Commands.Create;

public record CreateCityCommand : ICommand<Result>
{
    public string Name { get; init; }
    public string Country { get; init; }
    public string SubCountry { get; init; }
    public string GeonameId { get; init; }
}

public class CreateCityCommandHandler : ICommandHandler<CreateCityCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCityCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var city = new City
        {
            Name = request.Name,
            Country = request.Country,
            GeonameId = request.GeonameId,
            SubCountry = request.SubCountry
        };

        await _unitOfWork.Cities.AddAsync(city);
        var result = await _unitOfWork.CommitAsync();
        
        return result == 0 ? 
            Result.Failure("Not possible to insert new city") : 
            Result.Success();
    }
}