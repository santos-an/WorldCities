using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using CSharpFunctionalExtensions;

namespace Application.Cities.Queries.GetById;

public record GetCityByIdQuery : IQuery<Result<GetCityResponse>>
{
    public Guid Id { get; init; }
}

public class GetCityByIdQueryHandler : IQueryHandler<GetCityByIdQuery, Result<GetCityResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCityByIdQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<GetCityResponse>> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var cityOrNothing = await _unitOfWork.Cities.GetById(request.Id);
        if (cityOrNothing.HasNoValue)
            return Result.Failure<GetCityResponse>($"There is no city for the given id {request.Id}");

        var city = cityOrNothing.Value;
        var response = new GetCityResponse
        {
            Id = city.Id, Name = city.Name, Country = city.Country, SubCountry = city.SubCountry,
            GeonameId = city.GeonameId
        };

        return Result.Success(response);
    }
}