using System.Threading.Tasks;

namespace DataProcessor.Library.Tests;

public class DataParserTests
{
    private DataParser GetParserWithFakeLogger()
    {
        FakeLogger logger = new();
        DataParser parser = new(logger);
        return parser;
    }

    [Fact]
    public async Task ParseData_WithMixedData_ReturnsGoodRecords()
    {
        // Arrange
        DataParser parser = GetParserWithFakeLogger();

        // Act
        var records = await parser.ParseData(TestData.Data);

        // Assert
        Assert.Equal(14, records.Count);
    }

    [Fact]
    public async Task ParseData_WithGoodRecord_ReturnsOneRecord()
    {
        DataParser parser = GetParserWithFakeLogger();
        var records = await parser.ParseData(TestData.GoodRecord);
        Assert.Single(records);
    }

    [Fact]
    public async Task ParseData_WithBadRecord_ReturnsZeroRecords()
    {
        DataParser parser = GetParserWithFakeLogger();
        var records = await parser.ParseData(TestData.BadRecord);
        Assert.Empty(records);
    }

    [Fact]
    public async Task ParseData_WithBadStartDate_ReturnsZeroRecords()
    {
        DataParser parser = GetParserWithFakeLogger();
        var records = await parser.ParseData(TestData.BadStartDate);
        Assert.Empty(records);
    }

    [Fact]
    public async Task ParseData_WithBadRating_ReturnsZeroRecords()
    {
        DataParser parser = GetParserWithFakeLogger();
        var records = await parser.ParseData(TestData.BadRating);
        Assert.Empty(records);
    }
}
