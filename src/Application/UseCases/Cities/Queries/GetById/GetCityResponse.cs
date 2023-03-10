namespace Application.UseCases.Cities.Queries.GetById;

public record GetCityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string SubCountry { get; set; }
    public string GeonameId { get; set; }
}