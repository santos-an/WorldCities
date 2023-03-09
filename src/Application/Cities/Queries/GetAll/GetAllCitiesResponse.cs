namespace Application.Cities.Queries.GetAll;

public record GetAllCitiesResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string SubCountry { get; set; }
    public string GeonameId { get; set; }
}