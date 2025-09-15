namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using DepositLicenseeAPI.Features.Licensees;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class CreateCurrencyCriteriaTests
{
    [Fact]
    public void Constructor_WithValidParams_InitializesCorrectly()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();

        // Act
        var criteria = new CreateCurrencyCriteria(parameters);

        // Assert
        criteria.Should().NotBeNull();
        criteria.Mode.Should().Be(CreateMode.NewCurrency);
    }

    [Fact]
    public void Mode_AlwaysReturnsNewCurrency()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        var result = criteria.Mode;

        // Assert
        result.Should().Be(CreateMode.NewCurrency);
        result.Should().Be(2);
    }

    [Fact]
    public void CurrencyList_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);
        var expectedCurrencyList = "USD,EUR,GBP";

        // Act
        criteria.CurrencyList = expectedCurrencyList;

        // Assert
        criteria.CurrencyList.Should().Be(expectedCurrencyList);
    }

    [Fact]
    public void CurrencyList_DefaultValueIsNull()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act & Assert
        criteria.CurrencyList.Should().BeNull();
    }

    [Fact]
    public void PT_Sportsbook_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);
        var expectedPT = 10.5m;

        // Act
        criteria.PT_Sportsbook = expectedPT;

        // Assert
        criteria.PT_Sportsbook.Should().Be(expectedPT);
    }

    [Fact]
    public void PT_Sportsbook_DefaultValueIsZero()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act & Assert
        criteria.PT_Sportsbook.Should().Be(0m);
    }

    [Fact]
    public void InheritsFromSiteBuilderBaseCriteria()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();

        // Act
        var criteria = new CreateCurrencyCriteria(parameters);

        // Assert
        criteria.Should().BeAssignableTo<SiteBuilderBaseCriteria>();
        criteria.CustDBGroupId.Should().Be(parameters.CustDBGroupId);
        criteria.Customers.Should().Be(parameters.Customers);
        criteria.Creator.Should().Be(parameters.Creator);
        criteria.IP.Should().Be(parameters.IP);
        criteria.SiteId.Should().Be(parameters.SiteId);
        criteria.SiteName.Should().Be(parameters.SiteName);
    }

    [Fact]
    public void BasePropertiesAreAccessible()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act & Assert
        criteria.ProductBetSetting.Should().Be(parameters.ProductBetSetting);
        criteria.ProductPT.Should().Be(parameters.ProductPT);
        criteria.ProductStakeLevel.Should().Be(parameters.ProductStakeLevel);
        criteria.ProductComm.Should().Be(parameters.ProductComm);
        criteria.IsCreateSub.Should().Be(parameters.IsCreateSub);
        criteria.LicenseeType.Should().Be(parameters.LicenseeType);
        criteria.SettingStatus.Should().Be(parameters.SettingStatus);
    }

    [Fact]
    public void ErrorProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        criteria.ErrCode = 500;
        criteria.ErrMsg = "Test error message";
        criteria.ErrExtraInfo = "Extra error info";

        // Assert
        criteria.ErrCode.Should().Be(500);
        criteria.ErrMsg.Should().Be("Test error message");
        criteria.ErrExtraInfo.Should().Be("Extra error info");
    }

    [Fact]
    public void ListProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        criteria.AgentList = "agent1,agent2";
        criteria.InsBalExceptCustList = "cust1,cust2";
        criteria.FailedCurrencies = "USD,EUR";
        criteria.ListSuperRollBack = "rollback1,rollback2";

        // Assert
        criteria.AgentList.Should().Be("agent1,agent2");
        criteria.InsBalExceptCustList.Should().Be("cust1,cust2");
        criteria.FailedCurrencies.Should().Be("USD,EUR");
        criteria.ListSuperRollBack.Should().Be("rollback1,rollback2");
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("USD,EUR")]
    [InlineData("USD,EUR,GBP,JPY")]
    [InlineData("")]
    [InlineData(null)]
    public void CurrencyList_AcceptsVariousValues(string currencyList)
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        criteria.CurrencyList = currencyList;

        // Assert
        criteria.CurrencyList.Should().Be(currencyList);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.01)]
    [InlineData(1.0)]
    [InlineData(10.5)]
    [InlineData(999.99)]
    [InlineData(-1.0)]
    public void PT_Sportsbook_AcceptsVariousValues(decimal ptSportsbook)
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        criteria.PT_Sportsbook = ptSportsbook;

        // Assert
        criteria.PT_Sportsbook.Should().Be(ptSportsbook);
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().Contain("API_SiteBuilder_Licensee_Insert");
        result.Should().Contain(parameters.CustDBGroupId.ToString());
    }

    [Fact]
    public void IsValid_WithValidCustomers_ReturnsTrue()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

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
        var criteria = new CreateCurrencyCriteria(parameters);

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
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var parameters1 = CreateValidSiteBuilderParams();
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
        var criteria1 = new CreateCurrencyCriteria(parameters1);
        var criteria2 = new CreateCurrencyCriteria(parameters2);

        // Assert
        criteria1.Should().NotBeSameAs(criteria2);
        criteria1.CustDBGroupId.Should().Be(parameters1.CustDBGroupId);
        criteria1.SiteName.Should().Be(parameters1.SiteName);
        criteria2.CustDBGroupId.Should().Be(parameters2.CustDBGroupId);
        criteria2.SiteName.Should().Be(parameters2.SiteName);
    }

    [Fact]
    public void Mode_IsConsistentAcrossInstances()
    {
        // Arrange
        var parameters1 = CreateValidSiteBuilderParams();
        var parameters2 = CreateValidSiteBuilderParams();

        // Act
        var criteria1 = new CreateCurrencyCriteria(parameters1);
        var criteria2 = new CreateCurrencyCriteria(parameters2);

        // Assert
        criteria1.Mode.Should().Be(criteria2.Mode);
        criteria1.Mode.Should().Be(CreateMode.NewCurrency);
        criteria2.Mode.Should().Be(CreateMode.NewCurrency);
    }

    [Fact]
    public void MutableProperties_CanBeModifiedIndependently()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        criteria.CurrencyList = "USD,EUR";
        criteria.PT_Sportsbook = 15.5m;
        criteria.ErrCode = 404;
        criteria.ErrMsg = "Error occurred";

        // Assert
        criteria.CurrencyList.Should().Be("USD,EUR");
        criteria.PT_Sportsbook.Should().Be(15.5m);
        criteria.ErrCode.Should().Be(404);
        criteria.ErrMsg.Should().Be("Error occurred");

        // Base properties should remain unchanged
        criteria.SiteName.Should().Be(parameters.SiteName);
        criteria.Mode.Should().Be(CreateMode.NewCurrency);
        criteria.IsValid().Should().BeTrue();
    }

    [Fact]
    public void ValidationLogic_InheritsFromBaseClass()
    {
        // Arrange
        var validParameters = CreateValidSiteBuilderParams();
        var invalidParameters = new SiteBuilderParams(
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

        // Act
        var validCriteria = new CreateCurrencyCriteria(validParameters);
        var invalidCriteria = new CreateCurrencyCriteria(invalidParameters);

        // Assert
        validCriteria.IsValid().Should().BeTrue();
        invalidCriteria.IsValid().Should().BeFalse();
    }

    [Fact]
    public void Properties_DefaultValues_AreCorrect()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act & Assert
        criteria.CurrencyList.Should().BeNull();
        criteria.PT_Sportsbook.Should().Be(0m);
        criteria.ErrCode.Should().Be(0);
        criteria.ErrMsg.Should().BeNull();
        criteria.ErrExtraInfo.Should().BeNull();
        criteria.AgentList.Should().BeNull();
        criteria.InsBalExceptCustList.Should().BeNull();
        criteria.FailedCurrencies.Should().BeNull();
        criteria.ListSuperRollBack.Should().BeNull();
    }

    [Fact]
    public void GetSettingKey_CallsBaseImplementation()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be($"SiteDB{parameters.CustDBGroupId}_API_SiteBuilder_Licensee_Insert");
    }

    [Fact]
    public void AllMutableProperties_CanBeSetToNull()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();
        var criteria = new CreateCurrencyCriteria(parameters);

        // Act
        criteria.CurrencyList = null;
        criteria.ErrMsg = null;
        criteria.ErrExtraInfo = null;
        criteria.AgentList = null;
        criteria.InsBalExceptCustList = null;
        criteria.FailedCurrencies = null;
        criteria.ListSuperRollBack = null;

        // Assert
        criteria.CurrencyList.Should().BeNull();
        criteria.ErrMsg.Should().BeNull();
        criteria.ErrExtraInfo.Should().BeNull();
        criteria.AgentList.Should().BeNull();
        criteria.InsBalExceptCustList.Should().BeNull();
        criteria.FailedCurrencies.Should().BeNull();
        criteria.ListSuperRollBack.Should().BeNull();
    }

    [Fact]
    public void Constructor_StoresParametersCorrectly()
    {
        // Arrange
        var parameters = CreateValidSiteBuilderParams();

        // Act
        var criteria = new CreateCurrencyCriteria(parameters);

        // Assert
        // Verify all base class properties are correctly accessible
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

    private SiteBuilderParams CreateValidSiteBuilderParams()
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
}