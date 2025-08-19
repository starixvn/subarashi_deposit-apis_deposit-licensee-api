namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees;

using DepositLicenseeAPI.Features.Licensees;
using DepositLicenseeAPI.Features.Shared;

public class LicenseeModelsTests
{
    [Fact]
    public void BetSetting_WithAllProperties_CreatesCorrectly()
    {
        // Arrange
        var productId = 1;
        var sportType = 2;
        var betType = 3;
        var currency = 4;
        var minBet = 10.5m;
        var maxBet = 1000.0m;
        var mpm = 0.1m;
        var mpb = 0.2m;
        var mppm = 0.15m;

        // Act
        var betSetting = new BetSetting(productId, sportType, betType, currency, minBet, maxBet, mpm, mpb, mppm);

        // Assert
        betSetting.ProductID.Should().Be(productId);
        betSetting.Sporttype.Should().Be(sportType);
        betSetting.Bettype.Should().Be(betType);
        betSetting.Currency.Should().Be(currency);
        betSetting.MinBet.Should().Be(minBet);
        betSetting.MaxBet.Should().Be(maxBet);
        betSetting.MPM.Should().Be(mpm);
        betSetting.MPB.Should().Be(mpb);
        betSetting.MPPM.Should().Be(mppm);
    }

    [Fact]
    public void BetSetting_WithoutMPPM_CreatesWithNullMPPM()
    {
        // Arrange & Act
        var betSetting = new BetSetting(1, 2, 3, 4, 10.5m, 1000.0m, 0.1m, 0.2m);

        // Assert
        betSetting.MPPM.Should().BeNull();
    }

    [Fact]
    public void PositionTaking_CreatesCorrectly()
    {
        // Arrange
        var productId = 5;
        var pt = 0.75m;

        // Act
        var positionTaking = new PositionTaking(productId, pt);

        // Assert
        positionTaking.ProductID.Should().Be(productId);
        positionTaking.PT.Should().Be(pt);
    }

    [Fact]
    public void StakeLevel_CreatesCorrectly()
    {
        // Arrange
        var currency = 1;
        var gameTypeId = 2;
        var minStake = 1.0m;
        var maxStake = 100.0m;

        // Act
        var stakeLevel = new StakeLevel(currency, gameTypeId, minStake, maxStake);

        // Assert
        stakeLevel.Currency.Should().Be(currency);
        stakeLevel.GameTypeID.Should().Be(gameTypeId);
        stakeLevel.MinStake.Should().Be(minStake);
        stakeLevel.MaxStake.Should().Be(maxStake);
    }

    [Fact]
    public void Commission_CreatesCorrectly()
    {
        // Arrange
        var productId = 3;
        var comm = 0.05m;

        // Act
        var commission = new Commission(productId, comm);

        // Assert
        commission.ProductID.Should().Be(productId);
        commission.Comm.Should().Be(comm);
    }

    [Fact]
    public void CreateCurrencyRequest_CreatesCorrectly()
    {
        // Arrange
        var siteName = "TestSite";
        var productBetSetting = new[] { new BetSetting(1, 1, 1, 1, 10m, 1000m, 0.1m, 0.2m) };
        var productPT = new[] { new PositionTaking(1, 0.5m) };
        var productStakeLevel = new[] { new StakeLevel(1, 1, 1m, 100m) };
        var productComm = new[] { new Commission(1, 0.05m) };
        var creator = "TestUser";
        var ip = "192.168.1.1";
        var isCreateSub = true;
        var licenseeType = "Standard";

        // Act
        var request = new CreateCurrencyRequest(
            siteName, productBetSetting, productPT, productStakeLevel,
            productComm, creator, ip, isCreateSub, licenseeType);

        // Assert
        request.SiteName.Should().Be(siteName);
        request.ProductBetSetting.Should().BeEquivalentTo(productBetSetting);
        request.ProductPT.Should().BeEquivalentTo(productPT);
        request.ProductStakeLevel.Should().BeEquivalentTo(productStakeLevel);
        request.ProductComm.Should().BeEquivalentTo(productComm);
        request.Creator.Should().Be(creator);
        request.IP.Should().Be(ip);
        request.IsCreateSub.Should().Be(isCreateSub);
        request.LicenseeType.Should().Be(licenseeType);
    }

