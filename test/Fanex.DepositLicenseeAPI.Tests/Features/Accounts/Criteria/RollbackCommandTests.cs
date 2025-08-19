namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Accounts.Criteria;

public class RollbackCommandTests
{
    [Fact]
    public void Constructor_WithValidCustId_InitializesCorrectly()
    {
        // Arrange
        var custId = 123;

        // Act
        var command = new RollbackCommand(custId);

        // Assert
        command.Should().NotBeNull();
        command.CustId.Should().Be(custId);
    }

    [Fact]
    public void CustId_IsReadOnly()
    {
        // Arrange
        var command = new RollbackCommand(123);

        // Act & Assert
        var property = typeof(RollbackCommand).GetProperty(nameof(RollbackCommand.CustId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var command = new RollbackCommand(123);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("Acc_CustomerIns_Rollback");
    }

    [Fact]
    public void IsValid_WithPositiveCustId_ReturnsTrue()
    {
        // Arrange
        var command = new RollbackCommand(123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithZeroCustId_ReturnsFalse()
    {
        // Arrange
        var command = new RollbackCommand(0);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativeCustId_ReturnsFalse()
    {
        // Arrange
        var command = new RollbackCommand(-1);

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
    public void IsValid_WithPositiveCustIds_ReturnsTrue(int custId, bool expected)
    {
        // Arrange
        var command = new RollbackCommand(custId);

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
    public void IsValid_WithNonPositiveCustIds_ReturnsFalse(int custId, bool expected)
    {
        // Arrange
        var command = new RollbackCommand(custId);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new RollbackCommand(123);

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new RollbackCommand(100);
        var command2 = new RollbackCommand(200);

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("Acc_CustomerIns_Rollback");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var custId1 = 111;
        var custId2 = 222;

        // Act
        var command1 = new RollbackCommand(custId1);
        var command2 = new RollbackCommand(custId2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.CustId.Should().Be(custId1);
        command2.CustId.Should().Be(custId2);
        command1.CustId.Should().NotBe(command2.CustId);
    }

    [Fact]
    public void ValidationLogic_IsBasedOnCustIdGreaterThanZero()
    {
        // Arrange & Act & Assert
        new RollbackCommand(1).IsValid().Should().BeTrue();
        new RollbackCommand(2).IsValid().Should().BeTrue();
        new RollbackCommand(1000).IsValid().Should().BeTrue();

        new RollbackCommand(0).IsValid().Should().BeFalse();
        new RollbackCommand(-1).IsValid().Should().BeFalse();
        new RollbackCommand(-1000).IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesConditionCustIdGreaterThanZero()
    {
        // Arrange
        var positiveCommand = new RollbackCommand(42);
        var zeroCommand = new RollbackCommand(0);
        var negativeCommand = new RollbackCommand(-42);

        // Act & Assert
        positiveCommand.IsValid().Should().Be(positiveCommand.CustId > 0);
        zeroCommand.IsValid().Should().Be(zeroCommand.CustId > 0);
        negativeCommand.IsValid().Should().Be(negativeCommand.CustId > 0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(-50)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Constructor_AcceptsVariousCustIdValues(int custId)
    {
        // Arrange & Act
        var command = new RollbackCommand(custId);

        // Assert
        command.CustId.Should().Be(custId);
    }

    [Fact]
    public void GetSettingKey_IsConstant()
    {
        // Arrange
        var commands = new[]
        {
            new RollbackCommand(1),
            new RollbackCommand(100),
            new RollbackCommand(0),
            new RollbackCommand(-1)
        };

        // Act
        var settingKeys = commands.Select(c => c.GetSettingKey()).ToArray();

        // Assert
        settingKeys.Should().AllBe("Acc_CustomerIns_Rollback");
        settingKeys.Should().HaveCount(4);
        settingKeys.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public void ValidationLogic_OnlyDependsOnCustId()
    {
        // Arrange
        var validCommand = new RollbackCommand(42);
        var invalidCommand = new RollbackCommand(-42);

        // Act & Assert
        validCommand.IsValid().Should().BeTrue();
        invalidCommand.IsValid().Should().BeFalse();

        // Validation should be deterministic
        validCommand.IsValid().Should().Be(validCommand.IsValid());
        invalidCommand.IsValid().Should().Be(invalidCommand.IsValid());
    }

    [Fact]
    public void Command_HasNoMutableState()
    {
        // Arrange
        var command = new RollbackCommand(123);
        var initialCustId = command.CustId;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var custId1 = command.CustId;
        var custId2 = command.CustId;
        var settingKey1 = command.GetSettingKey();
        var settingKey2 = command.GetSettingKey();
        var valid1 = command.IsValid();
        var valid2 = command.IsValid();

        // Assert - All calls should return same values
        custId1.Should().Be(initialCustId);
        custId2.Should().Be(initialCustId);
        custId1.Should().Be(custId2);

        settingKey1.Should().Be(initialSettingKey);
        settingKey2.Should().Be(initialSettingKey);
        settingKey1.Should().Be(settingKey2);

        valid1.Should().Be(initialValid);
        valid2.Should().Be(initialValid);
        valid1.Should().Be(valid2);
    }

    [Fact]
    public void Command_BehaviorIsConsistentAcrossInstances()
    {
        // Arrange
        var custId = 42;
        var command1 = new RollbackCommand(custId);
        var command2 = new RollbackCommand(custId);

        // Act & Assert
        command1.CustId.Should().Be(command2.CustId);
        command1.GetSettingKey().Should().Be(command2.GetSettingKey());
        command1.IsValid().Should().Be(command2.IsValid());

        // They should behave identically but be different instances
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void ValidationBoundaryConditions_AreTested()
    {
        // Arrange & Act & Assert
        // Test boundary at zero
        new RollbackCommand(0).IsValid().Should().BeFalse();
        new RollbackCommand(1).IsValid().Should().BeTrue();

        // Test negative boundary
        new RollbackCommand(-1).IsValid().Should().BeFalse();

        // Test extreme values
        new RollbackCommand(int.MaxValue).IsValid().Should().BeTrue();
        new RollbackCommand(int.MinValue).IsValid().Should().BeFalse();
    }
}