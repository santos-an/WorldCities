using Application.Cities.Commands.Create;
using Application.Cities.Queries.GetAll;
using Application.Cities.Queries.GetByGeoNameId;
using Application.Cities.Queries.GetById;
using Application.Cities.Queries.GetByName;
using Application.Cities.Queries.GetBySubCountry;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Cities.Requests;

namespace Presentation.Cities;

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
        if (response.IsFailure)
            return BadRequest(response.Error);
        
        return Ok(response.Value.Take(100));
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCityByIdQuery { Id = id };
        
        var response = await _mediator.Send(query);
        if (response.IsFailure)
            return BadRequest(response.Error);
        
        return Ok(response.Value);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByGeonameId(string geoNameId)
    {
        var query = new GetCityByGeoNameId { GeonameId = geoNameId };
        
        var response = await _mediator.Send(query);
        if (response.IsFailure)
            return BadRequest(response.Error);
        
        return Ok(response.Value);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetWithName(string name)
    {
        var query = new GetCitiesWithNameQuery { Name = name };
        
        var response = await _mediator.Send(query);
        if (response.IsFailure)
            return BadRequest(response.Error);
        
        return Ok(response.Value);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetWithSubCountry(string subCountry)
    {
        var query = new GetCitiesWithSubCountryQuery { SubCountry = subCountry };
        
        var response = await _mediator.Send(query);
        if (response.IsFailure)
            return BadRequest(response.Error);
        
        return Ok(response.Value);
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
        if (response.IsFailure)
            return BadRequest(response.Error);
        
        return Ok();
    }
    
}