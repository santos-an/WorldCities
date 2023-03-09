using Application.Cities.Queries.GetAll;
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
    public async Task<IActionResult> GetById()
    {
        var query = new GetAllCitiesQuery();
        var response = await _mediator.Send(query);
        
        return Ok(response.Take(100));
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByGeonameId()
    {
        var query = new GetAllCitiesQuery();
        var response = await _mediator.Send(query);
        
        return Ok(response.Take(100));
    }
}