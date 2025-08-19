namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared;

using DepositLicenseeAPI.Features.Shared;

public class ResultTests
{
    [Fact]
    public void Constructor_WithErrorCodeAndMessage_SetsPropertiesCorrectly()
    {
        // Arrange
        var errorCode = 404;
        var errorMessage = "Not found";

        // Act
        var result = new Result(errorCode, errorMessage);

        // Assert
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void IsOK_WhenErrorCodeIsZero_ReturnsTrue()
    {
        // Arrange
        var result = new Result(0, "Success");

        // Act & Assert
        result.IsOK.Should().BeTrue();
    }

    [Fact]
    public void IsOK_WhenErrorCodeIsNotZero_ReturnsFalse()
    {
        // Arrange
        var result = new Result(500, "Internal error");

        // Act & Assert
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public void IsNotOK_WhenErrorCodeIsZero_ReturnsFalse()
    {
        // Arrange
        var result = new Result(0, "Success");

        // Act & Assert
        result.IsNotOK.Should().BeFalse();
    }

    [Fact]
    public void IsNotOK_WhenErrorCodeIsNotZero_ReturnsTrue()
    {
        // Arrange
        var result = new Result(400, "Bad request");

        // Act & Assert
        result.IsNotOK.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(-1, false)]
    [InlineData(404, false)]
    [InlineData(500, false)]
    public void IsOK_WithVariousErrorCodes_ReturnsExpectedResult(int errorCode, bool expectedIsOK)
    {
        // Arrange
        var result = new Result(errorCode, "Test message");

        // Act & Assert
        result.IsOK.Should().Be(expectedIsOK);
        result.IsNotOK.Should().Be(!expectedIsOK);
    }

    [Fact]
    public void ErrorCode_IsImmutable()
    {
        // Arrange
        var result = new Result(100, "Test");

        // Act & Assert
        result.ErrorCode.Should().Be(100);
    }

    [Fact]
    public void ErrorMessage_IsImmutable()
    {
        // Arrange
        var errorMessage = "Test error message";
        var result = new Result(100, errorMessage);

        // Act & Assert
        result.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void Constructor_WithNullErrorMessage_SetsErrorMessageToNull()
    {
        // Arrange & Act
        var result = new Result(404, null);

        // Assert
        result.ErrorCode.Should().Be(404);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithEmptyErrorMessage_SetsErrorMessageToEmpty()
    {
        // Arrange & Act
        var result = new Result(400, string.Empty);

        // Assert
        result.ErrorCode.Should().Be(400);
        result.ErrorMessage.Should().BeEmpty();
    }
}