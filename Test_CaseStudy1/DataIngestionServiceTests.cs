using Xunit;
using Moq;
using CaseStudy1.Interface;
using CaseStudy1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CaseStudy1.Tests;

public class DataIngestionServiceTests
{
    private readonly IDataIngestionService _service;
    private readonly Mock<ILogger<DataIngestionService>> _loggerMock;
    private readonly IConfiguration _configuration;

    public DataIngestionServiceTests()
    {
        _loggerMock = new Mock<ILogger<DataIngestionService>>();
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"LocalStorage:Path", Path.Combine(Path.GetTempPath(), "TestData")}
            })
            .Build();

        _service = new DataIngestionService(_loggerMock.Object, _configuration);
    }

    [Fact]
    public async Task IngestDataAsync_ValidData_ReturnsId()
    {
        // Arrange
        var testData = "Test content";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
        var tenantId = "test-tenant";
        var dataType = "test-type";

        // Act
        var result = await _service.IngestDataAsync(stream, tenantId, dataType);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task RetrieveDataAsync_ExistingData_ReturnsCorrectData()
    {
        // Arrange
        var testData = "Test content";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
        var tenantId = "test-tenant";
        var dataType = "test-type";

        // Act
        var id = await _service.IngestDataAsync(stream, tenantId, dataType);
        var result = await _service.RetrieveDataAsync(id, tenantId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testData, System.Text.Encoding.UTF8.GetString(result));
    }
}