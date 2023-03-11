namespace Domain.Cities;

public class City 
{
    public Guid Id { get; private set; }
    public string Name { get; }
    public string Country { get; }
    public string SubCountry { get; }
    public string GeoNameId { get; }

    public City() { }
    
    public City(string name, string country, string subCountry, string geonameId)
    {
        Name = name;
        Country = country;
        SubCountry = subCountry;
        GeoNameId = geonameId;
    }

    public void UpdateId(Guid id) => Id = id;
}