using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Application.UseCases.Cities.Queries.GetByName;

public record GetCitiesWithNameQuery : IQuery<Result<List<GetCityResponse>>>
{
    public string Name { get; init; }
}

public class GetCitiesWithNameQueryHandler : IQueryHandler<GetCitiesWithNameQuery, Result<List<GetCityResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCitiesWithNameQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<List<GetCityResponse>>> Handle(GetCitiesWithNameQuery request, CancellationToken cancellationToken)
    {
        var citiesOrNothing = await _unitOfWork.Cities.GetByName(request.Name);
        if (citiesOrNothing.HasNoValue)
            return Result.Failure<List<GetCityResponse>>($"There are no cities for the given Name {request.Name}");

        var cities = citiesOrNothing
            .Value
            .ToList();
        
        var response = cities
            .Select(ToResponse)
            .ToList();

        return Result.Success(response);
    }
    
    GetCityResponse ToResponse(City c)
    {
        return new GetCityResponse
        {
            Id = c.Id, 
            Name = c.Name, 
            Country = c.Country,
            GeonameId = c.GeonameId,
            SubCountry = c.SubCountry
        };
    }
}