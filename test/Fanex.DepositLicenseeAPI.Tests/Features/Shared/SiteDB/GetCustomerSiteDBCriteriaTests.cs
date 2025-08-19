namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared.SiteDB;

using Data.Repository;
using DepositLicenseeAPI.Features.Shared.SiteDB;

public class GetCustomerSiteDBCriteriaTests
{
    [Fact]
    public void Constructor_WithValidSiteDBGroupId_InitializesCorrectly()
    {
        // Arrange
        var siteDBGroupId = 123;

        // Act
        var criteria = new GetCustomerSiteDBCriteria(siteDBGroupId);

        // Assert
        criteria.Should().NotBeNull();
        criteria.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Fact]
    public void SiteDBGroupId_IsReadOnly()
    {
        // Arrange
        var siteDBGroupId = 456;
        var criteria = new GetCustomerSiteDBCriteria(siteDBGroupId);

        // Act & Assert
        criteria.SiteDBGroupId.Should().Be(siteDBGroupId);
        var property = typeof(GetCustomerSiteDBCriteria).GetProperty(nameof(GetCustomerSiteDBCriteria.SiteDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var criteria = new GetCustomerSiteDBCriteria(123);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().Be("API_SiteBuilder_GetCustomerSiteDB");
    }

    [Fact]
    public void IsValid_AlwaysReturnsTrue()
    {
        // Arrange
        var criteria = new GetCustomerSiteDBCriteria(123);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void IsValid_WithVariousSiteDBGroupIds_AlwaysReturnsTrue(int siteDBGroupId)
    {
        // Arrange
        var criteria = new GetCustomerSiteDBCriteria(siteDBGroupId);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void Constructor_WithPositiveValues_SetsPropertyCorrectly(int siteDBGroupId)
    {
        // Arrange & Act
        var criteria = new GetCustomerSiteDBCriteria(siteDBGroupId);

        // Assert
        criteria.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(int.MinValue)]
    public void Constructor_WithNonPositiveValues_SetsPropertyCorrectly(int siteDBGroupId)
    {
        // Arrange & Act
        var criteria = new GetCustomerSiteDBCriteria(siteDBGroupId);

        // Assert
        criteria.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Fact]
    public void InheritsFromCriteriaBase()
    {
        // Arrange
        var criteria = new GetCustomerSiteDBCriteria(123);

        // Act & Assert
        criteria.Should().BeAssignableTo<CriteriaBase>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var criteria1 = new GetCustomerSiteDBCriteria(100);
        var criteria2 = new GetCustomerSiteDBCriteria(200);

        // Act
        var result1 = criteria1.GetSettingKey();
        var result2 = criteria2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("API_SiteBuilder_GetCustomerSiteDB");
    }

    [Fact]
    public void IsValid_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var criteria1 = new GetCustomerSiteDBCriteria(100);
        var criteria2 = new GetCustomerSiteDBCriteria(200);

        // Act
        var result1 = criteria1.IsValid();
        var result2 = criteria2.IsValid();

        // Assert
        result1.Should().Be(result2);
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var siteDBGroupId1 = 111;
        var siteDBGroupId2 = 222;

        // Act
        var criteria1 = new GetCustomerSiteDBCriteria(siteDBGroupId1);
        var criteria2 = new GetCustomerSiteDBCriteria(siteDBGroupId2);

        // Assert
        criteria1.Should().NotBeSameAs(criteria2);
        criteria1.SiteDBGroupId.Should().Be(siteDBGroupId1);
        criteria2.SiteDBGroupId.Should().Be(siteDBGroupId2);
        criteria1.SiteDBGroupId.Should().NotBe(criteria2.SiteDBGroupId);
    }
}