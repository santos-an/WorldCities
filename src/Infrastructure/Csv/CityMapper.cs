using CsvHelper.Configuration;
using Domain.Entities;

namespace Infrastructure.Csv;

public class CityMapper : ClassMap<City>
{
    public CityMapper()
    {
        Map(p => p.Name).Name("name");
        Parameter("name").Name(nameof(City.Name));
        
        Map(p => p.Country).Name("country");
        Parameter("country").Name(nameof(City.Country));
        
        Map(p => p.SubCountry).Name("subCountry");
        Parameter("subCountry").Name(nameof(City.Country));
        
        Map(p => p.GeoNameId).Name("geonameId");
        Parameter("geonameId").Name(nameof(City.GeoNameId));
    }
}