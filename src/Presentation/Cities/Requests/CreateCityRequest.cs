namespace Presentation.Cities.Requests;

public record CreateCityRequest
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string SubCountry { get; set; }
    public string GeonameId { get; set; }
}