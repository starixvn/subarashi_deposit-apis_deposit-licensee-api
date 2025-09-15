namespace Fanex.DepositLicenseeAPI.Tests.Features.UserLog;

using DepositLicenseeAPI.Features.UserLog;

public class LogParamTests
{
    [Fact]
    public void LogParam_DefaultConstructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var logParam = new LogParam();

        // Assert
        logParam.Should().NotBeNull();
        logParam.UserId.Should().Be(0);
        logParam.CustId.Should().Be(0);
        logParam.ActionId.Should().Be(0);
        logParam.TrackingValue.Should().BeNull();
        logParam.TrackingValue2.Should().BeNull();
    }

    [Fact]
    public void LogParam_WithInitializer_SetsPropertiesCorrectly()
    {
        // Arrange
        var userId = 123;
        var custId = 456;
        var actionId = 789;
        var trackingValue = "TestTrackingValue";
        var trackingValue2 = "TestTrackingValue2";

        // Act
        var logParam = new LogParam
        {
            UserId = userId,
            CustId = custId,
            ActionId = actionId,
            TrackingValue = trackingValue,
            TrackingValue2 = trackingValue2
        };

        // Assert
        logParam.UserId.Should().Be(userId);
        logParam.CustId.Should().Be(custId);
        logParam.ActionId.Should().Be(actionId);
        logParam.TrackingValue.Should().Be(trackingValue);
        logParam.TrackingValue2.Should().Be(trackingValue2);
    }

    [Theory]
    [InlineData(1, 2, 3, "Value1", "Value2")]
    [InlineData(100, 200, 300, "TrackingA", "TrackingB")]
    [InlineData(0, 0, 0, null, null)]
    [InlineData(-1, -2, -3, "", "")]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, "MaxValue", "MaxValue2")]
    public void LogParam_WithVariousValues_SetsPropertiesCorrectly(int userId, int custId, int actionId, string trackingValue, string trackingValue2)
    {
        // Arrange & Act
        var logParam = new LogParam
        {
            UserId = userId,
            CustId = custId,
            ActionId = actionId,
            TrackingValue = trackingValue,
            TrackingValue2 = trackingValue2
        };

        // Assert
        logParam.UserId.Should().Be(userId);
        logParam.CustId.Should().Be(custId);
        logParam.ActionId.Should().Be(actionId);
        logParam.TrackingValue.Should().Be(trackingValue);
        logParam.TrackingValue2.Should().Be(trackingValue2);
    }

    [Fact]
    public void LogParam_IsRecord_SupportsValueEquality()
    {
        // Arrange
        var logParam1 = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        var logParam2 = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        // Act & Assert
        logParam1.Should().Be(logParam2);
        logParam1.Equals(logParam2).Should().BeTrue();
        (logParam1 == logParam2).Should().BeTrue();
        logParam1.GetHashCode().Should().Be(logParam2.GetHashCode());
    }

    [Fact]
    public void LogParam_DifferentValues_AreNotEqual()
    {
        // Arrange
        var logParam1 = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        var logParam2 = new LogParam
        {
            UserId = 124, // Different UserId
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        // Act & Assert
        logParam1.Should().NotBe(logParam2);
        logParam1.Equals(logParam2).Should().BeFalse();
        (logParam1 == logParam2).Should().BeFalse();
    }

    [Fact]
    public void LogParam_WithOperator_CreatesNewInstanceWithModifications()
    {
        // Arrange
        var originalLogParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "OriginalValue",
            TrackingValue2 = "OriginalValue2"
        };

        // Act
        var modifiedLogParam = originalLogParam with { TrackingValue = "ModifiedValue" };

        // Assert
        modifiedLogParam.Should().NotBeSameAs(originalLogParam);
        modifiedLogParam.UserId.Should().Be(originalLogParam.UserId);
        modifiedLogParam.CustId.Should().Be(originalLogParam.CustId);
        modifiedLogParam.ActionId.Should().Be(originalLogParam.ActionId);
        modifiedLogParam.TrackingValue.Should().Be("ModifiedValue");
        modifiedLogParam.TrackingValue2.Should().Be(originalLogParam.TrackingValue2);

        // Original should remain unchanged
        originalLogParam.TrackingValue.Should().Be("OriginalValue");
    }

    [Fact]
    public void LogParam_PropertiesAreReadable()
    {
        // Arrange
        var logParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        // Act & Assert
        var userIdProperty = typeof(LogParam).GetProperty(nameof(LogParam.UserId));
        var custIdProperty = typeof(LogParam).GetProperty(nameof(LogParam.CustId));
        var actionIdProperty = typeof(LogParam).GetProperty(nameof(LogParam.ActionId));
        var trackingValueProperty = typeof(LogParam).GetProperty(nameof(LogParam.TrackingValue));
        var trackingValue2Property = typeof(LogParam).GetProperty(nameof(LogParam.TrackingValue2));

        userIdProperty.CanRead.Should().BeTrue();
        custIdProperty.CanRead.Should().BeTrue();
        actionIdProperty.CanRead.Should().BeTrue();
        trackingValueProperty.CanRead.Should().BeTrue();
        trackingValue2Property.CanRead.Should().BeTrue();

        // Properties should have both getter and init setter
        userIdProperty.Should().NotBeNull();
        custIdProperty.Should().NotBeNull();
        actionIdProperty.Should().NotBeNull();
        trackingValueProperty.Should().NotBeNull();
        trackingValue2Property.Should().NotBeNull();
    }

    [Fact]
    public void LogParam_ToString_ReturnsFormattedString()
    {
        // Arrange
        var logParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        // Act
        var result = logParam.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("123");
        result.Should().Contain("456");
        result.Should().Contain("789");
        result.Should().Contain("Value1");
        result.Should().Contain("Value2");
    }

    [Fact]
    public void LogParam_WithNullStrings_HandlesCorrectly()
    {
        // Arrange & Act
        var logParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = null,
            TrackingValue2 = null
        };

        // Assert
        logParam.TrackingValue.Should().BeNull();
        logParam.TrackingValue2.Should().BeNull();
        logParam.UserId.Should().Be(123);
        logParam.CustId.Should().Be(456);
        logParam.ActionId.Should().Be(789);
    }

    [Fact]
    public void LogParam_WithEmptyStrings_HandlesCorrectly()
    {
        // Arrange & Act
        var logParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "",
            TrackingValue2 = ""
        };

        // Assert
        logParam.TrackingValue.Should().Be("");
        logParam.TrackingValue2.Should().Be("");
        logParam.UserId.Should().Be(123);
        logParam.CustId.Should().Be(456);
        logParam.ActionId.Should().Be(789);
    }

    [Fact]
    public void LogParam_WithWhitespaceStrings_HandlesCorrectly()
    {
        // Arrange & Act
        var logParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "   ",
            TrackingValue2 = "\t\n"
        };

        // Assert
        logParam.TrackingValue.Should().Be("   ");
        logParam.TrackingValue2.Should().Be("\t\n");
        logParam.UserId.Should().Be(123);
        logParam.CustId.Should().Be(456);
        logParam.ActionId.Should().Be(789);
    }

    [Fact]
    public void LogParam_SupportsRecordSemantics()
    {
        // Arrange
        var logParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        // Act & Assert
        logParam.UserId.Should().Be(123);
        logParam.CustId.Should().Be(456);
        logParam.ActionId.Should().Be(789);
        logParam.TrackingValue.Should().Be("Value1");
        logParam.TrackingValue2.Should().Be("Value2");
    }

    [Fact]
    public void LogParam_CopyConstructor_CreatesExactCopy()
    {
        // Arrange
        var originalLogParam = new LogParam
        {
            UserId = 123,
            CustId = 456,
            ActionId = 789,
            TrackingValue = "Value1",
            TrackingValue2 = "Value2"
        };

        // Act
        var copiedLogParam = originalLogParam with { };

        // Assert
        copiedLogParam.Should().NotBeSameAs(originalLogParam);
        copiedLogParam.Should().Be(originalLogParam);
        copiedLogParam.UserId.Should().Be(originalLogParam.UserId);
        copiedLogParam.CustId.Should().Be(originalLogParam.CustId);
        copiedLogParam.ActionId.Should().Be(originalLogParam.ActionId);
        copiedLogParam.TrackingValue.Should().Be(originalLogParam.TrackingValue);
        copiedLogParam.TrackingValue2.Should().Be(originalLogParam.TrackingValue2);
    }
}