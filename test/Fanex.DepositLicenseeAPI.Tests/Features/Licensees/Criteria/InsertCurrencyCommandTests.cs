namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class InsertCurrencyCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var siteName = "TestSite";
        var currencyList = "USD,EUR";
        var ptSportsbook = 10.5m;
        var ip = "192.168.1.1";

        // Act
        var command = new InsertCurrencyCommand(siteName, currencyList, ptSportsbook, ip);

        // Assert
        command.Should().NotBeNull();
        command.SiteName.Should().Be(siteName);
        command.CurrencyList.Should().Be(currencyList);
        command.PT_Sportsbook.Should().Be(ptSportsbook);
        command.IP.Should().Be(ip);
    }

    [Fact]
    public void SiteName_IsReadOnly()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");

        // Act & Assert
        var property = typeof(InsertCurrencyCommand).GetProperty(nameof(InsertCurrencyCommand.SiteName));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void CurrencyList_IsReadOnly()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");

        // Act & Assert
        var property = typeof(InsertCurrencyCommand).GetProperty(nameof(InsertCurrencyCommand.CurrencyList));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void PT_Sportsbook_IsReadOnly()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");

        // Act & Assert
        var property = typeof(InsertCurrencyCommand).GetProperty(nameof(InsertCurrencyCommand.PT_Sportsbook));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void IP_IsReadOnly()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");

        // Act & Assert
        var property = typeof(InsertCurrencyCommand).GetProperty(nameof(InsertCurrencyCommand.IP));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void ErrCode_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");
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
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");
        var expectedErrMsg = "Test error message";

        // Act
        command.ErrMsg = expectedErrMsg;

        // Assert
        command.ErrMsg.Should().Be(expectedErrMsg);
    }

    [Fact]
    public void LicenseeId_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");
        var expectedLicenseeId = 12345;

        // Act
        command.LicenseeId = expectedLicenseeId;

        // Assert
        command.LicenseeId.Should().Be(expectedLicenseeId);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_SiteBuilder_LicenseeDB_CurrencyIns");
    }

    [Fact]
    public void IsValid_WithAllValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.5m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand(null, "USD", 10.0m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptySiteName_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand("", "USD", 10.0m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand("   ", "USD", 10.0m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullCurrencyList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", null, 10.0m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyCurrencyList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "", 10.0m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceCurrencyList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "   ", 10.0m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithZeroPT_Sportsbook_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 0m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativePT_Sportsbook_ReturnsFalse()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", -1.5m, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Site1", "USD", 1.0, "192.168.1.1", true)]
    [InlineData("Site2", "USD,EUR", 10.5, "10.0.0.1", true)]
    [InlineData("S", "U", 0.01, "127.0.0.1", true)]
    [InlineData("ValidSite", "ValidCurrency", 999.99, "", true)] // IP can be empty
    public void IsValid_WithValidCombinations_ReturnsTrue(string siteName, string currencyList, decimal ptSportsbook, string ip, bool expected)
    {
        // Arrange
        var command = new InsertCurrencyCommand(siteName, currencyList, ptSportsbook, ip);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "USD", 10.0, "192.168.1.1", false)]
    [InlineData("", "USD", 10.0, "192.168.1.1", false)]
    [InlineData("   ", "USD", 10.0, "192.168.1.1", false)]
    [InlineData("Site", null, 10.0, "192.168.1.1", false)]
    [InlineData("Site", "", 10.0, "192.168.1.1", false)]
    [InlineData("Site", "   ", 10.0, "192.168.1.1", false)]
    [InlineData("Site", "USD", 0.0, "192.168.1.1", false)]
    [InlineData("Site", "USD", -1.0, "192.168.1.1", false)]
    [InlineData(null, null, 0.0, null, false)]
    public void IsValid_WithInvalidCombinations_ReturnsFalse(string siteName, string currencyList, decimal ptSportsbook, string ip, bool expected)
    {
        // Arrange
        var command = new InsertCurrencyCommand(siteName, currencyList, ptSportsbook, ip);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.0m, "192.168.1.1");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new InsertCurrencyCommand("Site1", "USD", 10.0m, "192.168.1.1");
        var command2 = new InsertCurrencyCommand("Site2", "EUR", 20.0m, "192.168.1.2");

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("API_SiteBuilder_LicenseeDB_CurrencyIns");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var siteName1 = "Site1";
        var currencyList1 = "USD";
        var ptSportsbook1 = 10.0m;
        var ip1 = "192.168.1.1";

        var siteName2 = "Site2";
        var currencyList2 = "EUR";
        var ptSportsbook2 = 20.0m;
        var ip2 = "192.168.1.2";

        // Act
        var command1 = new InsertCurrencyCommand(siteName1, currencyList1, ptSportsbook1, ip1);
        var command2 = new InsertCurrencyCommand(siteName2, currencyList2, ptSportsbook2, ip2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.SiteName.Should().Be(siteName1);
        command1.CurrencyList.Should().Be(currencyList1);
        command1.PT_Sportsbook.Should().Be(ptSportsbook1);
        command1.IP.Should().Be(ip1);
        command2.SiteName.Should().Be(siteName2);
        command2.CurrencyList.Should().Be(currencyList2);
        command2.PT_Sportsbook.Should().Be(ptSportsbook2);
        command2.IP.Should().Be(ip2);
    }

    [Fact]
    public void ValidationLogic_RequiresAllThreeConditions()
    {
        // Arrange & Act & Assert
        new InsertCurrencyCommand("ValidSite", "ValidCurrency", 1.0m, "IP").IsValid().Should().BeTrue();

        new InsertCurrencyCommand(null, "ValidCurrency", 1.0m, "IP").IsValid().Should().BeFalse();
        new InsertCurrencyCommand("ValidSite", null, 1.0m, "IP").IsValid().Should().BeFalse();
        new InsertCurrencyCommand("ValidSite", "ValidCurrency", 0.0m, "IP").IsValid().Should().BeFalse();
        new InsertCurrencyCommand("", "ValidCurrency", 1.0m, "IP").IsValid().Should().BeFalse();
        new InsertCurrencyCommand("ValidSite", "", 1.0m, "IP").IsValid().Should().BeFalse();
        new InsertCurrencyCommand("ValidSite", "ValidCurrency", -1.0m, "IP").IsValid().Should().BeFalse();
        new InsertCurrencyCommand("   ", "ValidCurrency", 1.0m, "IP").IsValid().Should().BeFalse();
        new InsertCurrencyCommand("ValidSite", "   ", 1.0m, "IP").IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesValidationCondition()
    {
        // Arrange
        var validCommand = new InsertCurrencyCommand("ValidSite", "ValidCurrency", 1.0m, "IP");
        var invalidSiteCommand = new InsertCurrencyCommand(null, "ValidCurrency", 1.0m, "IP");
        var invalidCurrencyCommand = new InsertCurrencyCommand("ValidSite", "", 1.0m, "IP");
        var invalidPTCommand = new InsertCurrencyCommand("ValidSite", "ValidCurrency", 0.0m, "IP");

        // Act & Assert
        validCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(validCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(validCommand.CurrencyList) &&
            validCommand.PT_Sportsbook > 0);

        invalidSiteCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidSiteCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(invalidSiteCommand.CurrencyList) &&
            invalidSiteCommand.PT_Sportsbook > 0);

        invalidCurrencyCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidCurrencyCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(invalidCurrencyCommand.CurrencyList) &&
            invalidCurrencyCommand.PT_Sportsbook > 0);

        invalidPTCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidPTCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(invalidPTCommand.CurrencyList) &&
            invalidPTCommand.PT_Sportsbook > 0);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(1.0)]
    [InlineData(10.5)]
    [InlineData(999.99)]
    [InlineData(99999999.99)]
    public void IsValid_WithPositivePT_Sportsbook_ReturnsTrue(decimal ptSportsbook)
    {
        // Arrange
        var command = new InsertCurrencyCommand("ValidSite", "ValidCurrency", ptSportsbook, "IP");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-0.01)]
    [InlineData(-1.0)]
    [InlineData(-99999999.99)]
    public void IsValid_WithNonPositivePT_Sportsbook_ReturnsFalse(decimal ptSportsbook)
    {
        // Arrange
        var command = new InsertCurrencyCommand("ValidSite", "ValidCurrency", ptSportsbook, "IP");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new InsertCurrencyCommand("TestSite", "USD", 10.5m, "192.168.1.1");
        var initialSiteName = command.SiteName;
        var initialCurrencyList = command.CurrencyList;
        var initialPT = command.PT_Sportsbook;
        var initialIP = command.IP;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods/properties multiple times
        var siteName1 = command.SiteName;
        var currencyList1 = command.CurrencyList;
        var pt1 = command.PT_Sportsbook;
        var ip1 = command.IP;
        var settingKey1 = command.GetSettingKey();
        var valid1 = command.IsValid();

        // Assert - All calls should return same values
        siteName1.Should().Be(initialSiteName);
        currencyList1.Should().Be(initialCurrencyList);
        pt1.Should().Be(initialPT);
        ip1.Should().Be(initialIP);
        settingKey1.Should().Be(initialSettingKey);
        valid1.Should().Be(initialValid);
    }
}