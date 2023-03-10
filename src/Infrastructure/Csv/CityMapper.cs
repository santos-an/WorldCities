using CsvHelper.Configuration;
using Domain.Entities;

namespace Infrastructure.Csv;

public class CityMapper : ClassMap<City>
{
    public CityMapper()
    {
        Map(p => p.Name).Name("name");
        Map(p => p.Country).Name("country");
        Map(p => p.SubCountry).Name("subCountry");
        Map(p => p.GeonameId).Name("geonameId");
    }
}