namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared.SiteDB;

using Data.Repository;
using DepositLicenseeAPI.Features.Shared.SiteDB;

public class GetSiteDBConfigByCustCriteriaTests
{
    [Fact]
    public void Constructor_WithValidCustId_InitializesCorrectly()
    {
        // Arrange
        var custId = 123;

        // Act
        var criteria = new GetSiteDBConfigByCustCriteria(custId);

        // Assert
        criteria.Should().NotBeNull();
        criteria.CustId.Should().Be(custId);
    }

    [Fact]
    public void CustId_IsReadOnly()
    {
        // Arrange
        var custId = 456;
        var criteria = new GetSiteDBConfigByCustCriteria(custId);

        // Act & Assert
        criteria.CustId.Should().Be(custId);
        var property = typeof(GetSiteDBConfigByCustCriteria).GetProperty(nameof(GetSiteDBConfigByCustCriteria.CustId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var criteria = new GetSiteDBConfigByCustCriteria(123);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().Be("Acc_Dep_ServerDBConfigSelByCust");
    }

    [Fact]
    public void IsValid_WithPositiveCustId_ReturnsTrue()
    {
        // Arrange
        var criteria = new GetSiteDBConfigByCustCriteria(123);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithZeroCustId_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetSiteDBConfigByCustCriteria(0);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativeCustId_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetSiteDBConfigByCustCriteria(-1);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(999, true)]
    [InlineData(int.MaxValue, true)]
    public void IsValid_WithPositiveValues_ReturnsTrue(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetSiteDBConfigByCustCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(-100, false)]
    [InlineData(int.MinValue, false)]
    public void IsValid_WithNonPositiveValues_ReturnsFalse(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetSiteDBConfigByCustCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void Constructor_WithPositiveValues_SetsPropertyCorrectly(int custId)
    {
        // Arrange & Act
        var criteria = new GetSiteDBConfigByCustCriteria(custId);

        // Assert
        criteria.CustId.Should().Be(custId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(int.MinValue)]
    public void Constructor_WithNonPositiveValues_SetsPropertyCorrectly(int custId)
    {
        // Arrange & Act
        var criteria = new GetSiteDBConfigByCustCriteria(custId);

        // Assert
        criteria.CustId.Should().Be(custId);
    }

    [Fact]
    public void InheritsFromCriteriaBase()
    {
        // Arrange
        var criteria = new GetSiteDBConfigByCustCriteria(123);

        // Act & Assert
        criteria.Should().BeAssignableTo<CriteriaBase>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var criteria1 = new GetSiteDBConfigByCustCriteria(100);
        var criteria2 = new GetSiteDBConfigByCustCriteria(200);

        // Act
        var result1 = criteria1.GetSettingKey();
        var result2 = criteria2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("Acc_Dep_ServerDBConfigSelByCust");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var custId1 = 111;
        var custId2 = 222;

        // Act
        var criteria1 = new GetSiteDBConfigByCustCriteria(custId1);
        var criteria2 = new GetSiteDBConfigByCustCriteria(custId2);

        // Assert
        criteria1.Should().NotBeSameAs(criteria2);
        criteria1.CustId.Should().Be(custId1);
        criteria2.CustId.Should().Be(custId2);
        criteria1.CustId.Should().NotBe(criteria2.CustId);
    }

    [Fact]
    public void ValidationLogic_IsBasedOnCustIdGreaterThanZero()
    {
        // Arrange & Act & Assert
        new GetSiteDBConfigByCustCriteria(1).IsValid().Should().BeTrue();
        new GetSiteDBConfigByCustCriteria(2).IsValid().Should().BeTrue();
        new GetSiteDBConfigByCustCriteria(1000).IsValid().Should().BeTrue();

        new GetSiteDBConfigByCustCriteria(0).IsValid().Should().BeFalse();
        new GetSiteDBConfigByCustCriteria(-1).IsValid().Should().BeFalse();
        new GetSiteDBConfigByCustCriteria(-1000).IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesConditionCustIdGreaterThanZero()
    {
        // Arrange
        var positiveCriteria = new GetSiteDBConfigByCustCriteria(42);
        var zeroCriteria = new GetSiteDBConfigByCustCriteria(0);
        var negativeCriteria = new GetSiteDBConfigByCustCriteria(-42);

        // Act & Assert
        positiveCriteria.IsValid().Should().Be(positiveCriteria.CustId > 0);
        zeroCriteria.IsValid().Should().Be(zeroCriteria.CustId > 0);
        negativeCriteria.IsValid().Should().Be(negativeCriteria.CustId > 0);
    }
}