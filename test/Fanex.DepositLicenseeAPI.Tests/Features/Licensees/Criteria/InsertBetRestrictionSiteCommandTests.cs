namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class InsertBetRestrictionSiteCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var custDBGroupId = 123;
        var siteId = 456;

        // Act
        var command = new InsertBetRestrictionSiteCommand(custDBGroupId, siteId);

        // Assert
        command.Should().NotBeNull();
        command.CustDBGroupId.Should().Be(custDBGroupId);
        command.SiteId.Should().Be(siteId);
    }

    [Fact]
    public void CustDBGroupId_IsReadOnly()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act & Assert
        var property = typeof(InsertBetRestrictionSiteCommand).GetProperty(nameof(InsertBetRestrictionSiteCommand.CustDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void SiteId_IsReadOnly()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act & Assert
        var property = typeof(InsertBetRestrictionSiteCommand).GetProperty(nameof(InsertBetRestrictionSiteCommand.SiteId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void UserId_HasCorrectDefaultValue()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act & Assert
        command.UserId.Should().Be(10_278_938);
    }

    [Fact]
    public void LiveBetAccept_HasCorrectDefaultValue()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act & Assert
        command.LiveBetAccept.Should().Be(-2);
    }

    [Fact]
    public void SeizeBetTicket_HasDefaultValue()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act & Assert
        command.SeizeBetTicket.Should().Be(0);
    }

    [Fact]
    public void FollowAccount_HasDefaultValue()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act & Assert
        command.FollowAccount.Should().BeNull();
    }

    [Fact]
    public void ErrCode_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);
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
        var command = new InsertBetRestrictionSiteCommand(123, 456);
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
        var custDBGroupId = 123;
        var command = new InsertBetRestrictionSiteCommand(custDBGroupId, 456);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Contain("NAP_BetRestriction_SiteIns");
        result.Should().Contain(custDBGroupId.ToString());
    }

    [Fact]
    public void IsValid_WithPositiveSiteId_ReturnsTrue()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithZeroSiteId_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 0);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativeSiteId_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, -1);

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
    public void IsValid_WithPositiveSiteIds_ReturnsTrue(int siteId, bool expected)
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, siteId);

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
    public void IsValid_WithNonPositiveSiteIds_ReturnsFalse(int siteId, bool expected)
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, siteId);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertBetRestrictionSiteCommand(123, 456);

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var custDBGroupId1 = 111;
        var siteId1 = 222;
        var custDBGroupId2 = 333;
        var siteId2 = 444;

        // Act
        var command1 = new InsertBetRestrictionSiteCommand(custDBGroupId1, siteId1);
        var command2 = new InsertBetRestrictionSiteCommand(custDBGroupId2, siteId2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.CustDBGroupId.Should().Be(custDBGroupId1);
        command1.SiteId.Should().Be(siteId1);
        command2.CustDBGroupId.Should().Be(custDBGroupId2);
        command2.SiteId.Should().Be(siteId2);
    }

    [Fact]
    public void ValidationLogic_IsBasedOnSiteIdGreaterThanZero()
    {
        // Arrange & Act & Assert
        new InsertBetRestrictionSiteCommand(123, 1).IsValid().Should().BeTrue();
        new InsertBetRestrictionSiteCommand(123, 2).IsValid().Should().BeTrue();
        new InsertBetRestrictionSiteCommand(123, 1000).IsValid().Should().BeTrue();

        new InsertBetRestrictionSiteCommand(123, 0).IsValid().Should().BeFalse();
        new InsertBetRestrictionSiteCommand(123, -1).IsValid().Should().BeFalse();
        new InsertBetRestrictionSiteCommand(123, -1000).IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesConditionSiteIdGreaterThanZero()
    {
        // Arrange
        var positiveCommand = new InsertBetRestrictionSiteCommand(123, 42);
        var zeroCommand = new InsertBetRestrictionSiteCommand(123, 0);
        var negativeCommand = new InsertBetRestrictionSiteCommand(123, -42);

        // Act & Assert
        positiveCommand.IsValid().Should().Be(positiveCommand.SiteId > 0);
        zeroCommand.IsValid().Should().Be(zeroCommand.SiteId > 0);
        negativeCommand.IsValid().Should().Be(negativeCommand.SiteId > 0);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(100, 200)]
    [InlineData(0, 300)]
    [InlineData(-50, 400)]
    public void Constructor_AcceptsVariousParameterValues(int custDBGroupId, int siteId)
    {
        // Arrange & Act
        var command = new InsertBetRestrictionSiteCommand(custDBGroupId, siteId);

        // Assert
        command.CustDBGroupId.Should().Be(custDBGroupId);
        command.SiteId.Should().Be(siteId);
    }

    [Fact]
    public void GetSettingKey_CallsSiteDBHelpers()
    {
        // Arrange
        var custDBGroupId = 123;
        var command = new InsertBetRestrictionSiteCommand(custDBGroupId, 456);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be($"SiteDB{custDBGroupId}_NAP_BetRestriction_SiteIns");
    }
}