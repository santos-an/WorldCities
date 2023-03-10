using Api.Cities.Requests;
using Application.UseCases.Cities.Commands.Create;
using Application.UseCases.Cities.Queries.GetAll;
using Application.UseCases.Cities.Queries.GetByCountry;
using Application.UseCases.Cities.Queries.GetByGeoNameId;
using Application.UseCases.Cities.Queries.GetById;
using Application.UseCases.Cities.Queries.GetByName;
using Application.UseCases.Cities.Queries.GetBySubCountry;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Cities;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
    private readonly IMediator _mediator;

    public CityController(IMediator mediator) => _mediator = mediator;
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCitiesQuery();
        
        var response = await _mediator.Send(query);
        return response.IsFailure ? 
            BadRequest(response.Error) : 
            Ok(response.Value.Take(100));
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCityByIdQuery { Id = id };
        
        var response = await _mediator.Send(query);
        return response.IsFailure ? 
            BadRequest(response.Error) : 
            Ok(response.Value);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByGeonameId(string geoNameId)
    {
        var query = new GetCityByGeoNameId { GeonameId = geoNameId };
        
        var response = await _mediator.Send(query);
        return response.IsFailure ? 
            BadRequest(response.Error) : 
            Ok(response.Value);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetWithName(string name)
    {
        var query = new GetCitiesWithNameQuery { Name = name };
        
        var response = await _mediator.Send(query);
        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetWithCountry(string country)
    {
        var query = new GetCitiesWithCountryQuery { Country = country };
        
        var response = await _mediator.Send(query);
        return response.IsFailure ? 
            BadRequest(response.Error) : 
            Ok(response.Value);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetWithSubCountry(string subCountry)
    {
        var query = new GetCitiesWithSubCountryQuery { SubCountry = subCountry };
        
        var response = await _mediator.Send(query);
        return response.IsFailure ? 
            BadRequest(response.Error) : 
            Ok(response.Value);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(CreateCityRequest request)
    {
        var command = new CreateCityCommand
        {
            Name = request.Name,
            Country = request.Country,
            SubCountry = request.SubCountry,
            GeonameId = request.GeonameId
        };
        
        var response = await _mediator.Send(command);
        return response.IsFailure ? 
            BadRequest(response.Error) : 
            Ok();
    }
    
}