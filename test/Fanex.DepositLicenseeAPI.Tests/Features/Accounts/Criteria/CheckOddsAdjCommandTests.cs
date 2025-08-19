namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Accounts.Criteria;

public class CheckOddsAdjCommandTests
{
    [Fact]
    public void Constructor_WithValidSiteId_InitializesCorrectly()
    {
        // Arrange
        var siteId = "TestSite123";

        // Act
        var command = new CheckOddsAdjCommand(siteId);

        // Assert
        command.Should().NotBeNull();
        command.SiteId.Should().Be(siteId);
    }

    [Fact]
    public void SiteId_IsReadOnly()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act & Assert
        var property = typeof(CheckOddsAdjCommand).GetProperty(nameof(CheckOddsAdjCommand.SiteId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void ErrCode_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");
        var expectedErrCode = 500;

        // Act
        command.ErrCode = expectedErrCode;

        // Assert
        command.ErrCode.Should().Be(expectedErrCode);
    }

    [Fact]
    public void ErrMsg_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");
        var expectedErrMsg = "Test error message";

        // Act
        command.ErrMsg = expectedErrMsg;

        // Assert
        command.ErrMsg.Should().Be(expectedErrMsg);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_Dep_CustomerCheckOddsAdj");
    }

    [Fact]
    public void IsValid_AlwaysReturnsTrue()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Site1")]
    [InlineData("Site123")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("Very Long Site Name With Spaces")]
    [InlineData("SpecialChars@#$%^&*()")]
    public void IsValid_WithAnySiteId_AlwaysReturnsTrue(string siteId)
    {
        // Arrange
        var command = new CheckOddsAdjCommand(siteId);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MutableProperties_DefaultValues()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act & Assert
        command.ErrCode.Should().Be(0);
        command.ErrMsg.Should().BeNull();
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new CheckOddsAdjCommand("Site1");
        var command2 = new CheckOddsAdjCommand("Site2");

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("API_Dep_CustomerCheckOddsAdj");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var siteId1 = "Site1";
        var siteId2 = "Site2";

        // Act
        var command1 = new CheckOddsAdjCommand(siteId1);
        var command2 = new CheckOddsAdjCommand(siteId2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.SiteId.Should().Be(siteId1);
        command2.SiteId.Should().Be(siteId2);
        command1.SiteId.Should().NotBe(command2.SiteId);
    }

    [Theory]
    [InlineData("Site1")]
    [InlineData("Site2")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("Special!@#$%")]
    public void Constructor_AcceptsVariousSiteIdValues(string siteId)
    {
        // Arrange & Act
        var command = new CheckOddsAdjCommand(siteId);

        // Assert
        command.SiteId.Should().Be(siteId);
    }

    [Fact]
    public void ValidationLogic_AlwaysValid()
    {
        // Arrange
        var commands = new[]
        {
            new CheckOddsAdjCommand("ValidSite"),
            new CheckOddsAdjCommand(""),
            new CheckOddsAdjCommand(null),
            new CheckOddsAdjCommand("   "),
            new CheckOddsAdjCommand("123"),
            new CheckOddsAdjCommand("Special@Characters!")
        };

        // Act & Assert
        foreach (var command in commands)
        {
            command.IsValid().Should().BeTrue($"Command with SiteId '{command.SiteId}' should be valid");
        }
    }

    [Fact]
    public void GetSettingKey_IsConstant()
    {
        // Arrange
        var commands = new[]
        {
            new CheckOddsAdjCommand("Site1"),
            new CheckOddsAdjCommand("Site2"),
            new CheckOddsAdjCommand(""),
            new CheckOddsAdjCommand(null)
        };

        // Act
        var settingKeys = commands.Select(c => c.GetSettingKey()).ToArray();

        // Assert
        settingKeys.Should().AllBe("API_Dep_CustomerCheckOddsAdj");
        settingKeys.Should().HaveCount(4);
        settingKeys.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");
        var initialSiteId = command.SiteId;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var siteId1 = command.SiteId;
        var siteId2 = command.SiteId;
        var settingKey1 = command.GetSettingKey();
        var settingKey2 = command.GetSettingKey();
        var valid1 = command.IsValid();
        var valid2 = command.IsValid();

        // Assert - All calls should return same values
        siteId1.Should().Be(initialSiteId);
        siteId2.Should().Be(initialSiteId);
        siteId1.Should().Be(siteId2);

        settingKey1.Should().Be(initialSettingKey);
        settingKey2.Should().Be(initialSettingKey);
        settingKey1.Should().Be(settingKey2);

        valid1.Should().Be(initialValid);
        valid2.Should().Be(initialValid);
        valid1.Should().Be(valid2);

        // All should be consistent
        valid1.Should().BeTrue();
        valid2.Should().BeTrue();
    }

    [Fact]
    public void Command_BehaviorIsConsistentAcrossInstances()
    {
        // Arrange
        var siteId = "TestSite";
        var command1 = new CheckOddsAdjCommand(siteId);
        var command2 = new CheckOddsAdjCommand(siteId);

        // Act & Assert
        command1.SiteId.Should().Be(command2.SiteId);
        command1.GetSettingKey().Should().Be(command2.GetSettingKey());
        command1.IsValid().Should().Be(command2.IsValid());

        // They should behave identically but be different instances
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void MutableProperties_CanBeModifiedIndependently()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act
        command.ErrCode = 500;
        command.ErrMsg = "Error occurred";

        // Assert
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Error occurred");

        // Read-only properties should remain unchanged
        command.SiteId.Should().Be("TestSite");
        command.IsValid().Should().BeTrue();
        command.GetSettingKey().Should().Be("API_Dep_CustomerCheckOddsAdj");
    }

    [Fact]
    public void ErrCode_DefaultValue_IsZero()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act & Assert
        command.ErrCode.Should().Be(0);
    }

