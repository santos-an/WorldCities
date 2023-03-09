using Application.Cities.Queries.GetById;
using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;

namespace Application.Cities.Queries.GetByGeoNameId;

public record GetCityByGeoNameId : IQuery<Result<GetCityResponse>>
{
    public string GeonameId { get; init; }
}

public class GetCityByGeoNameIdQueryHandler : IQueryHandler<GetCityByGeoNameId, Result<GetCityResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCityByGeoNameIdQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<GetCityResponse>> Handle(GetCityByGeoNameId request, CancellationToken cancellationToken)
    {
        var cityOrNothing = await _unitOfWork.Cities.GetByGeonameId(request.GeonameId);
        if (cityOrNothing.HasNoValue)
            return Result.Failure<GetCityResponse>($"There is no city for the given GeonameId {request.GeonameId}");

        var city = cityOrNothing.Value;
        var response = new GetCityResponse
        {
            Id = city.Id, Name = city.Name, Country = city.Country, SubCountry = city.SubCountry,
            GeonameId = city.GeonameId
        };

        return Result.Success(response);
    }
}