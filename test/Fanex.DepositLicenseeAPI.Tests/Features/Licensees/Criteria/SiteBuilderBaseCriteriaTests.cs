namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class SiteBuilderBaseCriteriaTests
{
    [Fact]
    public void Constructor_WithValidParams_InitializesCorrectly()
    {
        // Arrange
        var parameters = CreateValidParams();

        // Act
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Assert
        criteria.Should().NotBeNull();
        criteria.CustDBGroupId.Should().Be(parameters.CustDBGroupId);
        criteria.Customers.Should().Be(parameters.Customers);
        criteria.ProductBetSetting.Should().Be(parameters.ProductBetSetting);
        criteria.ProductPT.Should().Be(parameters.ProductPT);
        criteria.ProductStakeLevel.Should().Be(parameters.ProductStakeLevel);
        criteria.ProductComm.Should().Be(parameters.ProductComm);
        criteria.Creator.Should().Be(parameters.Creator);
        criteria.IP.Should().Be(parameters.IP);
        criteria.IsCreateSub.Should().Be(parameters.IsCreateSub);
        criteria.LicenseeType.Should().Be(parameters.LicenseeType);
        criteria.SiteId.Should().Be(parameters.SiteId);
        criteria.SiteName.Should().Be(parameters.SiteName);
        criteria.SettingStatus.Should().Be(parameters.SettingStatus);
    }

    [Fact]
    public void CustDBGroupId_IsReadOnly()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act & Assert
        var property = typeof(SiteBuilderBaseCriteria).GetProperty(nameof(SiteBuilderBaseCriteria.CustDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void Customers_IsReadOnly()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act & Assert
        var property = typeof(SiteBuilderBaseCriteria).GetProperty(nameof(SiteBuilderBaseCriteria.Customers));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().Be($"SiteDB{parameters.CustDBGroupId}_API_SiteBuilder_Licensee_Insert");
    }

    [Fact]
    public void GetSettingKey_WithZeroCustDBGroupId_ReturnsBaseName()
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: 0,
            Customers: "customer1,customer2",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().Be("API_SiteBuilder_Licensee_Insert");
    }

    [Fact]
    public void IsValid_WithValidCustomers_ReturnsTrue()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullCustomers_ReturnsFalse()
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: null,
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyCustomers_ReturnsFalse()
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: "",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("customer1", true)]
    [InlineData("customer1,customer2", true)]
    [InlineData("a", true)]
    [InlineData("   customer   ", true)] // Not trimmed, just not null/empty
    public void IsValid_WithVariousValidCustomers_ReturnsTrue(string customers, bool expected)
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: customers,
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValid_WithInvalidCustomers_ReturnsFalse(string customers, bool expected)
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: customers,
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void MutableProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        criteria.ErrCode = 500;
        criteria.ErrMsg = "Test error message";
        criteria.ErrExtraInfo = "Extra error info";
        criteria.AgentList = "agent1,agent2";
        criteria.InsBalExceptCustList = "cust1,cust2";
        criteria.FailedCurrencies = "USD,EUR";
        criteria.ListSuperRollBack = "rollback1,rollback2";

        // Assert
        criteria.ErrCode.Should().Be(500);
        criteria.ErrMsg.Should().Be("Test error message");
        criteria.ErrExtraInfo.Should().Be("Extra error info");
        criteria.AgentList.Should().Be("agent1,agent2");
        criteria.InsBalExceptCustList.Should().Be("cust1,cust2");
        criteria.FailedCurrencies.Should().Be("USD,EUR");
        criteria.ListSuperRollBack.Should().Be("rollback1,rollback2");
    }

    [Fact]
    public void MutableProperties_DefaultValues()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act & Assert
        criteria.ErrCode.Should().Be(0);
        criteria.ErrMsg.Should().BeNull();
        criteria.ErrExtraInfo.Should().BeNull();
        criteria.AgentList.Should().BeNull();
        criteria.InsBalExceptCustList.Should().BeNull();
        criteria.FailedCurrencies.Should().BeNull();
        criteria.ListSuperRollBack.Should().BeNull();
    }

    [Fact]
    public void InheritsFromCriteriaBase()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act & Assert
        criteria.Should().BeAssignableTo<CriteriaBase>();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var parameters1 = CreateValidParams();
        var parameters2 = new SiteBuilderParams(
            CustDBGroupId: 2,
            Customers: "customer3,customer4",
            ProductBetSetting: "setting2",
            ProductPT: "pt2",
            ProductStakeLevel: "level2",
            ProductComm: "comm2",
            Creator: "testuser2",
            IP: "192.168.1.2",
            IsCreateSub: false,
            LicenseeType: "type2",
            SiteId: 456,
            SiteName: "TestSite2",
            SettingStatus: 2
        );

        // Act
        var criteria1 = new TestSiteBuilderBaseCriteria(parameters1);
        var criteria2 = new TestSiteBuilderBaseCriteria(parameters2);

        // Assert
        criteria1.Should().NotBeSameAs(criteria2);
        criteria1.CustDBGroupId.Should().Be(parameters1.CustDBGroupId);
        criteria1.SiteName.Should().Be(parameters1.SiteName);
        criteria2.CustDBGroupId.Should().Be(parameters2.CustDBGroupId);
        criteria2.SiteName.Should().Be(parameters2.SiteName);
    }

    [Theory]
    [InlineData(0, 123, "TestSite")]
    [InlineData(1, 456, "AnotherSite")]
    [InlineData(999, 789, "ThirdSite")]
    [InlineData(-1, -123, "NegativeSite")]
    public void Constructor_WithVariousParameters_InitializesCorrectly(int custDBGroupId, int siteId, string siteName)
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: custDBGroupId,
            Customers: "customer1",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: siteId,
            SiteName: siteName,
            SettingStatus: 1
        );

        // Act
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Assert
        criteria.CustDBGroupId.Should().Be(custDBGroupId);
        criteria.SiteId.Should().Be(siteId);
        criteria.SiteName.Should().Be(siteName);
    }

    [Fact]
    public void ValidationLogic_OnlyDependsOnCustomers()
    {
        // Arrange
        var validParams = CreateValidParams();
        var invalidParams = new SiteBuilderParams(
            CustDBGroupId: validParams.CustDBGroupId,
            Customers: null,
            ProductBetSetting: validParams.ProductBetSetting,
            ProductPT: validParams.ProductPT,
            ProductStakeLevel: validParams.ProductStakeLevel,
            ProductComm: validParams.ProductComm,
            Creator: validParams.Creator,
            IP: validParams.IP,
            IsCreateSub: validParams.IsCreateSub,
            LicenseeType: validParams.LicenseeType,
            SiteId: validParams.SiteId,
            SiteName: validParams.SiteName,
            SettingStatus: validParams.SettingStatus
        );

        var validCriteria = new TestSiteBuilderBaseCriteria(validParams);
        var invalidCriteria = new TestSiteBuilderBaseCriteria(invalidParams);

        // Act & Assert
        validCriteria.IsValid().Should().BeTrue();
        invalidCriteria.IsValid().Should().BeFalse();

        // Validation should be deterministic
        validCriteria.IsValid().Should().Be(validCriteria.IsValid());
        invalidCriteria.IsValid().Should().Be(invalidCriteria.IsValid());
    }

    [Fact]
    public void IsValid_MatchesConditionNotNullOrEmpty()
    {
        // Arrange
        var validCriteria = new TestSiteBuilderBaseCriteria(new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: "validcustomer",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        ));

        var nullCriteria = new TestSiteBuilderBaseCriteria(new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: null,
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        ));

        var emptyCriteria = new TestSiteBuilderBaseCriteria(new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: "",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        ));

        // Act & Assert
        validCriteria.IsValid().Should().Be(!string.IsNullOrEmpty(validCriteria.Customers));
        nullCriteria.IsValid().Should().Be(!string.IsNullOrEmpty(nullCriteria.Customers));
        emptyCriteria.IsValid().Should().Be(!string.IsNullOrEmpty(emptyCriteria.Customers));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsCreateSub_ReflectsParameterValue(bool isCreateSub)
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: "customer1",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: isCreateSub,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act & Assert
        criteria.IsCreateSub.Should().Be(isCreateSub);
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)255)]
    public void SettingStatus_ReflectsParameterValue(byte settingStatus)
    {
        // Arrange
        var parameters = new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: "customer1",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: settingStatus
        );
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act & Assert
        criteria.SettingStatus.Should().Be(settingStatus);
    }

    [Fact]
    public void AllReadOnlyProperties_AreConsistent()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act - Call properties multiple times
        var custDBGroupId1 = criteria.CustDBGroupId;
        var custDBGroupId2 = criteria.CustDBGroupId;
        var customers1 = criteria.Customers;
        var customers2 = criteria.Customers;
        var settingKey1 = criteria.GetSettingKey();
        var settingKey2 = criteria.GetSettingKey();

        // Assert - All calls should return same values
        custDBGroupId1.Should().Be(custDBGroupId2);
        customers1.Should().Be(customers2);
        settingKey1.Should().Be(settingKey2);

        custDBGroupId1.Should().Be(parameters.CustDBGroupId);
        customers1.Should().Be(parameters.Customers);
    }

    [Fact]
    public void AllMutableProperties_CanBeSetToNull()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Act
        criteria.ErrMsg = null;
        criteria.ErrExtraInfo = null;
        criteria.AgentList = null;
        criteria.InsBalExceptCustList = null;
        criteria.FailedCurrencies = null;
        criteria.ListSuperRollBack = null;

        // Assert
        criteria.ErrMsg.Should().BeNull();
        criteria.ErrExtraInfo.Should().BeNull();
        criteria.AgentList.Should().BeNull();
        criteria.InsBalExceptCustList.Should().BeNull();
        criteria.FailedCurrencies.Should().BeNull();
        criteria.ListSuperRollBack.Should().BeNull();
    }

    [Fact]
    public void SiteBuilderParams_IsRecord()
    {
        // Arrange
        var parameters1 = CreateValidParams();
        var parameters2 = CreateValidParams();

        // Act & Assert
        parameters1.Should().Be(parameters2);
        parameters1.Should().NotBeSameAs(parameters2);
        parameters1.GetHashCode().Should().Be(parameters2.GetHashCode());
    }

    [Fact]
    public void Constructor_StoresParametersCorrectly()
    {
        // Arrange
        var parameters = CreateValidParams();

        // Act
        var criteria = new TestSiteBuilderBaseCriteria(parameters);

        // Assert
        // Verify all parameters are correctly accessible through properties
        criteria.CustDBGroupId.Should().Be(parameters.CustDBGroupId);
        criteria.Customers.Should().Be(parameters.Customers);
        criteria.ProductBetSetting.Should().Be(parameters.ProductBetSetting);
        criteria.ProductPT.Should().Be(parameters.ProductPT);
        criteria.ProductStakeLevel.Should().Be(parameters.ProductStakeLevel);
        criteria.ProductComm.Should().Be(parameters.ProductComm);
        criteria.Creator.Should().Be(parameters.Creator);
        criteria.IP.Should().Be(parameters.IP);
        criteria.IsCreateSub.Should().Be(parameters.IsCreateSub);
        criteria.LicenseeType.Should().Be(parameters.LicenseeType);
        criteria.SiteId.Should().Be(parameters.SiteId);
        criteria.SiteName.Should().Be(parameters.SiteName);
        criteria.SettingStatus.Should().Be(parameters.SettingStatus);
    }

    private SiteBuilderParams CreateValidParams()
    {
        return new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: "customer1,customer2",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: true,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );
    }

    private class TestSiteBuilderBaseCriteria : SiteBuilderBaseCriteria
    {
        public TestSiteBuilderBaseCriteria(SiteBuilderParams parameters) : base(parameters)
        {
        }
    }
}