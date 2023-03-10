using System.Globalization;
using System.Text;
using Application.Interfaces.Infrastructure;
using CSharpFunctionalExtensions;
using CsvHelper.Configuration;
using Domain.Cities;
using Microsoft.Extensions.Options;

namespace Infrastructure.Cities;

public class CsvReader : ICsvReader
{
    private readonly CsvOptions _csvOptions;

    public CsvReader(IOptionsMonitor<CsvOptions> csvOptions) => _csvOptions = csvOptions.CurrentValue;

    public Result<IEnumerable<City>> Read()
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            Delimiter = ",",
            HasHeaderRecord = false
        };

        if (!File.Exists(_csvOptions.FileName))
            return Result.Failure<IEnumerable<City>>(
                $"There is no csv file at at {_csvOptions.FileName}");

        using var fs = File.Open(_csvOptions.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (!fs.CanRead)
            return Result.Failure<IEnumerable<City>>(
                $"Not possible to open the csv file. Please make sure there is a csv at {_csvOptions.FileName}");

        using var reader = new StreamReader(fs, Encoding.UTF8);
        using var csv = new CsvHelper.CsvReader(reader, configuration);

        return ParseToCities(csv);
    }

    private static Result<IEnumerable<City>> ParseToCities(CsvHelper.CsvReader csv)
    {
        csv.Context.RegisterClassMap<CityMapper>();

        try
        {
            var cities = csv.GetRecords<City>().ToList();

            return Result.Success<IEnumerable<City>>(cities);
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<City>>(
                $"Not possible to parse csv rows to a list of cities. Please check the following error: {e.Message}");
        }
    }
}