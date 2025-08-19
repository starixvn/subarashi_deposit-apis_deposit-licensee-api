namespace Fanex.DepositLicenseeAPI.Tests.Features.UserLog;

using Data.Repository;
using DepositLicenseeAPI.Features.UserLog;

public class InsertUserLogMainCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var userId = 123;
        var custId = 456;
        var actionId = 789;
        var trackingValue = "TrackingValue";
        var trackingValue2 = "TrackingValue2";

        // Act
        var command = new InsertUserLogMainCommand(userId, custId, actionId, trackingValue, trackingValue2);

        // Assert
        command.Should().NotBeNull();
        command.UserId.Should().Be(userId);
        command.CustId.Should().Be(custId);
        command.ActionId.Should().Be(actionId);
        command.TrackingValue.Should().Be(trackingValue);
        command.TrackingValue2.Should().Be(trackingValue2);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var command = new InsertUserLogMainCommand(123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("Main_WriteLog_UserLog_RunSP");
    }

    [Fact]
    public void IsValid_WithAllValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new InsertUserLogMainCommand(123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 456, 789, "TrackingValue", false)]
    [InlineData(-1, 456, 789, "TrackingValue", false)]
    [InlineData(123, 0, 789, "TrackingValue", false)]
    [InlineData(123, -1, 789, "TrackingValue", false)]
    [InlineData(123, 456, 0, "TrackingValue", false)]
    [InlineData(123, 456, -1, "TrackingValue", false)]
    [InlineData(123, 456, 789, null, false)]
    [InlineData(123, 456, 789, "", false)]
    [InlineData(123, 456, 789, "   ", false)]
    public void IsValid_WithInvalidParameters_ReturnsFalse(int userId, int custId, int actionId, string trackingValue, bool expected)
    {
        // Arrange
        var command = new InsertUserLogMainCommand(userId, custId, actionId, trackingValue, "TrackingValue2");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(1, 1, 1, "Valid", true)]
    [InlineData(100, 200, 300, "Valid", true)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, "Valid", true)]
    public void IsValid_WithValidParameters_ReturnsTrue(int userId, int custId, int actionId, string trackingValue, bool expected)
    {
        // Arrange
        var command = new InsertUserLogMainCommand(userId, custId, actionId, trackingValue, "TrackingValue2");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromInsertUserLogBaseCommand()
    {
        // Arrange
        var command = new InsertUserLogMainCommand(123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act & Assert
        command.Should().BeAssignableTo<InsertUserLogBaseCommand>();
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertUserLogMainCommand(123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }
}

public class InsertUserLogSiteDBCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var custDBGroupId = 111;
        var userId = 123;
        var custId = 456;
        var actionId = 789;
        var trackingValue = "TrackingValue";
        var trackingValue2 = "TrackingValue2";

        // Act
        var command = new InsertUserLogSiteDBCommand(custDBGroupId, userId, custId, actionId, trackingValue, trackingValue2);

        // Assert
        command.Should().NotBeNull();
        command.CustDBGroupId.Should().Be(custDBGroupId);
        command.UserId.Should().Be(userId);
        command.CustId.Should().Be(custId);
        command.ActionId.Should().Be(actionId);
        command.TrackingValue.Should().Be(trackingValue);
        command.TrackingValue2.Should().Be(trackingValue2);
    }

    [Fact]
    public void CustDBGroupId_IsReadOnly()
    {
        // Arrange
        var command = new InsertUserLogSiteDBCommand(111, 123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act & Assert
        var property = typeof(InsertUserLogSiteDBCommand).GetProperty(nameof(InsertUserLogSiteDBCommand.CustDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var custDBGroupId = 123;
        var command = new InsertUserLogSiteDBCommand(custDBGroupId, 111, 456, 789, "TrackingValue", "TrackingValue2");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be($"SiteDB{custDBGroupId}_WriteLog_UserLog_RunSP");
    }

    [Fact]
    public void GetSettingKey_WithZeroCustDBGroupId_ReturnsBaseName()
    {
        // Arrange
        var custDBGroupId = 0;
        var command = new InsertUserLogSiteDBCommand(custDBGroupId, 111, 456, 789, "TrackingValue", "TrackingValue2");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("WriteLog_UserLog_RunSP");
    }

    [Fact]
    public void IsValid_WithAllValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new InsertUserLogSiteDBCommand(111, 123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 456, 789, "TrackingValue", false)]
    [InlineData(-1, 456, 789, "TrackingValue", false)]
    [InlineData(123, 0, 789, "TrackingValue", false)]
    [InlineData(123, -1, 789, "TrackingValue", false)]
    [InlineData(123, 456, 0, "TrackingValue", false)]
    [InlineData(123, 456, -1, "TrackingValue", false)]
    [InlineData(123, 456, 789, null, false)]
    [InlineData(123, 456, 789, "", false)]
    [InlineData(123, 456, 789, "   ", false)]
    public void IsValid_WithInvalidParameters_ReturnsFalse(int userId, int custId, int actionId, string trackingValue, bool expected)
    {
        // Arrange
        var command = new InsertUserLogSiteDBCommand(111, userId, custId, actionId, trackingValue, "TrackingValue2");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromInsertUserLogBaseCommand()
    {
        // Arrange
        var command = new InsertUserLogSiteDBCommand(111, 123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act & Assert
        command.Should().BeAssignableTo<InsertUserLogBaseCommand>();
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertUserLogSiteDBCommand(111, 123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(-50)]
    public void Constructor_WithVariousCustDBGroupIds_SetsPropertyCorrectly(int custDBGroupId)
    {
        // Arrange & Act
        var command = new InsertUserLogSiteDBCommand(custDBGroupId, 123, 456, 789, "TrackingValue", "TrackingValue2");

        // Assert
        command.CustDBGroupId.Should().Be(custDBGroupId);
    }
}

public class InsertUserLogBaseCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var userId = 123;
        var custId = 456;
        var actionId = 789;
        var trackingValue = "TrackingValue";
        var trackingValue2 = "TrackingValue2";

        // Act
        var command = new TestInsertUserLogBaseCommand(userId, custId, actionId, trackingValue, trackingValue2);

        // Assert
        command.Should().NotBeNull();
        command.UserId.Should().Be(userId);
        command.CustId.Should().Be(custId);
        command.ActionId.Should().Be(actionId);
        command.TrackingValue.Should().Be(trackingValue);
        command.TrackingValue2.Should().Be(trackingValue2);
    }

    [Fact]
    public void Properties_AreReadOnly()
    {
        // Arrange
        var command = new TestInsertUserLogBaseCommand(123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act & Assert
        var userIdProperty = typeof(InsertUserLogBaseCommand).GetProperty(nameof(InsertUserLogBaseCommand.UserId));
        var custIdProperty = typeof(InsertUserLogBaseCommand).GetProperty(nameof(InsertUserLogBaseCommand.CustId));
        var actionIdProperty = typeof(InsertUserLogBaseCommand).GetProperty(nameof(InsertUserLogBaseCommand.ActionId));
        var trackingValueProperty = typeof(InsertUserLogBaseCommand).GetProperty(nameof(InsertUserLogBaseCommand.TrackingValue));
        var trackingValue2Property = typeof(InsertUserLogBaseCommand).GetProperty(nameof(InsertUserLogBaseCommand.TrackingValue2));

        userIdProperty.CanWrite.Should().BeFalse();
        custIdProperty.CanWrite.Should().BeFalse();
        actionIdProperty.CanWrite.Should().BeFalse();
        trackingValueProperty.CanWrite.Should().BeFalse();
        trackingValue2Property.CanWrite.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithAllValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new TestInsertUserLogBaseCommand(123, 456, 789, "TrackingValue", "TrackingValue2");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidationLogic_RequiresAllParametersToBePositiveAndTrackingValueNotEmpty()
    {
        // Arrange & Act & Assert
        new TestInsertUserLogBaseCommand(1, 1, 1, "Valid", "Valid").IsValid().Should().BeTrue();
        new TestInsertUserLogBaseCommand(100, 200, 300, "Valid", null).IsValid().Should().BeTrue();
        new TestInsertUserLogBaseCommand(100, 200, 300, "Valid", "").IsValid().Should().BeTrue();

        new TestInsertUserLogBaseCommand(0, 1, 1, "Valid", "Valid").IsValid().Should().BeFalse();
        new TestInsertUserLogBaseCommand(1, 0, 1, "Valid", "Valid").IsValid().Should().BeFalse();
        new TestInsertUserLogBaseCommand(1, 1, 0, "Valid", "Valid").IsValid().Should().BeFalse();
        new TestInsertUserLogBaseCommand(1, 1, 1, null, "Valid").IsValid().Should().BeFalse();
        new TestInsertUserLogBaseCommand(1, 1, 1, "", "Valid").IsValid().Should().BeFalse();
        new TestInsertUserLogBaseCommand(1, 1, 1, "   ", "Valid").IsValid().Should().BeFalse();
    }

    private class TestInsertUserLogBaseCommand : InsertUserLogBaseCommand
    {
        public TestInsertUserLogBaseCommand(int userId, int custId, int actionId, string trackingValue, string trackingValue2)
            : base(userId, custId, actionId, trackingValue, trackingValue2)
        {
        }

        public override string GetSettingKey() => "TestSettingKey";
    }
}