    [Fact]
    public void ErrMsg_DefaultValue_IsNull()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act & Assert
        command.ErrMsg.Should().BeNull();
    }

    [Fact]
    public void ValidationAlwaysTrue_RegarlessOfSiteIdValue()
    {
        // Arrange & Act & Assert
        new CheckOddsAdjCommand(null).IsValid().Should().BeTrue();
        new CheckOddsAdjCommand("").IsValid().Should().BeTrue();
        new CheckOddsAdjCommand("   ").IsValid().Should().BeTrue();
        new CheckOddsAdjCommand("valid").IsValid().Should().BeTrue();
        new CheckOddsAdjCommand("123").IsValid().Should().BeTrue();
        new CheckOddsAdjCommand("@#$%^&*()").IsValid().Should().BeTrue();

        // Validation logic always returns true regardless of input
        var testCases = new[] { null, "", "   ", "\t\n", "valid", "123", "special@chars!" };
        foreach (var testCase in testCases)
        {
            var command = new CheckOddsAdjCommand(testCase);
            command.IsValid().Should().BeTrue($"SiteId '{testCase}' should be valid");
        }
    }

    [Fact]
    public void GetSettingKey_NeverChanges()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act - Call multiple times and modify mutable properties
        var key1 = command.GetSettingKey();
        command.ErrCode = 500;
        var key2 = command.GetSettingKey();
        command.ErrMsg = "Error";
        var key3 = command.GetSettingKey();

        // Assert
        key1.Should().Be("API_Dep_CustomerCheckOddsAdj");
        key2.Should().Be("API_Dep_CustomerCheckOddsAdj");
        key3.Should().Be("API_Dep_CustomerCheckOddsAdj");
        key1.Should().Be(key2);
        key2.Should().Be(key3);
    }

    [Fact]
    public void AllMutableProperties_CanBeSetToNull()
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act
        command.ErrMsg = null;

        // Assert
        command.ErrMsg.Should().BeNull();
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(500)]
    [InlineData(int.MaxValue)]
    public void ErrCode_AcceptsVariousValues(int errCode)
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act
        command.ErrCode = errCode;

        // Assert
        command.ErrCode.Should().Be(errCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Error message")]
    [InlineData("   ")]
    [InlineData("Very long error message with special characters @#$%^&*()")]
    public void ErrMsg_AcceptsVariousValues(string errMsg)
    {
        // Arrange
        var command = new CheckOddsAdjCommand("TestSite");

        // Act
        command.ErrMsg = errMsg;

        // Assert
        command.ErrMsg.Should().Be(errMsg);
    }
}