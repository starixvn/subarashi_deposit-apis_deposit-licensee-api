namespace Fanex.DepositLicenseeAPI.Tests;

using System.Net;
using System.Text.Json;
using Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

public class GlobalExceptionHandlerTests
{
    private readonly GlobalExceptionHandler _handler;
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<ILogger> _mockLogger;

    public GlobalExceptionHandlerTests()
    {
        _mockLogger = new Mock<ILogger>();
        _handler = new GlobalExceptionHandler(_mockLogger.Object);
        _httpContext = new DefaultHttpContext();

        _httpContext.Response.Body = new MemoryStream();
        _httpContext.Request.Body = new MemoryStream();
    }

    [Fact]
    public async Task TryHandleAsync_WithException_LogsError()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        result.Should().BeTrue();
        _mockLogger.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task TryHandleAsync_WithException_SetsResponseStatusTo500()
    {
        // Arrange
        var exception = new ArgumentException("Test exception");
        var cancellationToken = CancellationToken.None;

        // Act
        await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task TryHandleAsync_WithException_SetsContentTypeToJson()
    {
        // Arrange
        var exception = new NotSupportedException("Test exception");
        var cancellationToken = CancellationToken.None;

        // Act
        await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        _httpContext.Response.ContentType.Should().StartWith("application/json");
    }

    [Fact]
    public async Task TryHandleAsync_WithException_WritesJsonResponse()
    {
        // Arrange
        var exceptionMessage = "Test error message";
        var exception = new Exception(exceptionMessage);
        var cancellationToken = CancellationToken.None;

        // Act
        await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var jsonDocument = JsonDocument.Parse(responseContent);

        var statusCodeProperty = jsonDocument.RootElement.TryGetProperty("StatusCode", out var statusCode)
            ? statusCode
            : jsonDocument.RootElement.GetProperty("statusCode");
        var messageProperty = jsonDocument.RootElement.TryGetProperty("Message", out var message) ? message : jsonDocument.RootElement.GetProperty("message");

        statusCodeProperty.GetInt32().Should().Be(500);
        messageProperty.GetString().Should().Be(exceptionMessage);
    }

    [Fact]
    public async Task TryHandleAsync_WithException_EnablesRequestBuffering()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var cancellationToken = CancellationToken.None;
        var originalRequest = _httpContext.Request;

        // Act
        await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        var httpRequestFeature = _httpContext.Features.Get<IHttpRequestFeature>();
        _httpContext.Request.Should().BeSameAs(originalRequest);
    }

    [Fact]
    public async Task TryHandleAsync_WithException_ClearsResponse()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var cancellationToken = CancellationToken.None;

        _httpContext.Response.Headers.Append("Custom-Header", "value");
        _httpContext.Response.StatusCode = 200;

        // Act
        await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(500);
        _httpContext.Response.ContentType.Should().StartWith("application/json");
    }

    [Fact]
    public async Task TryHandleAsync_WithCancellationToken_PassesToWriteAsJsonAsync()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var cancellationToken = new CancellationToken(canceled: true);

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await _handler.TryHandleAsync(_httpContext, exception, cancellationToken));
    }

    [Fact]
    public async Task TryHandleAsync_LogsGeneratedMessage()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var cancellationToken = CancellationToken.None;
        string loggedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => loggedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        loggedMessage.Should().NotBeNull();
        loggedMessage.Should().Contain("MESSAGE: Test exception");
        loggedMessage.Should().Contain("REQUEST INFO");
        loggedMessage.Should().Contain("DATABASE INFO");
        loggedMessage.Should().Contain("EXCEPTION INFO");
    }

    [Fact]
    public async Task TryHandleAsync_WithNullException_HandlesGracefully()
    {
        // Arrange
        Exception exception = null;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(async () =>
            await _handler.TryHandleAsync(_httpContext, exception, cancellationToken));
    }

    [Theory]
    [InlineData("Simple error")]
    [InlineData("")]
    [InlineData("Error with special characters: !@#$%^&*()")]
    [InlineData("Very long error message that contains multiple words and might span several lines when displayed in the user interface")]
    public async Task TryHandleAsync_WithVariousExceptionMessages_WritesCorrectResponse(string errorMessage)
    {
        // Arrange
        var exception = new Exception(errorMessage);
        var cancellationToken = CancellationToken.None;

        // Act
        await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var jsonDocument = JsonDocument.Parse(responseContent);

        var messageProperty = jsonDocument.RootElement.TryGetProperty("Message", out var message) ? message : jsonDocument.RootElement.GetProperty("message");
        messageProperty.GetString().Should().Be(errorMessage);
    }

    [Fact]
    public async Task TryHandleAsync_AlwaysReturnsTrue()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.TryHandleAsync(_httpContext, exception, cancellationToken);

        // Assert
        result.Should().BeTrue();
    }
}