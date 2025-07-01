using Moq;

namespace DataProcessor.Library.Tests;

public class DataParserLoggerTests
{
    [Fact]
    public async Task ParseData_WithBadRecord_LoggerIsCalledOnce()
    {
        // Arrange
        Mock<ILogger> mockLogger = new();
        DataParser parser = new(mockLogger.Object);

        // Act
        await parser.ParseData(TestData.BadRecord);

        // Assert
        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.BadRecord[0]),
            Times.Once());
    }

    [Fact]
    public async Task ParseData_WithGoodRecord_LoggerIsNotCalled()
    {
        Mock<ILogger> mockLogger = new();
        DataParser parser = new(mockLogger.Object);

        await parser.ParseData(TestData.GoodRecord);

        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.GoodRecord[0]),
            Times.Never());
    }

    [Fact]
    public async Task ParseData_WithBadStartDate_LoggerIsCalledOnce()
    {
        Mock<ILogger> mockLogger = new();
        DataParser parser = new(mockLogger.Object);

        await parser.ParseData(TestData.BadStartDate);

        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.BadStartDate[0]),
            Times.Once());
    }

    [Fact]
    public async Task ParseData_WithBadRating_LoggerIsCalledOnce()
    {
        Mock<ILogger> mockLogger = new();
        DataParser parser = new(mockLogger.Object);

        await parser.ParseData(TestData.BadRating);

        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.BadRating[0]),
            Times.Once());
    }
}
