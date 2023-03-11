using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Application.UseCases.Cities.Queries.GetBySubCountry;

public record GetCitiesWithSubCountryQuery : IQuery<Result<List<GetCityResponse>>>
{
    public string SubCountry { get; init; }
}

public class GetCitiesWithSubCountryQueryHandler : IQueryHandler<GetCitiesWithSubCountryQuery, Result<List<GetCityResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCitiesWithSubCountryQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<List<GetCityResponse>>> Handle(GetCitiesWithSubCountryQuery request, CancellationToken cancellationToken)
    {
        var citiesOrNothing = await _unitOfWork.Cities.GetBySubCountry(request.SubCountry);
        if (citiesOrNothing.HasNoValue)
            return Result.Failure<List<GetCityResponse>>($"There are no cities for the given sub-country {request.SubCountry}");

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
            GeonameId = c.GeoNameId,
            SubCountry = c.SubCountry
        };
    }
}