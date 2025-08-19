namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared;

using DepositLicenseeAPI.Features.Shared;
using Logging;

public class ILoggerExtensionsTests
{
    private readonly Mock<ILogger> _mockLogger;

    public ILoggerExtensionsTests()
    {
        _mockLogger = new Mock<ILogger>();
    }

    [Fact]
    public async Task LogErrExtraInfo_WithValidParameters_CallsLoggerErrorAsync()
    {
        // Arrange
        var errExtraInfo = "Test extra error information";
        var settingKey = "TestSettingKey";
        var logger = _mockLogger.Object;

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        _mockLogger.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task LogErrExtraInfo_WithValidParameters_LogsCorrectMessage()
    {
        // Arrange
        var errExtraInfo = "Test extra error information";
        var settingKey = "TestSettingKey";
        var logger = _mockLogger.Object;
        string capturedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        capturedMessage.Should().NotBeNull();
        capturedMessage.Should().Contain($"ErrExtraInfo: {errExtraInfo}");
        capturedMessage.Should().Contain($"SettingKey: {settingKey}");
    }

    [Fact]
    public async Task LogErrExtraInfo_WithNullErrExtraInfo_LogsNullValue()
    {
        // Arrange
        string errExtraInfo = null;
        var settingKey = "TestSettingKey";
        var logger = _mockLogger.Object;
        string capturedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        capturedMessage.Should().Contain("ErrExtraInfo:");
        capturedMessage.Should().Contain($"SettingKey: {settingKey}");
    }

    [Fact]
    public async Task LogErrExtraInfo_WithNullSettingKey_LogsNullValue()
    {
        // Arrange
        var errExtraInfo = "Test extra error information";
        string settingKey = null;
        var logger = _mockLogger.Object;
        string capturedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        capturedMessage.Should().Contain($"ErrExtraInfo: {errExtraInfo}");
        capturedMessage.Should().Contain("SettingKey:");
    }

    [Fact]
    public async Task LogErrExtraInfo_WithEmptyStrings_LogsEmptyValues()
    {
        // Arrange
        var errExtraInfo = "";
        var settingKey = "";
        var logger = _mockLogger.Object;
        string capturedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        capturedMessage.Should().Contain("ErrExtraInfo:");
        capturedMessage.Should().Contain("SettingKey:");
    }

    [Fact]
    public async Task LogErrExtraInfo_WithSpecialCharacters_HandlesCorrectly()
    {
        // Arrange
        var errExtraInfo = "Error with special chars: !@#$%^&*(){}[]|\\:;\"'<>,.?/~`";
        var settingKey = "Key_with-special.chars123";
        var logger = _mockLogger.Object;
        string capturedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        capturedMessage.Should().Contain($"ErrExtraInfo: {errExtraInfo}");
        capturedMessage.Should().Contain($"SettingKey: {settingKey}");
    }

    [Fact]
    public async Task LogErrExtraInfo_WithLongStrings_HandlesCorrectly()
    {
        // Arrange
        var errExtraInfo = new string('A', 1000);
        var settingKey = new string('B', 500);
        var logger = _mockLogger.Object;
        string capturedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        capturedMessage.Should().Contain($"ErrExtraInfo: {errExtraInfo}");
        capturedMessage.Should().Contain($"SettingKey: {settingKey}");
    }

    [Fact]
    public async Task LogErrExtraInfo_MessageFormat_IsCorrect()
    {
        // Arrange
        var errExtraInfo = "Test error";
        var settingKey = "TestKey";
        var logger = _mockLogger.Object;
        string capturedMessage = null;

        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Callback<string>(msg => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        // Act
        await logger.LogErrExtraInfo(errExtraInfo, settingKey);

        // Assert
        var lines = capturedMessage.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        lines.Should().HaveCount(2);
        lines[0].Should().Be($"ErrExtraInfo: {errExtraInfo}");
        lines[1].Should().Be($"SettingKey: {settingKey}");
    }

    [Fact]
    public async Task LogErrExtraInfo_ReturnsTask_ThatCompletes()
    {
        // Arrange
        var logger = _mockLogger.Object;
        _mockLogger.Setup(x => x.ErrorAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var task = logger.LogErrExtraInfo("test", "key");

        // Assert
        task.Should().NotBeNull();
        await task;
        task.IsCompleted.Should().BeTrue();
    }
}