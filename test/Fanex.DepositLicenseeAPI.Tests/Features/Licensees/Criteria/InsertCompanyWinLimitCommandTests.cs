namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class InsertCompanyWinLimitCommandTests
{
    [Fact]
    public void Constructor_WithValidLicenseeId_InitializesCorrectly()
    {
        // Arrange
        var licenseeId = 123;

        // Act
        var command = new InsertCompanyWinLimitCommand(licenseeId);

        // Assert
        command.Should().NotBeNull();
        command.LicenseeId.Should().Be(licenseeId);
    }

    [Fact]
    public void LicenseeId_IsReadOnly()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);

        // Act & Assert
        var property = typeof(InsertCompanyWinLimitCommand).GetProperty(nameof(InsertCompanyWinLimitCommand.LicenseeId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void ErrCode_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);
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
        var command = new InsertCompanyWinLimitCommand(123);
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
        var command = new InsertCompanyWinLimitCommand(123);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("Dot_Dep_CWL_InsertSetting");
    }

    [Fact]
    public void IsValid_WithPositiveLicenseeId_ReturnsTrue()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithZeroLicenseeId_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(0);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativeLicenseeId_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(-1);

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
    public void IsValid_WithPositiveLicenseeIds_ReturnsTrue(int licenseeId, bool expected)
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(licenseeId);

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
    public void IsValid_WithNonPositiveLicenseeIds_ReturnsFalse(int licenseeId, bool expected)
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(licenseeId);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new InsertCompanyWinLimitCommand(100);
        var command2 = new InsertCompanyWinLimitCommand(200);

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("Dot_Dep_CWL_InsertSetting");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var licenseeId1 = 111;
        var licenseeId2 = 222;

        // Act
        var command1 = new InsertCompanyWinLimitCommand(licenseeId1);
        var command2 = new InsertCompanyWinLimitCommand(licenseeId2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.LicenseeId.Should().Be(licenseeId1);
        command2.LicenseeId.Should().Be(licenseeId2);
        command1.LicenseeId.Should().NotBe(command2.LicenseeId);
    }

    [Fact]
    public void ValidationLogic_IsBasedOnLicenseeIdGreaterThanZero()
    {
        // Arrange & Act & Assert
        new InsertCompanyWinLimitCommand(1).IsValid().Should().BeTrue();
        new InsertCompanyWinLimitCommand(2).IsValid().Should().BeTrue();
        new InsertCompanyWinLimitCommand(1000).IsValid().Should().BeTrue();

        new InsertCompanyWinLimitCommand(0).IsValid().Should().BeFalse();
        new InsertCompanyWinLimitCommand(-1).IsValid().Should().BeFalse();
        new InsertCompanyWinLimitCommand(-1000).IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesConditionLicenseeIdGreaterThanZero()
    {
        // Arrange
        var positiveCommand = new InsertCompanyWinLimitCommand(42);
        var zeroCommand = new InsertCompanyWinLimitCommand(0);
        var negativeCommand = new InsertCompanyWinLimitCommand(-42);

        // Act & Assert
        positiveCommand.IsValid().Should().Be(positiveCommand.LicenseeId > 0);
        zeroCommand.IsValid().Should().Be(zeroCommand.LicenseeId > 0);
        negativeCommand.IsValid().Should().Be(negativeCommand.LicenseeId > 0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(-50)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Constructor_AcceptsVariousLicenseeIdValues(int licenseeId)
    {
        // Arrange & Act
        var command = new InsertCompanyWinLimitCommand(licenseeId);

        // Assert
        command.LicenseeId.Should().Be(licenseeId);
    }

    [Fact]
    public void GetSettingKey_IsConstant()
    {
        // Arrange
        var commands = new[]
        {
            new InsertCompanyWinLimitCommand(1),
            new InsertCompanyWinLimitCommand(100),
            new InsertCompanyWinLimitCommand(0),
            new InsertCompanyWinLimitCommand(-1)
        };

        // Act
        var settingKeys = commands.Select(c => c.GetSettingKey()).ToArray();

        // Assert
        settingKeys.Should().AllBe("Dot_Dep_CWL_InsertSetting");
        settingKeys.Should().HaveCount(4);
        settingKeys.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public void ValidationLogic_OnlyDependsOnLicenseeId()
    {
        // Arrange
        var validCommand = new InsertCompanyWinLimitCommand(42);
        var invalidCommand = new InsertCompanyWinLimitCommand(-42);

        // Act & Assert
        validCommand.IsValid().Should().BeTrue();
        invalidCommand.IsValid().Should().BeFalse();

        // Validation should be deterministic
        validCommand.IsValid().Should().Be(validCommand.IsValid());
        invalidCommand.IsValid().Should().Be(invalidCommand.IsValid());
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);
        var initialLicenseeId = command.LicenseeId;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var licenseeId1 = command.LicenseeId;
        var licenseeId2 = command.LicenseeId;
        var settingKey1 = command.GetSettingKey();
        var settingKey2 = command.GetSettingKey();
        var valid1 = command.IsValid();
        var valid2 = command.IsValid();

        // Assert - All calls should return same values
        licenseeId1.Should().Be(initialLicenseeId);
        licenseeId2.Should().Be(initialLicenseeId);
        licenseeId1.Should().Be(licenseeId2);

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
        var licenseeId = 42;
        var command1 = new InsertCompanyWinLimitCommand(licenseeId);
        var command2 = new InsertCompanyWinLimitCommand(licenseeId);

        // Act & Assert
        command1.LicenseeId.Should().Be(command2.LicenseeId);
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
        new InsertCompanyWinLimitCommand(0).IsValid().Should().BeFalse();
        new InsertCompanyWinLimitCommand(1).IsValid().Should().BeTrue();

        // Test negative boundary
        new InsertCompanyWinLimitCommand(-1).IsValid().Should().BeFalse();

        // Test extreme values
        new InsertCompanyWinLimitCommand(int.MaxValue).IsValid().Should().BeTrue();
        new InsertCompanyWinLimitCommand(int.MinValue).IsValid().Should().BeFalse();
    }

    [Fact]
    public void ErrCode_DefaultValue_IsZero()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);

        // Act & Assert
        command.ErrCode.Should().Be(0);
    }

    [Fact]
    public void ErrMsg_DefaultValue_IsNull()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);

        // Act & Assert
        command.ErrMsg.Should().BeNull();
    }

    [Fact]
    public void MutableProperties_CanBeModifiedIndependently()
    {
        // Arrange
        var command = new InsertCompanyWinLimitCommand(123);

        // Act
        command.ErrCode = 500;
        command.ErrMsg = "Error occurred";

        // Assert
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Error occurred");
        command.LicenseeId.Should().Be(123); // Readonly property unchanged
        command.IsValid().Should().BeTrue(); // Validation unchanged
    }
}