namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class RollbackCreateLicenseeCommandTests
{
    [Fact]
    public void Constructor_WithValidCustIds_InitializesCorrectly()
    {
        // Arrange
        var custIds = "123,456,789";

        // Act
        var command = new RollbackCreateLicenseeCommand(custIds);

        // Assert
        command.Should().NotBeNull();
        command.CustIds.Should().Be(custIds);
        command.IsRollbackFullLine.Should().Be(1);
    }

    [Fact]
    public void CustIds_IsReadOnly()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");

        // Act & Assert
        var property = typeof(RollbackCreateLicenseeCommand).GetProperty(nameof(RollbackCreateLicenseeCommand.CustIds));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void IsRollbackFullLine_IsReadOnly()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");

        // Act & Assert
        var property = typeof(RollbackCreateLicenseeCommand).GetProperty(nameof(RollbackCreateLicenseeCommand.IsRollbackFullLine));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void IsRollbackFullLine_AlwaysReturnsOne()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");

        // Act
        var result = command.IsRollbackFullLine;

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("Acc_CustomerIns_Rollback_Multiple");
    }

    [Fact]
    public void IsValid_WithValidCustIds_ReturnsTrue()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456,789");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullCustIds_ReturnsFalse()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand(null);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyCustIds_ReturnsFalse()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceCustIds_ReturnsFalse()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("   ");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("123", true)]
    [InlineData("123,456", true)]
    [InlineData("123,456,789", true)]
    [InlineData("a", true)]
    [InlineData("customer with spaces", true)]
    public void IsValid_WithValidCustIdsValues_ReturnsTrue(string custIds, bool expected)
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand(custIds);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("\t", false)]
    [InlineData("\n", false)]
    public void IsValid_WithInvalidCustIdsValues_ReturnsFalse(string custIds, bool expected)
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand(custIds);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new RollbackCreateLicenseeCommand("123,456");
        var command2 = new RollbackCreateLicenseeCommand("789,999");

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("Acc_CustomerIns_Rollback_Multiple");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var custIds1 = "123,456";
        var custIds2 = "789,999";

        // Act
        var command1 = new RollbackCreateLicenseeCommand(custIds1);
        var command2 = new RollbackCreateLicenseeCommand(custIds2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.CustIds.Should().Be(custIds1);
        command2.CustIds.Should().Be(custIds2);
        command1.CustIds.Should().NotBe(command2.CustIds);

        // Both should have the same IsRollbackFullLine value
        command1.IsRollbackFullLine.Should().Be(1);
        command2.IsRollbackFullLine.Should().Be(1);
        command1.IsRollbackFullLine.Should().Be(command2.IsRollbackFullLine);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("123,456")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Special@Characters!")]
    [InlineData("   whitespace   ")]
    public void Constructor_WithVariousCustIds_InitializesCorrectly(string custIds)
    {
        // Arrange & Act
        var command = new RollbackCreateLicenseeCommand(custIds);

        // Assert
        command.CustIds.Should().Be(custIds);
        command.IsRollbackFullLine.Should().Be(1);
    }

    [Fact]
    public void ValidationLogic_OnlyDependsOnCustIds()
    {
        // Arrange & Act & Assert
        new RollbackCreateLicenseeCommand("valid").IsValid().Should().BeTrue();
        new RollbackCreateLicenseeCommand("123,456,789").IsValid().Should().BeTrue();

        new RollbackCreateLicenseeCommand(null).IsValid().Should().BeFalse();
        new RollbackCreateLicenseeCommand("").IsValid().Should().BeFalse();
        new RollbackCreateLicenseeCommand("   ").IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesConditionNotNullOrWhiteSpace()
    {
        // Arrange
        var validCommand = new RollbackCreateLicenseeCommand("validcustids");
        var nullCommand = new RollbackCreateLicenseeCommand(null);
        var emptyCommand = new RollbackCreateLicenseeCommand("");
        var whitespaceCommand = new RollbackCreateLicenseeCommand("   ");

        // Act & Assert
        validCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(validCommand.CustIds));
        nullCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(nullCommand.CustIds));
        emptyCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(emptyCommand.CustIds));
        whitespaceCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(whitespaceCommand.CustIds));
    }

    [Fact]
    public void GetSettingKey_IsConstant()
    {
        // Arrange
        var commands = new[]
        {
            new RollbackCreateLicenseeCommand("123,456"),
            new RollbackCreateLicenseeCommand("789,999"),
            new RollbackCreateLicenseeCommand(""),
            new RollbackCreateLicenseeCommand(null)
        };

        // Act
        var settingKeys = commands.Select(c => c.GetSettingKey()).ToArray();

        // Assert
        settingKeys.Should().AllBe("Acc_CustomerIns_Rollback_Multiple");
        settingKeys.Should().HaveCount(4);
        settingKeys.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public void ValidationBoundaryConditions_AreTested()
    {
        // Arrange & Act & Assert
        // Test string boundary conditions
        new RollbackCreateLicenseeCommand("1").IsValid().Should().BeTrue(); // Single character
        new RollbackCreateLicenseeCommand(" 123 ").IsValid().Should().BeTrue(); // Whitespace around valid content
        new RollbackCreateLicenseeCommand("").IsValid().Should().BeFalse(); // Empty string
        new RollbackCreateLicenseeCommand(null).IsValid().Should().BeFalse(); // Null string
        new RollbackCreateLicenseeCommand("   ").IsValid().Should().BeFalse(); // Only whitespace
        new RollbackCreateLicenseeCommand("\t\n").IsValid().Should().BeFalse(); // Only whitespace characters
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");
        var initialCustIds = command.CustIds;
        var initialIsRollbackFullLine = command.IsRollbackFullLine;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var custIds1 = command.CustIds;
        var isRollbackFullLine1 = command.IsRollbackFullLine;
        var settingKey1 = command.GetSettingKey();
        var valid1 = command.IsValid();

        // Assert - All calls should return same values
        custIds1.Should().Be(initialCustIds);
        isRollbackFullLine1.Should().Be(initialIsRollbackFullLine);
        settingKey1.Should().Be(initialSettingKey);
        valid1.Should().Be(initialValid);
    }

    [Fact]
    public void Command_BehaviorIsConsistentAcrossInstances()
    {
        // Arrange
        var custIds = "123,456";
        var command1 = new RollbackCreateLicenseeCommand(custIds);
        var command2 = new RollbackCreateLicenseeCommand(custIds);

        // Act & Assert
        command1.CustIds.Should().Be(command2.CustIds);
        command1.IsRollbackFullLine.Should().Be(command2.IsRollbackFullLine);
        command1.GetSettingKey().Should().Be(command2.GetSettingKey());
        command1.IsValid().Should().Be(command2.IsValid());

        // They should behave identically but be different instances
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void IsRollbackFullLine_IsAlwaysOne()
    {
        // Arrange
        var commands = new[]
        {
            new RollbackCreateLicenseeCommand("123"),
            new RollbackCreateLicenseeCommand("456,789"),
            new RollbackCreateLicenseeCommand(""),
            new RollbackCreateLicenseeCommand(null)
        };

        // Act & Assert
        foreach (var command in commands)
        {
            command.IsRollbackFullLine.Should().Be(1, $"Command with CustIds '{command.CustIds}' should have IsRollbackFullLine = 1");
        }
    }

    [Fact]
    public void GetSettingKey_NeverChanges()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");

        // Act - Call multiple times
        var key1 = command.GetSettingKey();
        var key2 = command.GetSettingKey();
        var key3 = command.GetSettingKey();

        // Assert
        key1.Should().Be("Acc_CustomerIns_Rollback_Multiple");
        key2.Should().Be("Acc_CustomerIns_Rollback_Multiple");
        key3.Should().Be("Acc_CustomerIns_Rollback_Multiple");
        key1.Should().Be(key2);
        key2.Should().Be(key3);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("123,456,789")]
    [InlineData("customer1,customer2")]
    [InlineData("a,b,c,d,e")]
    [InlineData("customer with spaces,another customer")]
    [InlineData("special@chars#,more$special%")]
    public void IsValid_WithVariousValidCustomerIds_ReturnsTrue(string custIds)
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand(custIds);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidationLogic_IsBasedOnStringNotNullOrWhiteSpace()
    {
        // Arrange & Act & Assert
        new RollbackCreateLicenseeCommand("1").IsValid().Should().BeTrue();
        new RollbackCreateLicenseeCommand("valid").IsValid().Should().BeTrue();
        new RollbackCreateLicenseeCommand("123,456").IsValid().Should().BeTrue();

        new RollbackCreateLicenseeCommand(null).IsValid().Should().BeFalse();
        new RollbackCreateLicenseeCommand("").IsValid().Should().BeFalse();
        new RollbackCreateLicenseeCommand(" ").IsValid().Should().BeFalse();
        new RollbackCreateLicenseeCommand("\t").IsValid().Should().BeFalse();
        new RollbackCreateLicenseeCommand("\n").IsValid().Should().BeFalse();
        new RollbackCreateLicenseeCommand("   ").IsValid().Should().BeFalse();
    }

    [Fact]
    public void Command_IsImmutableForConstructorProperties()
    {
        // Arrange
        var originalCustIds = "123,456";
        var command = new RollbackCreateLicenseeCommand(originalCustIds);

        // Act - Try to verify immutability by calling properties multiple times
        var custIds1 = command.CustIds;
        var custIds2 = command.CustIds;
        var isRollbackFullLine1 = command.IsRollbackFullLine;
        var isRollbackFullLine2 = command.IsRollbackFullLine;

        // Assert - All values should remain consistent with original constructor parameters
        custIds1.Should().Be(originalCustIds);
        custIds2.Should().Be(originalCustIds);
        custIds1.Should().Be(custIds2);

        isRollbackFullLine1.Should().Be(1);
        isRollbackFullLine2.Should().Be(1);
        isRollbackFullLine1.Should().Be(isRollbackFullLine2);
    }

    [Fact]
    public void IsRollbackFullLine_PropertyReflection()
    {
        // Arrange
        var command = new RollbackCreateLicenseeCommand("123,456");

        // Act & Assert
        var property = typeof(RollbackCreateLicenseeCommand).GetProperty(nameof(RollbackCreateLicenseeCommand.IsRollbackFullLine));
        property.Should().NotBeNull();
        property.PropertyType.Should().Be(typeof(byte));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeFalse();
    }

    [Fact]
    public void Constructor_InitializesIsRollbackFullLineToOne()
    {
        // Arrange & Act
        var commands = new[]
        {
            new RollbackCreateLicenseeCommand("123"),
            new RollbackCreateLicenseeCommand("456,789"),
            new RollbackCreateLicenseeCommand(null),
            new RollbackCreateLicenseeCommand("")
        };

        // Assert
        foreach (var command in commands)
        {
            command.IsRollbackFullLine.Should().Be(1);
        }
    }

    [Fact]
    public void ValidationLogic_OnlyConsidersCustIdsProperty()
    {
        // Arrange & Act & Assert
        // IsRollbackFullLine value should not affect validation
        var validCommand = new RollbackCreateLicenseeCommand("123,456");
        var invalidCommand = new RollbackCreateLicenseeCommand(null);

        validCommand.IsValid().Should().BeTrue();
        validCommand.IsRollbackFullLine.Should().Be(1);

        invalidCommand.IsValid().Should().BeFalse();
        invalidCommand.IsRollbackFullLine.Should().Be(1);

        // Both commands have same IsRollbackFullLine value, but different validation results
        validCommand.IsRollbackFullLine.Should().Be(invalidCommand.IsRollbackFullLine);
        validCommand.IsValid().Should().NotBe(invalidCommand.IsValid());
    }
}