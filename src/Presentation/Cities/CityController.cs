using Application.Cities.Queries.GetAll;
using Application.Cities.Queries.GetByGeoNameId;
using Application.Cities.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        
        return Ok(response.Take(100));
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
}