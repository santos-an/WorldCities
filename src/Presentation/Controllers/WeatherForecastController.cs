using Application;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IReader _reader;

    public WeatherForecastController(IReader reader)
    {
        _reader = reader;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        var result = _reader.Read();
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value.Take(100));
    }
}