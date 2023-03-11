using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetById;
using CSharpFunctionalExtensions;
using Domain.Cities;

namespace Application.UseCases.Cities.Queries.GetAll;

public record GetAllCitiesQuery : IQuery<Result<List<GetCityResponse>>>;

public class GetCoursesQueryHandler : IQueryHandler<GetAllCitiesQuery, Result<List<GetCityResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetCoursesQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<Result<List<GetCityResponse>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await _unitOfWork.Cities.GetAll();

        return cities
            .Select(ToResponse)
            .ToList();
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