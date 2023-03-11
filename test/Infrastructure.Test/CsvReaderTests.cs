using FluentAssertions;
using Infrastructure.Cities;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Infrastructure.Test;

public class CsvReaderTests
{
    private readonly Mock<IOptionsMonitor<CsvOptions>> _optionsMonitor;
    
    public CsvReaderTests() => _optionsMonitor = new Mock<IOptionsMonitor<CsvOptions>>();

    [Fact]
    public void Read_DoesNotFindCsvFile_ReturnsFailure()
    {
        // arrange
        var csvOptions = new CsvOptions { FileName = string.Empty };
        _optionsMonitor.Setup(m => m.CurrentValue).Returns(csvOptions);

        var reader = new CsvReader(_optionsMonitor.Object);
        
        // act
        var result = reader.Read();
        
        // assert 
        result.IsFailure.Should().Be(true);
        result.IsSuccess.Should().Be(false);
    }
}