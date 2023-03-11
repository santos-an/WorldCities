using System.Globalization;
using System.Text;
using Application.Interfaces.Infrastructure;
using CSharpFunctionalExtensions;
using CsvHelper.Configuration;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infrastructure.Csv;

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

        using var fs = File.Open(_csvOptions.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (!fs.CanRead)
            return Result.Failure<IEnumerable<City>>(
                $"Not possible to read the csv file. Please make sure there is a csv at {_csvOptions.FileName}");

        using var textReader = new StreamReader(fs, Encoding.UTF8);
        using var csv = new CsvHelper.CsvReader(textReader, configuration);

        return ParseToCities(csv);
    }

    private static Result<IEnumerable<City>> ParseToCities(CsvHelper.CsvReader csv)
    {
        csv.Context.RegisterClassMap<CityMapper>();

        try
        {
            var cities = csv
                .GetRecords<City>()
                .ToList();

            return Result.Success<IEnumerable<City>>(cities);
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<City>>(
                $"Not possible to parse csv rows to a list of cities. Please check the following error: {e.Message}");
        }
    }
}