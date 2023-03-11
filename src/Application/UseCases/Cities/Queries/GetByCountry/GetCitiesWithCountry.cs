using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Application.UseCases.Cities.Queries.GetByCountry;

public class GetCitiesWithCountryQuery : IQuery<Result<List<GetCityResponse>>>
{
    public string Country { get; init; }
}

public class GetByCountryQueryHandler : IQueryHandler<GetCitiesWithCountryQuery, Result<List<GetCityResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByCountryQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<List<GetCityResponse>>> Handle(GetCitiesWithCountryQuery request, CancellationToken cancellationToken)
    {
        var citiesOrNothing = await _unitOfWork.Cities.GetByCountry(request.Country);
        if (citiesOrNothing.HasNoValue)
            return Result.Failure<List<GetCityResponse>>($"There is no city for the given Country {request.Country}");

        var cities = citiesOrNothing.Value;
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