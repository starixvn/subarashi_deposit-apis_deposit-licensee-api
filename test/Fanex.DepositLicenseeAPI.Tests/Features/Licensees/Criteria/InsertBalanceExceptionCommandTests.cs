namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class InsertBalanceExceptionCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var insBalExceptCustList = "cust1,cust2,cust3";
        var siteDBGroupId = 123;

        // Act
        var command = new InsertBalanceExceptionCommand(insBalExceptCustList, siteDBGroupId);

        // Assert
        command.Should().NotBeNull();
        command.InsBalExceptCustList.Should().Be(insBalExceptCustList);
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Fact]
    public void InsBalExceptCustList_IsReadOnly()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", 123);

        // Act & Assert
        var property = typeof(InsertBalanceExceptionCommand).GetProperty(nameof(InsertBalanceExceptionCommand.InsBalExceptCustList));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void SiteDBGroupId_IsReadOnly()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", 123);

        // Act & Assert
        var property = typeof(InsertBalanceExceptionCommand).GetProperty(nameof(InsertBalanceExceptionCommand.SiteDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var siteDBGroupId = 123;
        var command = new InsertBalanceExceptionCommand("cust1,cust2", siteDBGroupId);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be($"SiteDB{siteDBGroupId}_API_SiteBuilder_InsertBalance_Exception");
        result.Should().Be("SiteDB123_API_SiteBuilder_InsertBalance_Exception");
    }

    [Fact]
    public void GetSettingKey_WithZeroSiteDBGroupId_ReturnsBaseName()
    {
        // Arrange
        var siteDBGroupId = 0;
        var command = new InsertBalanceExceptionCommand("cust1,cust2", siteDBGroupId);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_SiteBuilder_InsertBalance_Exception");
    }

    [Fact]
    public void IsValid_WithValidInsBalExceptCustList_ReturnsTrue()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", 123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullInsBalExceptCustList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(null, 123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyInsBalExceptCustList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("", 123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceInsBalExceptCustList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("   ", 123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("cust1", true)]
    [InlineData("cust1,cust2", true)]
    [InlineData("cust1,cust2,cust3", true)]
    [InlineData("a", true)]
    [InlineData("customer with spaces", true)]
    public void IsValid_WithValidInsBalExceptCustListValues_ReturnsTrue(string insBalExceptCustList, bool expected)
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(insBalExceptCustList, 123);

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
    public void IsValid_WithInvalidInsBalExceptCustListValues_ReturnsFalse(string insBalExceptCustList, bool expected)
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(insBalExceptCustList, 123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", 123);

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var insBalExceptCustList1 = "cust1,cust2";
        var siteDBGroupId1 = 123;
        var insBalExceptCustList2 = "cust3,cust4";
        var siteDBGroupId2 = 456;

        // Act
        var command1 = new InsertBalanceExceptionCommand(insBalExceptCustList1, siteDBGroupId1);
        var command2 = new InsertBalanceExceptionCommand(insBalExceptCustList2, siteDBGroupId2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.InsBalExceptCustList.Should().Be(insBalExceptCustList1);
        command1.SiteDBGroupId.Should().Be(siteDBGroupId1);
        command2.InsBalExceptCustList.Should().Be(insBalExceptCustList2);
        command2.SiteDBGroupId.Should().Be(siteDBGroupId2);
    }

    [Theory]
    [InlineData("cust1", 1)]
    [InlineData("cust1,cust2", 100)]
    [InlineData("", 0)]
    [InlineData(null, -1)]
    [InlineData("Special@Characters!", int.MaxValue)]
    [InlineData("   whitespace   ", int.MinValue)]
    public void Constructor_WithVariousParameters_InitializesCorrectly(string insBalExceptCustList, int siteDBGroupId)
    {
        // Arrange & Act
        var command = new InsertBalanceExceptionCommand(insBalExceptCustList, siteDBGroupId);

        // Assert
        command.InsBalExceptCustList.Should().Be(insBalExceptCustList);
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(-50)]
    public void GetSettingKey_UsesSiteDBHelpers(int siteDBGroupId)
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", siteDBGroupId);

        // Act
        var result = command.GetSettingKey();

        // Assert
        if (siteDBGroupId == 0)
        {
            result.Should().Be("API_SiteBuilder_InsertBalance_Exception");
        }
        else
        {
            result.Should().Be($"SiteDB{siteDBGroupId}_API_SiteBuilder_InsertBalance_Exception");
        }
    }

    [Fact]
    public void ValidationLogic_OnlyDependsOnInsBalExceptCustList()
    {
        // Arrange & Act & Assert
        new InsertBalanceExceptionCommand("valid", 123).IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand("valid", -999).IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand("valid", 0).IsValid().Should().BeTrue();

        new InsertBalanceExceptionCommand(null, 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("", 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("   ", 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand(null, 999).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("", -999).IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesConditionNotNullOrWhiteSpace()
    {
        // Arrange
        var validCommand = new InsertBalanceExceptionCommand("validcustlist", 123);
        var nullCommand = new InsertBalanceExceptionCommand(null, 123);
        var emptyCommand = new InsertBalanceExceptionCommand("", 123);
        var whitespaceCommand = new InsertBalanceExceptionCommand("   ", 123);

        // Act & Assert
        validCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(validCommand.InsBalExceptCustList));
        nullCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(nullCommand.InsBalExceptCustList));
        emptyCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(emptyCommand.InsBalExceptCustList));
        whitespaceCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(whitespaceCommand.InsBalExceptCustList));
    }

    [Fact]
    public void GetSettingKey_IsConsistentForSameInstance()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", 123);

        // Act
        var result1 = command.GetSettingKey();
        var result2 = command.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("SiteDB123_API_SiteBuilder_InsertBalance_Exception");
    }

    [Fact]
    public void ValidationBoundaryConditions_AreTested()
    {
        // Arrange & Act & Assert
        // Test string boundary conditions
        new InsertBalanceExceptionCommand("a", 123).IsValid().Should().BeTrue(); // Single character
        new InsertBalanceExceptionCommand(" a ", 123).IsValid().Should().BeTrue(); // Whitespace around valid content
        new InsertBalanceExceptionCommand("", 123).IsValid().Should().BeFalse(); // Empty string
        new InsertBalanceExceptionCommand(null, 123).IsValid().Should().BeFalse(); // Null string
        new InsertBalanceExceptionCommand("   ", 123).IsValid().Should().BeFalse(); // Only whitespace
        new InsertBalanceExceptionCommand("\t\n", 123).IsValid().Should().BeFalse(); // Only whitespace characters
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", 123);
        var initialInsBalExceptCustList = command.InsBalExceptCustList;
        var initialSiteDBGroupId = command.SiteDBGroupId;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var insBalExceptCustList1 = command.InsBalExceptCustList;
        var siteDBGroupId1 = command.SiteDBGroupId;
        var settingKey1 = command.GetSettingKey();
        var valid1 = command.IsValid();

        // Assert - All calls should return same values
        insBalExceptCustList1.Should().Be(initialInsBalExceptCustList);
        siteDBGroupId1.Should().Be(initialSiteDBGroupId);
        settingKey1.Should().Be(initialSettingKey);
        valid1.Should().Be(initialValid);
    }

    [Fact]
    public void Command_BehaviorIsConsistentAcrossInstances()
    {
        // Arrange
        var insBalExceptCustList = "cust1,cust2";
        var siteDBGroupId = 123;
        var command1 = new InsertBalanceExceptionCommand(insBalExceptCustList, siteDBGroupId);
        var command2 = new InsertBalanceExceptionCommand(insBalExceptCustList, siteDBGroupId);

        // Act & Assert
        command1.InsBalExceptCustList.Should().Be(command2.InsBalExceptCustList);
        command1.SiteDBGroupId.Should().Be(command2.SiteDBGroupId);
        command1.GetSettingKey().Should().Be(command2.GetSettingKey());
        command1.IsValid().Should().Be(command2.IsValid());

        // They should behave identically but be different instances
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void ValidationDoesNotDependOnSiteDBGroupId()
    {
        // Arrange & Act & Assert
        // Validation should only depend on InsBalExceptCustList, not on SiteDBGroupId
        new InsertBalanceExceptionCommand("valid", -999).IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand("valid", 0).IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand("valid", int.MaxValue).IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand("valid", int.MinValue).IsValid().Should().BeTrue();

        new InsertBalanceExceptionCommand(null, 999).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("", 999).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("   ", 999).IsValid().Should().BeFalse();
    }

    [Fact]
    public void GetSettingKey_NeverChanges()
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand("cust1,cust2", 123);

        // Act - Call multiple times
        var key1 = command.GetSettingKey();
        var key2 = command.GetSettingKey();
        var key3 = command.GetSettingKey();

        // Assert
        key1.Should().Be("SiteDB123_API_SiteBuilder_InsertBalance_Exception");
        key2.Should().Be("SiteDB123_API_SiteBuilder_InsertBalance_Exception");
        key3.Should().Be("SiteDB123_API_SiteBuilder_InsertBalance_Exception");
        key1.Should().Be(key2);
        key2.Should().Be(key3);
    }

    [Theory]
    [InlineData("cust1")]
    [InlineData("cust1,cust2,cust3")]
    [InlineData("123,456,789")]
    [InlineData("a,b,c,d,e")]
    [InlineData("customer with spaces,another customer")]
    [InlineData("special@chars#,more$special%")]
    public void IsValid_WithVariousValidCustomerLists_ReturnsTrue(string insBalExceptCustList)
    {
        // Arrange
        var command = new InsertBalanceExceptionCommand(insBalExceptCustList, 123);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1000)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(int.MaxValue)]
    public void SiteDBGroupId_AcceptsVariousValues(int siteDBGroupId)
    {
        // Arrange & Act
        var command = new InsertBalanceExceptionCommand("cust1,cust2", siteDBGroupId);

        // Assert
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Fact]
    public void ValidationLogic_IsBasedOnStringNotNullOrWhiteSpace()
    {
        // Arrange & Act & Assert
        new InsertBalanceExceptionCommand("a", 123).IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand("valid", 123).IsValid().Should().BeTrue();
        new InsertBalanceExceptionCommand("cust1,cust2", 123).IsValid().Should().BeTrue();

        new InsertBalanceExceptionCommand(null, 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("", 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand(" ", 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("\t", 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("\n", 123).IsValid().Should().BeFalse();
        new InsertBalanceExceptionCommand("   ", 123).IsValid().Should().BeFalse();
    }

    [Fact]
    public void Command_IsImmutableForConstructorProperties()
    {
        // Arrange
        var originalInsBalExceptCustList = "cust1,cust2";
        var originalSiteDBGroupId = 123;
        var command = new InsertBalanceExceptionCommand(originalInsBalExceptCustList, originalSiteDBGroupId);

        // Act - Try to verify immutability by calling properties multiple times
        var insBalExceptCustList1 = command.InsBalExceptCustList;
        var insBalExceptCustList2 = command.InsBalExceptCustList;
        var siteDBGroupId1 = command.SiteDBGroupId;
        var siteDBGroupId2 = command.SiteDBGroupId;

        // Assert - All values should remain consistent with original constructor parameters
        insBalExceptCustList1.Should().Be(originalInsBalExceptCustList);
        insBalExceptCustList2.Should().Be(originalInsBalExceptCustList);
        insBalExceptCustList1.Should().Be(insBalExceptCustList2);

        siteDBGroupId1.Should().Be(originalSiteDBGroupId);
        siteDBGroupId2.Should().Be(originalSiteDBGroupId);
        siteDBGroupId1.Should().Be(siteDBGroupId2);
    }
}