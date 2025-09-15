namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class InsertLicenseeDataCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var siteName = "TestSite";
        var productList = "Product1,Product2";
        var licenseeType = "TypeA";

        // Act
        var command = new InsertLicenseeDataCommand(siteName, productList, licenseeType);

        // Assert
        command.Should().NotBeNull();
        command.SiteName.Should().Be(siteName);
        command.ProductList.Should().Be(productList);
        command.LicenseeType.Should().Be(licenseeType);
    }

    [Fact]
    public void SiteName_IsReadOnly()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");

        // Act & Assert
        var property = typeof(InsertLicenseeDataCommand).GetProperty(nameof(InsertLicenseeDataCommand.SiteName));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void ProductList_IsReadOnly()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");

        // Act & Assert
        var property = typeof(InsertLicenseeDataCommand).GetProperty(nameof(InsertLicenseeDataCommand.ProductList));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void LicenseeType_IsReadOnly()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");

        // Act & Assert
        var property = typeof(InsertLicenseeDataCommand).GetProperty(nameof(InsertLicenseeDataCommand.LicenseeType));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void ErrCode_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");
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
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");
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
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");
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
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_SiteBuilder_LicenseeDB_Ins");
    }

    [Fact]
    public void IsValid_WithAllValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand(null, "Products", "Type");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptySiteName_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("", "Products", "Type");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("   ", "Products", "Type");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullProductList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", null, "Type");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyProductList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "", "Type");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceProductList_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "   ", "Type");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullLicenseeType_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", null);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyLicenseeType_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceLicenseeType_ReturnsFalse()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "   ");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Site1", "Product1", "Type1", true)]
    [InlineData("Site2", "Product1,Product2", "Type2", true)]
    [InlineData("S", "P", "T", true)]
    public void IsValid_WithValidParameters_ReturnsTrue(string siteName, string productList, string licenseeType, bool expected)
    {
        // Arrange
        var command = new InsertLicenseeDataCommand(siteName, productList, licenseeType);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "Products", "Type", false)]
    [InlineData("", "Products", "Type", false)]
    [InlineData("   ", "Products", "Type", false)]
    [InlineData("Site", null, "Type", false)]
    [InlineData("Site", "", "Type", false)]
    [InlineData("Site", "   ", "Type", false)]
    [InlineData("Site", "Products", null, false)]
    [InlineData("Site", "Products", "", false)]
    [InlineData("Site", "Products", "   ", false)]
    public void IsValid_WithInvalidParameters_ReturnsFalse(string siteName, string productList, string licenseeType, bool expected)
    {
        // Arrange
        var command = new InsertLicenseeDataCommand(siteName, productList, licenseeType);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new InsertLicenseeDataCommand("TestSite", "Products", "Type");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new InsertLicenseeDataCommand("Site1", "Products1", "Type1");
        var command2 = new InsertLicenseeDataCommand("Site2", "Products2", "Type2");

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("API_SiteBuilder_LicenseeDB_Ins");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var siteName1 = "Site1";
        var productList1 = "Products1";
        var licenseeType1 = "Type1";
        var siteName2 = "Site2";
        var productList2 = "Products2";
        var licenseeType2 = "Type2";

        // Act
        var command1 = new InsertLicenseeDataCommand(siteName1, productList1, licenseeType1);
        var command2 = new InsertLicenseeDataCommand(siteName2, productList2, licenseeType2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.SiteName.Should().Be(siteName1);
        command1.ProductList.Should().Be(productList1);
        command1.LicenseeType.Should().Be(licenseeType1);
        command2.SiteName.Should().Be(siteName2);
        command2.ProductList.Should().Be(productList2);
        command2.LicenseeType.Should().Be(licenseeType2);
    }

    [Fact]
    public void ValidationLogic_RequiresAllParametersToBeNonNullOrWhiteSpace()
    {
        // Arrange & Act & Assert
        new InsertLicenseeDataCommand("ValidSite", "ValidProducts", "ValidType").IsValid().Should().BeTrue();
        new InsertLicenseeDataCommand("A", "B", "C").IsValid().Should().BeTrue();

        new InsertLicenseeDataCommand(null, "Products", "Type").IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("Site", null, "Type").IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("Site", "Products", null).IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("", "Products", "Type").IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("Site", "", "Type").IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("Site", "Products", "").IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("   ", "Products", "Type").IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("Site", "   ", "Type").IsValid().Should().BeFalse();
        new InsertLicenseeDataCommand("Site", "Products", "   ").IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesStringValidationLogic()
    {
        // Arrange
        var validCommand = new InsertLicenseeDataCommand("ValidSite", "ValidProducts", "ValidType");
        var invalidSiteCommand = new InsertLicenseeDataCommand(null, "Products", "Type");
        var invalidProductCommand = new InsertLicenseeDataCommand("Site", "", "Type");
        var invalidTypeCommand = new InsertLicenseeDataCommand("Site", "Products", "   ");

        // Act & Assert
        validCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(validCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(validCommand.ProductList) &&
            !string.IsNullOrWhiteSpace(validCommand.LicenseeType));

        invalidSiteCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidSiteCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(invalidSiteCommand.ProductList) &&
            !string.IsNullOrWhiteSpace(invalidSiteCommand.LicenseeType));

        invalidProductCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidProductCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(invalidProductCommand.ProductList) &&
            !string.IsNullOrWhiteSpace(invalidProductCommand.LicenseeType));

        invalidTypeCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidTypeCommand.SiteName) &&
            !string.IsNullOrWhiteSpace(invalidTypeCommand.ProductList) &&
            !string.IsNullOrWhiteSpace(invalidTypeCommand.LicenseeType));
    }
}