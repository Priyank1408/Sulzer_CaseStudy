using CaseStudy1.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace CaseStudy1.Tests;

public class DataControllerTests
{
    private readonly DataController _controller;
    private readonly Mock<IDataIngestionService> _serviceMock;

    public DataControllerTests()
    {
        _serviceMock = new Mock<IDataIngestionService>();
        _controller = new DataController(_serviceMock.Object);
    }

    private record UploadResponse(string FileId);

    [Fact]
    public async Task UploadData_ValidFile_ReturnsOkResult()
    {
        // Arrange
        var fileName = "test.txt";
        var content = "Test content";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var file = new FormFile(stream, 0, stream.Length, "file", fileName);

        _serviceMock.Setup(x => x.IngestDataAsync(
            It.IsAny<Stream>(),
            It.IsAny<string>(),
            It.IsAny<string>()))
            .ReturnsAsync("test-id");

        // Act
        var result = await _controller.UploadData(file, "test-tenant", "test-type");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = JsonSerializer.Deserialize<UploadResponse>(
            JsonSerializer.Serialize(okResult.Value),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        Assert.NotNull(response);
        Assert.Equal("test-id", response.FileId);

        // Verify service was called with correct parameters
        _serviceMock.Verify(x => x.IngestDataAsync(
            It.IsAny<Stream>(),
            "test-tenant",
            "test-type"),
            Times.Once);
    }

    [Fact]
    public async Task UploadData_NullFile_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.UploadData(null, "test-tenant", "test-type");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DownloadData_ValidId_ReturnsFileResult()
    {
        // Arrange
        var testData = new byte[] { 1, 2, 3, 4, 5 };
        _serviceMock.Setup(x => x.RetrieveDataAsync("test-id", "test-tenant"))
            .ReturnsAsync(testData);

        // Act
        var result = await _controller.DownloadData("test-tenant", "test-id");

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/octet-stream", fileResult.ContentType);
        Assert.Equal(testData, fileResult.FileContents);
    }

    [Fact]
    public async Task DownloadData_FileNotFound_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(x => x.RetrieveDataAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new FileNotFoundException());

        // Act
        var result = await _controller.DownloadData("test-tenant", "non-existent-id");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}