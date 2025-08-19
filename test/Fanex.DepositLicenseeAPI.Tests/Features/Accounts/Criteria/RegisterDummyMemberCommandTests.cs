namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Accounts.Criteria;

public class RegisterDummyMemberCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var agentId = 123;
        var site = "TestSite";
        var siteDBGroupId = 456;

        // Act
        var command = new RegisterDummyMemberCommand(agentId, site, siteDBGroupId);

        // Assert
        command.Should().NotBeNull();
        command.AgentId.Should().Be(agentId);
        command.Site.Should().Be(site);
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Fact]
    public void AgentId_IsReadOnly()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act & Assert
        var property = typeof(RegisterDummyMemberCommand).GetProperty(nameof(RegisterDummyMemberCommand.AgentId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void Site_IsReadOnly()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act & Assert
        var property = typeof(RegisterDummyMemberCommand).GetProperty(nameof(RegisterDummyMemberCommand.Site));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void SiteDBGroupId_IsReadOnly()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act & Assert
        var property = typeof(RegisterDummyMemberCommand).GetProperty(nameof(RegisterDummyMemberCommand.SiteDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void MutableProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        command.ErrCode = 500;
        command.ErrMsg = "Test error message";
        command.Username = "testuser";
        command.CustId = 789;

        // Assert
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Test error message");
        command.Username.Should().Be("testuser");
        command.CustId.Should().Be(789);
    }

    [Fact]
    public void MutableProperties_DefaultValues()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act & Assert
        command.ErrCode.Should().Be(0);
        command.ErrMsg.Should().BeNull();
        command.Username.Should().BeNull();
        command.CustId.Should().Be(0);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_InsOddsAdj_Register");
    }

    [Fact]
    public void IsValid_WithPositiveAgentId_ReturnsTrue()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithZeroAgentId_ReturnsFalse()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(0, "TestSite", 456);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativeAgentId_ReturnsFalse()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(-1, "TestSite", 456);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(999, true)]
    [InlineData(int.MaxValue, true)]
    public void IsValid_WithPositiveAgentIds_ReturnsTrue(int agentId, bool expected)
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(agentId, "TestSite", 456);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(-100, false)]
    [InlineData(int.MinValue, false)]
    public void IsValid_WithNonPositiveAgentIds_ReturnsFalse(int agentId, bool expected)
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(agentId, "TestSite", 456);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new RegisterDummyMemberCommand(123, "Site1", 456);
        var command2 = new RegisterDummyMemberCommand(789, "Site2", 999);

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("API_InsOddsAdj_Register");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var agentId1 = 123;
        var site1 = "Site1";
        var siteDBGroupId1 = 456;
        var agentId2 = 789;
        var site2 = "Site2";
        var siteDBGroupId2 = 999;

        // Act
        var command1 = new RegisterDummyMemberCommand(agentId1, site1, siteDBGroupId1);
        var command2 = new RegisterDummyMemberCommand(agentId2, site2, siteDBGroupId2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.AgentId.Should().Be(agentId1);
        command1.Site.Should().Be(site1);
        command1.SiteDBGroupId.Should().Be(siteDBGroupId1);
        command2.AgentId.Should().Be(agentId2);
        command2.Site.Should().Be(site2);
        command2.SiteDBGroupId.Should().Be(siteDBGroupId2);
    }

    [Theory]
    [InlineData(1, "Site1", 100)]
    [InlineData(999, "Site999", 0)]
    [InlineData(123, "", -1)]
    [InlineData(456, null, int.MaxValue)]
    [InlineData(int.MaxValue, "MaxSite", int.MinValue)]
    public void Constructor_WithVariousParameters_InitializesCorrectly(int agentId, string site, int siteDBGroupId)
    {
        // Arrange & Act
        var command = new RegisterDummyMemberCommand(agentId, site, siteDBGroupId);

        // Assert
        command.AgentId.Should().Be(agentId);
        command.Site.Should().Be(site);
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Fact]
    public void ValidationLogic_OnlyDependsOnAgentId()
    {
        // Arrange & Act & Assert
        new RegisterDummyMemberCommand(1, "Site", 123).IsValid().Should().BeTrue();
        new RegisterDummyMemberCommand(100, null, -1).IsValid().Should().BeTrue();
        new RegisterDummyMemberCommand(999, "", 0).IsValid().Should().BeTrue();

        new RegisterDummyMemberCommand(0, "Site", 123).IsValid().Should().BeFalse();
        new RegisterDummyMemberCommand(-1, "Site", 123).IsValid().Should().BeFalse();
        new RegisterDummyMemberCommand(-100, "ValidSite", 999).IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesConditionAgentIdGreaterThanZero()
    {
        // Arrange
        var positiveCommand = new RegisterDummyMemberCommand(42, "TestSite", 123);
        var zeroCommand = new RegisterDummyMemberCommand(0, "TestSite", 123);
        var negativeCommand = new RegisterDummyMemberCommand(-42, "TestSite", 123);

        // Act & Assert
        positiveCommand.IsValid().Should().Be(positiveCommand.AgentId > 0);
        zeroCommand.IsValid().Should().Be(zeroCommand.AgentId > 0);
        negativeCommand.IsValid().Should().Be(negativeCommand.AgentId > 0);
    }

    [Fact]
    public void GetSettingKey_IsConstant()
    {
        // Arrange
        var commands = new[]
        {
            new RegisterDummyMemberCommand(1, "Site1", 100),
            new RegisterDummyMemberCommand(999, "Site2", 200),
            new RegisterDummyMemberCommand(0, "", -1),
            new RegisterDummyMemberCommand(-1, null, 0)
        };

        // Act
        var settingKeys = commands.Select(c => c.GetSettingKey()).ToArray();

        // Assert
        settingKeys.Should().AllBe("API_InsOddsAdj_Register");
        settingKeys.Should().HaveCount(4);
        settingKeys.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);
        var initialAgentId = command.AgentId;
        var initialSite = command.Site;
        var initialSiteDBGroupId = command.SiteDBGroupId;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var agentId1 = command.AgentId;
        var site1 = command.Site;
        var siteDBGroupId1 = command.SiteDBGroupId;
        var settingKey1 = command.GetSettingKey();
        var valid1 = command.IsValid();

        // Assert - All calls should return same values
        agentId1.Should().Be(initialAgentId);
        site1.Should().Be(initialSite);
        siteDBGroupId1.Should().Be(initialSiteDBGroupId);
        settingKey1.Should().Be(initialSettingKey);
        valid1.Should().Be(initialValid);
    }

    [Fact]
    public void Command_BehaviorIsConsistentAcrossInstances()
    {
        // Arrange
        var agentId = 123;
        var site = "TestSite";
        var siteDBGroupId = 456;
        var command1 = new RegisterDummyMemberCommand(agentId, site, siteDBGroupId);
        var command2 = new RegisterDummyMemberCommand(agentId, site, siteDBGroupId);

        // Act & Assert
        command1.AgentId.Should().Be(command2.AgentId);
        command1.Site.Should().Be(command2.Site);
        command1.SiteDBGroupId.Should().Be(command2.SiteDBGroupId);
        command1.GetSettingKey().Should().Be(command2.GetSettingKey());
        command1.IsValid().Should().Be(command2.IsValid());

        // They should behave identically but be different instances
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void MutableProperties_CanBeModifiedIndependently()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        command.ErrCode = 500;
        command.ErrMsg = "Error occurred";
        command.Username = "newuser";
        command.CustId = 999;

        // Assert
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Error occurred");
        command.Username.Should().Be("newuser");
        command.CustId.Should().Be(999);

        // Read-only properties should remain unchanged
        command.AgentId.Should().Be(123);
        command.Site.Should().Be("TestSite");
        command.SiteDBGroupId.Should().Be(456);
        command.IsValid().Should().BeTrue();
    }

    [Fact]
    public void ValidationBoundaryConditions_AreTested()
    {
        // Arrange & Act & Assert
        // Test boundary at zero
        new RegisterDummyMemberCommand(0, "Site", 123).IsValid().Should().BeFalse();
        new RegisterDummyMemberCommand(1, "Site", 123).IsValid().Should().BeTrue();

        // Test negative boundary
        new RegisterDummyMemberCommand(-1, "Site", 123).IsValid().Should().BeFalse();

        // Test extreme values
        new RegisterDummyMemberCommand(int.MaxValue, "Site", 123).IsValid().Should().BeTrue();
        new RegisterDummyMemberCommand(int.MinValue, "Site", 123).IsValid().Should().BeFalse();
    }

    [Fact]
    public void AllMutableProperties_CanBeSetToNull()
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        command.ErrMsg = null;
        command.Username = null;

        // Assert
        command.ErrMsg.Should().BeNull();
        command.Username.Should().BeNull();
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
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        command.ErrCode = errCode;

        // Assert
        command.ErrCode.Should().Be(errCode);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void CustId_AcceptsVariousValues(int custId)
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        command.CustId = custId;

        // Assert
        command.CustId.Should().Be(custId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("username")]
    [InlineData("   ")]
    [InlineData("Very long username with special characters @#$%^&*()")]
    public void Username_AcceptsVariousValues(string username)
    {
        // Arrange
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        command.Username = username;

        // Assert
        command.Username.Should().Be(username);
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
        var command = new RegisterDummyMemberCommand(123, "TestSite", 456);

        // Act
        command.ErrMsg = errMsg;

        // Assert
        command.ErrMsg.Should().Be(errMsg);
    }

    [Fact]
    public void ReadOnlyProperties_DoNotAffectValidation()
    {
        // Arrange & Act & Assert
        // Validation should only depend on AgentId, not Site or SiteDBGroupId
        new RegisterDummyMemberCommand(1, null, -999).IsValid().Should().BeTrue();
        new RegisterDummyMemberCommand(1, "", int.MinValue).IsValid().Should().BeTrue();
        new RegisterDummyMemberCommand(1, "ValidSite", int.MaxValue).IsValid().Should().BeTrue();

        new RegisterDummyMemberCommand(0, "ValidSite", 999).IsValid().Should().BeFalse();
        new RegisterDummyMemberCommand(-1, "ValidSite", 999).IsValid().Should().BeFalse();
    }

    [Fact]
    public void ValidationLogic_IsBasedOnAgentIdGreaterThanZero()
    {
        // Arrange & Act & Assert
        new RegisterDummyMemberCommand(1, "Site", 123).IsValid().Should().BeTrue();
        new RegisterDummyMemberCommand(2, "Site", 123).IsValid().Should().BeTrue();
        new RegisterDummyMemberCommand(1000, "Site", 123).IsValid().Should().BeTrue();

        new RegisterDummyMemberCommand(0, "Site", 123).IsValid().Should().BeFalse();
        new RegisterDummyMemberCommand(-1, "Site", 123).IsValid().Should().BeFalse();
        new RegisterDummyMemberCommand(-1000, "Site", 123).IsValid().Should().BeFalse();
    }
}