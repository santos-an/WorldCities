using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Domain.Entities;

namespace Application.Cities.Queries.GetAll;

public record GetAllCitiesQuery : IQuery<List<GetAllCitiesResponse>>;

public class GetCoursesQueryHandler : IQueryHandler<GetAllCitiesQuery, List<GetAllCitiesResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCoursesQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<List<GetAllCitiesResponse>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await _unitOfWork.Cities.GetAll();

        return cities
            .Select(ToResponse)
            .ToList();
    }
    
    GetAllCitiesResponse ToResponse(City c)
    {
        return new GetAllCitiesResponse
        {
            Id = c.Id, 
            Name = c.Name, 
            Country = c.Country,
            GeonameId = c.GeonameId,
            SubCountry = c.SubCountry
        };
    }
}