    [Fact]
    public void CreateLicenseeRequest_CreatesCorrectly()
    {
        // Arrange
        var siteName = "TestSite";
        var productBetSetting = new[] { new BetSetting(1, 1, 1, 1, 10m, 1000m, 0.1m, 0.2m) };
        var productPT = new[] { new PositionTaking(1, 0.5m) };
        var productStakeLevel = new[] { new StakeLevel(1, 1, 1m, 100m) };
        var productComm = new[] { new Commission(1, 0.05m) };
        var creator = "TestUser";
        var ip = "192.168.1.1";
        var isCreateSub = true;
        var licenseeType = "Standard";
        var isChinaView = false;
        byte siteDBGroupId = 1;
        byte specialPlatform = 0;
        byte siteType = 1;

        // Act
        var request = new CreateLicenseeRequest(
            siteName, productBetSetting, productPT, productStakeLevel, productComm,
            creator, ip, isCreateSub, licenseeType, isChinaView, siteDBGroupId,
            specialPlatform, siteType);

        // Assert
        request.SiteName.Should().Be(siteName);
        request.ProductBetSetting.Should().BeEquivalentTo(productBetSetting);
        request.ProductPT.Should().BeEquivalentTo(productPT);
        request.ProductStakeLevel.Should().BeEquivalentTo(productStakeLevel);
        request.ProductComm.Should().BeEquivalentTo(productComm);
        request.Creator.Should().Be(creator);
        request.IP.Should().Be(ip);
        request.IsCreateSub.Should().Be(isCreateSub);
        request.LicenseeType.Should().Be(licenseeType);
        request.IsChinaView.Should().Be(isChinaView);
        request.SiteDBGroupId.Should().Be(siteDBGroupId);
        request.SpecialPlatform.Should().Be(specialPlatform);
        request.SiteType.Should().Be(siteType);
    }

    [Fact]
    public void CreateLicenseeRequest_InheritsFromCreateCurrencyRequest()
    {
        // Arrange
        var request = new CreateLicenseeRequest(
            "TestSite",
            new[] { new BetSetting(1, 1, 1, 1, 10m, 1000m, 0.1m, 0.2m) },
            new[] { new PositionTaking(1, 0.5m) },
            new[] { new StakeLevel(1, 1, 1m, 100m) },
            new[] { new Commission(1, 0.05m) },
            "TestUser", "192.168.1.1", true, "Standard", false, 1, 0, 1);

        // Act & Assert
        request.Should().BeAssignableTo<CreateCurrencyRequest>();
    }

    [Fact]
    public void ValidateCreateLicenseeResult_CreatesCorrectly()
    {
        // Arrange
        var errorCode = 200;
        var errorMessage = "Success";
        var registerUsernames = "user1,user2";

        // Act
        var result = new ValidateCreateLicenseeResult(errorCode, errorMessage, registerUsernames);

        // Assert
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
        result.RegisterUsernames.Should().Be(registerUsernames);
        result.Should().BeAssignableTo<Result>();
    }

    [Fact]
    public void RegisterCustomersResult_CreatesCorrectly()
    {
        // Arrange
        var errorCode = 0;
        var errorMessage = "Success";
        var siteId = 123;
        var customers = "customer1,customer2";

        // Act
        var result = new RegisterCustomersResult(errorCode, errorMessage, siteId, customers);

        // Assert
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
        result.SiteId.Should().Be(siteId);
        result.Customers.Should().Be(customers);
        result.Should().BeAssignableTo<Result>();
    }

    [Fact]
    public void CreateLicenseeResult_CreatesCorrectly()
    {
        // Arrange
        var errorCode = 0;
        var errorMessage = "";
        var failedCurrencies = new[] { "USD", "EUR" };

        // Act
        var result = new CreateLicenseeResult(errorCode, errorMessage, failedCurrencies);

        // Assert
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
        result.FailedCurrencies.Should().BeEquivalentTo(failedCurrencies);
        result.Should().BeAssignableTo<Result>();
    }

    [Fact]
    public void RegisterDummyMemberResult_CreatesCorrectly()
    {
        // Arrange
        var errorCode = 0;
        var errorMessage = "Success";
        var custId = 456;
        var username = "testuser";

        // Act
        var result = new RegisterDummyMemberResult(errorCode, errorMessage, custId, username);

        // Assert
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
        result.CustId.Should().Be(custId);
        result.Username.Should().Be(username);
        result.Should().BeAssignableTo<Result>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("ABCDEFGHIJ")]
    public void CreateLicenseeRequest_SiteName_AcceptsValidLengths(string siteName)
    {
        // Arrange & Act
        var request = new CreateLicenseeRequest(
            siteName,
            new[] { new BetSetting(1, 1, 1, 1, 10m, 1000m, 0.1m, 0.2m) },
            new[] { new PositionTaking(1, 0.5m) },
            null,
            null,
            "Creator", "192.168.1.1", true, "Standard", false, 1, 0, 1);

        // Assert
        request.SiteName.Should().Be(siteName);
    }

    [Fact]
    public void CreateLicenseeRequest_SiteName_PropertyExists()
    {
        // Arrange
        var propertyInfo = typeof(CreateLicenseeRequest).GetProperty("SiteName");

        // Act & Assert
        propertyInfo.Should().NotBeNull();
        propertyInfo.PropertyType.Should().Be(typeof(string));
    }

    [Fact]
    public void CreateLicenseeRequest_Properties_AreNotNull()
    {
        // Arrange
        var type = typeof(CreateLicenseeRequest);
        var properties = new[]
        {
            "SiteName", "ProductBetSetting", "ProductPT", "Creator", "IP",
            "IsCreateSub", "LicenseeType", "IsChinaView", "SiteDBGroupId",
            "SpecialPlatform", "SiteType"
        };

        // Act & Assert
        foreach (var propertyName in properties)
        {
            var propertyInfo = type.GetProperty(propertyName);
            propertyInfo.Should().NotBeNull($"{propertyName} property should exist");
        }
    }
}