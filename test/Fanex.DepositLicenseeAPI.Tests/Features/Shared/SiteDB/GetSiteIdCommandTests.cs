namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared.SiteDB;

using Data.Repository;
using DepositLicenseeAPI.Features.Shared.SiteDB;

public class GetSiteIdCommandTests
{
    [Fact]
    public void Constructor_WithValidSiteName_InitializesCorrectly()
    {
        // Arrange
        var siteName = "TestSite";

        // Act
        var command = new GetSiteIdCommand(siteName);

        // Assert
        command.Should().NotBeNull();
        command.SiteName.Should().Be(siteName);
    }

    [Fact]
    public void SiteName_IsReadOnly()
    {
        // Arrange
        var siteName = "TestSite";
        var command = new GetSiteIdCommand(siteName);

        // Act & Assert
        command.SiteName.Should().Be(siteName);
        var property = typeof(GetSiteIdCommand).GetProperty(nameof(GetSiteIdCommand.SiteName));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void SiteId_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new GetSiteIdCommand("TestSite");
        var expectedSiteId = "SITE001";

        // Act
        command.SiteId = expectedSiteId;

        // Assert
        command.SiteId.Should().Be(expectedSiteId);
    }

    [Fact]
    public void SiteId_DefaultValueIsNull()
    {
        // Arrange
        var command = new GetSiteIdCommand("TestSite");

        // Act & Assert
        command.SiteId.Should().BeNull();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var command = new GetSiteIdCommand("TestSite");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_Dep_GetSiteID_LicVendorProfile");
    }

    [Fact]
    public void IsValid_WithValidSiteName_ReturnsTrue()
    {
        // Arrange
        var command = new GetSiteIdCommand("TestSite");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new GetSiteIdCommand(null);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptySiteName_ReturnsFalse()
    {
        // Arrange
        var command = new GetSiteIdCommand("");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new GetSiteIdCommand("   ");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Site1", true)]
    [InlineData("Site2", true)]
    [InlineData("LongSiteName", true)]
    [InlineData("S", true)]
    public void IsValid_WithValidSiteNames_ReturnsTrue(string siteName, bool expected)
    {
        // Arrange
        var command = new GetSiteIdCommand(siteName);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("   ", false)]
    [InlineData("\t", false)]
    [InlineData("\n", false)]
    public void IsValid_WithInvalidSiteNames_ReturnsFalse(string siteName, bool expected)
    {
        // Arrange
        var command = new GetSiteIdCommand(siteName);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new GetSiteIdCommand("TestSite");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var command1 = new GetSiteIdCommand("Site1");
        var command2 = new GetSiteIdCommand("Site2");

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("API_Dep_GetSiteID_LicVendorProfile");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var siteName1 = "Site1";
        var siteName2 = "Site2";

        // Act
        var command1 = new GetSiteIdCommand(siteName1);
        var command2 = new GetSiteIdCommand(siteName2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.SiteName.Should().Be(siteName1);
        command2.SiteName.Should().Be(siteName2);
        command1.SiteName.Should().NotBe(command2.SiteName);
    }

    [Fact]
    public void SiteId_CanBeModifiedAfterConstruction()
    {
        // Arrange
        var command = new GetSiteIdCommand("TestSite");
        var firstSiteId = "SITE001";
        var secondSiteId = "SITE002";

        // Act
        command.SiteId = firstSiteId;
        var firstResult = command.SiteId;

        command.SiteId = secondSiteId;
        var secondResult = command.SiteId;

        // Assert
        firstResult.Should().Be(firstSiteId);
        secondResult.Should().Be(secondSiteId);
        secondResult.Should().NotBe(firstResult);
    }

    [Fact]
    public void ValidationLogic_IsBasedOnNotNullOrWhiteSpace()
    {
        // Arrange & Act & Assert
        new GetSiteIdCommand("ValidSite").IsValid().Should().BeTrue();
        new GetSiteIdCommand("A").IsValid().Should().BeTrue();
        new GetSiteIdCommand("Site123").IsValid().Should().BeTrue();

        new GetSiteIdCommand(null).IsValid().Should().BeFalse();
        new GetSiteIdCommand("").IsValid().Should().BeFalse();
        new GetSiteIdCommand(" ").IsValid().Should().BeFalse();
        new GetSiteIdCommand("\t\n").IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesStringIsNullOrWhiteSpaceNegation()
    {
        // Arrange
        var validCommand = new GetSiteIdCommand("ValidSite");
        var nullCommand = new GetSiteIdCommand(null);
        var emptyCommand = new GetSiteIdCommand("");
        var whitespaceCommand = new GetSiteIdCommand("   ");

        // Act & Assert
        validCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(validCommand.SiteName));
        nullCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(nullCommand.SiteName));
        emptyCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(emptyCommand.SiteName));
        whitespaceCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(whitespaceCommand.SiteName));
    }
}