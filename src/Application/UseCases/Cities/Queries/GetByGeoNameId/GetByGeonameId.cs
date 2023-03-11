using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;

namespace Application.UseCases.Cities.Queries.GetByGeoNameId;

public record GetCityByGeoNameIdQuery : IQuery<Result<GetCityResponse>>
{
    public string GeonameId { get; init; }
}

public class GetCityByGeoNameIdQueryHandler : IQueryHandler<GetCityByGeoNameIdQuery, Result<GetCityResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCityByGeoNameIdQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<GetCityResponse>> Handle(GetCityByGeoNameIdQuery request, CancellationToken cancellationToken)
    {
        var cityOrNothing = await _unitOfWork.Cities.GetByGeonameId(request.GeonameId);
        if (cityOrNothing.HasNoValue)
            return Result.Failure<GetCityResponse>($"There is no city for the given GeonameId {request.GeonameId}");

        var city = cityOrNothing.Value;
        var response = new GetCityResponse
        {
            Id = city.Id, Name = city.Name, Country = city.Country, SubCountry = city.SubCountry,
            GeonameId = city.GeoNameId
        };

        return Result.Success(response);
    }
}