namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared.SiteDB;

using Data.Repository;
using DepositLicenseeAPI.Features.Shared.SiteDB;

public class GetSiteDBConfigCriteriaTests
{
    [Fact]
    public void Constructor_WithValidSite_InitializesCorrectly()
    {
        // Arrange
        var site = "TestSite";

        // Act
        var criteria = new GetSiteDBConfigCriteria(site);

        // Assert
        criteria.Should().NotBeNull();
        criteria.Site.Should().Be(site);
    }

    [Fact]
    public void Site_IsReadOnly()
    {
        // Arrange
        var site = "TestSite";
        var criteria = new GetSiteDBConfigCriteria(site);

        // Act & Assert
        criteria.Site.Should().Be(site);
        var property = typeof(GetSiteDBConfigCriteria).GetProperty(nameof(GetSiteDBConfigCriteria.Site));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria("TestSite");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().Be("Acc_ServerDBConfigSel");
    }

    [Fact]
    public void IsValid_WithValidSite_ReturnsTrue()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria("TestSite");

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullSite_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria(null);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptySite_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria("");

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceSite_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria("   ");

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Site1", true)]
    [InlineData("Site2", true)]
    [InlineData("LongSiteName", true)]
    [InlineData("S", true)]
    public void IsValid_WithValidSiteNames_ReturnsTrue(string site, bool expected)
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria(site);

        // Act
        var result = criteria.IsValid();

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
    public void IsValid_WithInvalidSiteNames_ReturnsFalse(string site, bool expected)
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria(site);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromCriteriaBase()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria("TestSite");

        // Act & Assert
        criteria.Should().BeAssignableTo<CriteriaBase>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var criteria1 = new GetSiteDBConfigCriteria("Site1");
        var criteria2 = new GetSiteDBConfigCriteria("Site2");

        // Act
        var result1 = criteria1.GetSettingKey();
        var result2 = criteria2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("Acc_ServerDBConfigSel");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var site1 = "Site1";
        var site2 = "Site2";

        // Act
        var criteria1 = new GetSiteDBConfigCriteria(site1);
        var criteria2 = new GetSiteDBConfigCriteria(site2);

        // Assert
        criteria1.Should().NotBeSameAs(criteria2);
        criteria1.Site.Should().Be(site1);
        criteria2.Site.Should().Be(site2);
        criteria1.Site.Should().NotBe(criteria2.Site);
    }

    [Fact]
    public void ValidationLogic_IsBasedOnNotNullOrWhiteSpace()
    {
        // Arrange & Act & Assert
        new GetSiteDBConfigCriteria("ValidSite").IsValid().Should().BeTrue();
        new GetSiteDBConfigCriteria("A").IsValid().Should().BeTrue();
        new GetSiteDBConfigCriteria("Site123").IsValid().Should().BeTrue();

        new GetSiteDBConfigCriteria(null).IsValid().Should().BeFalse();
        new GetSiteDBConfigCriteria("").IsValid().Should().BeFalse();
        new GetSiteDBConfigCriteria(" ").IsValid().Should().BeFalse();
        new GetSiteDBConfigCriteria("\t\n").IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesStringIsNullOrWhiteSpaceNegation()
    {
        // Arrange
        var validCriteria = new GetSiteDBConfigCriteria("ValidSite");
        var nullCriteria = new GetSiteDBConfigCriteria(null);
        var emptyCriteria = new GetSiteDBConfigCriteria("");
        var whitespaceCriteria = new GetSiteDBConfigCriteria("   ");

        // Act & Assert
        validCriteria.IsValid().Should().Be(!string.IsNullOrWhiteSpace(validCriteria.Site));
        nullCriteria.IsValid().Should().Be(!string.IsNullOrWhiteSpace(nullCriteria.Site));
        emptyCriteria.IsValid().Should().Be(!string.IsNullOrWhiteSpace(emptyCriteria.Site));
        whitespaceCriteria.IsValid().Should().Be(!string.IsNullOrWhiteSpace(whitespaceCriteria.Site));
    }
}