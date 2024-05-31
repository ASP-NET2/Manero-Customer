using Manero_Customer.Services;
using Microsoft.AspNetCore.Components.Forms;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ManeroCustomerTest;

public class ImgServiceTest
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly ImageService _imageService;

    public ImgServiceTest()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockConfig = new Mock<IConfiguration>();
        _imageService = new ImageService(_httpClient, _mockConfig.Object);
    }

    [Fact]
    public async Task UploadImageAsync_ReturnsResponseContent_WhenSuccess()
    {
        // Arrange
        var fileMock = new MockBrowserFile("test.jpg", "image/jpeg");

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("https://example.com/image.jpg")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://blobmanero.azurewebsites.net/api/Upload")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _imageService.UploadImageAsync(fileMock);

        // Assert
        Assert.Equal("https://example.com/image.jpg", result);
    }

    [Fact]
    public async Task UploadImageAsync_ThrowsException_WhenHttpRequestException()
    {
        // Arrange
        var fileMock = new MockBrowserFile("test.jpg", "image/jpeg");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://blobmanero.azurewebsites.net/api/Upload")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _imageService.UploadImageAsync(fileMock));
    }

    [Fact]
    public async Task UploadImageAsync_ThrowsException_WhenGeneralException()
    {
        // Arrange
        var fileMock = new MockBrowserFile("test.jpg", "image/jpeg");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://blobmanero.azurewebsites.net/api/Upload")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception("General error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _imageService.UploadImageAsync(fileMock));
    }
}

public class MockBrowserFile : IBrowserFile
{
    public MockBrowserFile(string name, string contentType)
    {
        Name = name;
        ContentType = contentType;
    }

    public string Name { get; }

    public DateTimeOffset LastModified => DateTimeOffset.Now;

    public long Size => 1024;

    public string ContentType { get; }

    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
    {
        return new MemoryStream(new byte[0]);
    }
}

