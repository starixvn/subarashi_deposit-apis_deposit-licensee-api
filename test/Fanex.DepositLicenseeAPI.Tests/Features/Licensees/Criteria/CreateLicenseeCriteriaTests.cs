namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using DepositLicenseeAPI.Features.Licensees;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class CreateLicenseeCriteriaTests
{
    [Fact]
    public void Constructor_WithValidParams_InitializesCorrectly()
    {
        // Arrange
        var parameters = CreateValidParams();

        // Act
        var criteria = new CreateLicenseeCriteria(parameters);

        // Assert
        criteria.Should().NotBeNull();
        criteria.IsChinaView.Should().Be(parameters.IsChinaView);
        criteria.SpecialPlatform.Should().Be(parameters.SpecialPlatform);
        criteria.SiteType.Should().Be(parameters.SiteType);
        criteria.Mode.Should().Be(CreateMode.NewLicensee);
    }

    [Fact]
    public void IsChinaView_ReturnsCorrectValue()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

        // Act
        var result = criteria.IsChinaView;

        // Assert
        result.Should().Be(parameters.IsChinaView);
    }

    [Fact]
    public void SpecialPlatform_ReturnsCorrectValue()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

        // Act
        var result = criteria.SpecialPlatform;

        // Assert
        result.Should().Be(parameters.SpecialPlatform);
    }

    [Fact]
    public void SiteType_ReturnsCorrectValue()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

        // Act
        var result = criteria.SiteType;

        // Assert
        result.Should().Be(parameters.SiteType);
    }

    [Fact]
    public void Mode_AlwaysReturnsNewLicensee()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

        // Act
        var result = criteria.Mode;

        // Assert
        result.Should().Be(CreateMode.NewLicensee);
        result.Should().Be(1);
    }

    [Fact]
    public void ProductList_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);
        var expectedProductList = "product1,product2,product3";

        // Act
        criteria.ProductList = expectedProductList;

        // Assert
        criteria.ProductList.Should().Be(expectedProductList);
    }

    [Fact]
    public void ProductList_DefaultValueIsNull()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

        // Act & Assert
        criteria.ProductList.Should().BeNull();
    }

    [Theory]
    [InlineData(true, (byte)1, (byte)2)]
    [InlineData(false, (byte)0, (byte)1)]
    [InlineData(true, (byte)255, (byte)255)]
    [InlineData(false, (byte)128, (byte)64)]
    public void Constructor_WithVariousParams_InitializesCorrectly(bool isChinaView, byte specialPlatform, byte siteType)
    {
        // Arrange
        var baseParams = new SiteBuilderParams(
            CustDBGroupId: 1,
            Customers: "customer1",
            ProductBetSetting: "setting1",
            ProductPT: "pt1",
            ProductStakeLevel: "level1",
            ProductComm: "comm1",
            Creator: "testuser",
            IP: "192.168.1.1",
            IsCreateSub: false,
            LicenseeType: "type1",
            SiteId: 123,
            SiteName: "TestSite",
            SettingStatus: 1
        );

        var parameters = new CreateLicenseeParams(
            BaseParams: baseParams,
            IsChinaView: isChinaView,
            SpecialPlatform: specialPlatform,
            SiteType: siteType
        );

        // Act
        var criteria = new CreateLicenseeCriteria(parameters);

        // Assert
        criteria.IsChinaView.Should().Be(isChinaView);
        criteria.SpecialPlatform.Should().Be(specialPlatform);
        criteria.SiteType.Should().Be(siteType);
    }

    [Fact]
    public void InheritsFromSiteBuilderBaseCriteria()
    {
        // Arrange
        var parameters = CreateValidParams();

        // Act
        var criteria = new CreateLicenseeCriteria(parameters);

        // Assert
        criteria.Should().BeAssignableTo<SiteBuilderBaseCriteria>();
        criteria.CustDBGroupId.Should().Be(parameters.BaseParams.CustDBGroupId);
        criteria.Customers.Should().Be(parameters.BaseParams.Customers);
        criteria.Creator.Should().Be(parameters.BaseParams.Creator);
        criteria.IP.Should().Be(parameters.BaseParams.IP);
        criteria.SiteId.Should().Be(parameters.BaseParams.SiteId);
        criteria.SiteName.Should().Be(parameters.BaseParams.SiteName);
    }

    [Fact]
    public void BasePropertiesAreAccessible()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

        // Act & Assert
        criteria.ProductBetSetting.Should().Be(parameters.BaseParams.ProductBetSetting);
        criteria.ProductPT.Should().Be(parameters.BaseParams.ProductPT);
        criteria.ProductStakeLevel.Should().Be(parameters.BaseParams.ProductStakeLevel);
        criteria.ProductComm.Should().Be(parameters.BaseParams.ProductComm);
        criteria.IsCreateSub.Should().Be(parameters.BaseParams.IsCreateSub);
        criteria.LicenseeType.Should().Be(parameters.BaseParams.LicenseeType);
        criteria.SettingStatus.Should().Be(parameters.BaseParams.SettingStatus);
    }

    [Fact]
    public void ErrorProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

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
        var parameters = CreateValidParams();
        var criteria = new CreateLicenseeCriteria(parameters);

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

    private CreateLicenseeParams CreateValidParams()
    {
        var baseParams = new SiteBuilderParams(
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

        return new CreateLicenseeParams(
            BaseParams: baseParams,
            IsChinaView: true,
            SpecialPlatform: 2,
            SiteType: 3
        );
    }
}