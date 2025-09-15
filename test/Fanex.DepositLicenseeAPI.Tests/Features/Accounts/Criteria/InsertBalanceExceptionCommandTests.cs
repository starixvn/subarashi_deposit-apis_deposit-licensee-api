namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Accounts.Criteria;

public class InsertBalanceExceptionCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var siteDBGroupId = 123;
        var custId = 456;
        var username = "testuser";

        // Act
        var command = new InsertBalanceExceptionCommand(siteDBGroupId, custId, username);

        // Assert
        command.Should().NotBeNull();
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
        command.CustId.Should().Be(custId);
        command.Username.Should().Be(username);
    }

    [Fact]
    public void SiteDBGroupId_IsReadOnly()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "testuser");

        // Act & Assert
        var property = typeof(InsertBalanceExceptionCommand).GetProperty(nameof(InsertBalanceExceptionCommand.SiteDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void CustId_IsReadOnly()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "testuser");

        // Act & Assert
        var property = typeof(InsertBalanceExceptionCommand).GetProperty(nameof(InsertBalanceExceptionCommand.CustId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void Username_IsReadOnly()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "testuser");

        // Act & Assert
        var property = typeof(InsertBalanceExceptionCommand).GetProperty(nameof(InsertBalanceExceptionCommand.Username));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var siteDBGroupId = 123;
        var command = new InsertBalanceExceptionCommand(siteDBGroupId, 456, "testuser");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be($"SiteDB{siteDBGroupId}_Task_Cust_CheckBalance_Exception_Ins");
    }

    [Fact]
    public void GetSettingKey_WithZeroSiteDBGroupId_ReturnsBaseName()
    {
        // Arrange
        var siteDBGroupId = 0;
        var command = new InsertBalanceExceptionCommand(siteDBGroupId, 456, "testuser");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("Task_Cust_CheckBalance_Exception_Ins");
    }

    [Fact]
    public void IsValid_WithValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "testuser");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithZeroCustId_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 0, "testuser");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativeCustId_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, -1, "testuser");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullUsername_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, null);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyUsername_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceUsername_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "   ");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, "user1", true)]
    [InlineData(100, "user100", true)]
    [InlineData(999, "testuser", true)]
    [InlineData(int.MaxValue, "maxuser", true)]
    public void IsValid_WithValidCombinations_ReturnsTrue(int custId, string username, bool expected)
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, custId, username);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, "user", false)]
    [InlineData(-1, "user", false)]
    [InlineData(123, null, false)]
    [InlineData(123, "", false)]
    [InlineData(123, "   ", false)]
    [InlineData(0, null, false)]
    [InlineData(-1, "", false)]
    public void IsValid_WithInvalidCombinations_ReturnsFalse(int custId, string username, bool expected)
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, custId, username);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "testuser");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var siteDBGroupId1 = 111;
        var custId1 = 222;
        var username1 = "user1";
        var siteDBGroupId2 = 333;
        var custId2 = 444;
        var username2 = "user2";

        // Act
        var command1 = new InsertBalanceExceptionCommand(siteDBGroupId1, custId1, username1);
        var command2 = new InsertBalanceExceptionCommand(siteDBGroupId2, custId2, username2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.SiteDBGroupId.Should().Be(siteDBGroupId1);
        command1.CustId.Should().Be(custId1);
        command1.Username.Should().Be(username1);
        command2.SiteDBGroupId.Should().Be(siteDBGroupId2);
        command2.CustId.Should().Be(custId2);
        command2.Username.Should().Be(username2);
    }

    [Fact]
    public void ValidationLogic_RequiresBothCustIdAndUsername()
    {
        // Arrange & Act & Assert
        new InsertBalanceExceptionCommand(123, 1, "valid").IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand(123, 100, "valid").IsValid().Should().BeTrue();

        new InsertBalanceExceptionCommand(123, 0, "valid").IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand(123, -1, "valid").IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand(123, 100, null).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand(123, 100, "").IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand(123, 100, "   ").IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesValidationCondition()
    {
        // Arrange
        var validCommand = new InsertBalanceExceptionCommand(123, 42, "validuser");
        var invalidCustIdCommand = new InsertBalanceExceptionCommand(123, 0, "validuser");
        var invalidUsernameCommand = new InsertBalanceExceptionCommand(123, 42, null);
        var bothInvalidCommand = new InsertBalanceExceptionCommand(123, 0, "");

        // Act & Assert
        validCommand.IsValid().Should().Be(validCommand.CustId > 0 && !string.IsNullOrWhiteSpace(validCommand.Username));
        invalidCustIdCommand.IsValid().Should().Be(invalidCustIdCommand.CustId > 0 && !string.IsNullOrWhiteSpace(invalidCustIdCommand.Username));
        invalidUsernameCommand.IsValid().Should().Be(invalidUsernameCommand.CustId > 0 && !string.IsNullOrWhiteSpace(invalidUsernameCommand.Username));
        bothInvalidCommand.IsValid().Should().Be(bothInvalidCommand.CustId > 0 && !string.IsNullOrWhiteSpace(bothInvalidCommand.Username));
    }

    [Theory]
    [InlineData(1, 1, "user1")]
    [InlineData(100, 200, "user200")]
    [InlineData(0, 300, "user300")]
    [InlineData(-50, 400, "user400")]
    [InlineData(999, -100, "user-100")]
    public void Constructor_AcceptsVariousParameterValues(int siteDBGroupId, int custId, string username)
    {
        // Arrange & Act
        var command = new InsertBalanceExceptionCommand(siteDBGroupId, custId, username);

        // Assert
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
        command.CustId.Should().Be(custId);
        command.Username.Should().Be(username);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(-50)]
    public void GetSettingKey_UsesSiteDBHelpers(int siteDBGroupId)
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(siteDBGroupId, 456, "testuser");

        // Act
        var result = command.GetSettingKey();

        // Assert
        if (siteDBGroupId == 0)
        {
            result.Should().Be("Task_Cust_CheckBalance_Exception_Ins");
        }
        else
        {
            result.Should().Be($"SiteDB{siteDBGroupId}_Task_Cust_CheckBalance_Exception_Ins");
        }
    }

    [Fact]
    public void GetSettingKey_IsConsistentForSameInstance()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "testuser");

        // Act
        var result1 = command.GetSettingKey();
        var result2 = command.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("SiteDB123_Task_Cust_CheckBalance_Exception_Ins");
    }

    [Fact]
    public void ValidationBoundaryConditions_AreTested()
    {
        // Arrange & Act & Assert
        // Test CustId boundary at zero
        new InsertBalanceExceptionCommand(123, 0, "user").IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand(123, 1, "user").IsValid().Should().BeTrue();

        // Test negative CustId boundary
        new InsertBalanceExceptionCommand(123, -1, "user").IsValid().Should().BeFalse();

        // Test extreme CustId values
        new InsertBalanceExceptionCommand(123, int.MaxValue, "user").IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand(123, int.MinValue, "user").IsValid().Should().BeFalse();

        // Test username edge cases
        new InsertBalanceExceptionCommand(123, 100, "a").IsValid().Should().BeTrue(); // Single character
        new InsertBalanceExceptionCommand(123, 100, " a ").IsValid().Should().BeTrue(); // Whitespace around valid content
    }

    [Fact]
    public void Command_HasNoMutableState()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(123, 456, "testuser");
        var initialSiteDBGroupId = command.SiteDBGroupId;
        var initialCustId = command.CustId;
        var initialUsername = command.Username;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var siteDBGroupId1 = command.SiteDBGroupId;
        var custId1 = command.CustId;
        var username1 = command.Username;
        var settingKey1 = command.GetSettingKey();
        var valid1 = command.IsValid();

        // Assert - All calls should return same values
        siteDBGroupId1.Should().Be(initialSiteDBGroupId);
        custId1.Should().Be(initialCustId);
        username1.Should().Be(initialUsername);
        settingKey1.Should().Be(initialSettingKey);
        valid1.Should().Be(initialValid);
    }
}