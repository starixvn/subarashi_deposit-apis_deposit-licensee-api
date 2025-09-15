namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class RegisterCustomersCommandTests
{
    [Fact]
    public void Constructor_WithValidParams_InitializesCorrectly()
    {
        // Arrange
        var parameters = CreateValidParams();

        // Act
        var command = new RegisterCustomersCommand(parameters);

        // Assert
        command.Should().NotBeNull();
        command.RegisterUsernames.Should().Be(parameters.RegisterUsernames);
        command.SiteName.Should().Be(parameters.SiteName);
        command.IsChinaView.Should().Be(parameters.IsChinaView);
        command.SpecialPlatform.Should().Be(parameters.SpecialPlatform);
        command.SiteType.Should().Be(parameters.SiteType);
        command.ProductStakeLevel.Should().Be(parameters.ProductStakeLevel);
        command.IP.Should().Be(parameters.IP);
        command.Mode.Should().Be(parameters.Mode);
        command.SiteDBGroupId.Should().Be(parameters.SiteDBGroupId);
    }

    [Fact]
    public void RegisterUsernames_IsReadOnly()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act & Assert
        var property = typeof(RegisterCustomersCommand).GetProperty(nameof(RegisterCustomersCommand.RegisterUsernames));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void SiteName_IsReadOnly()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act & Assert
        var property = typeof(RegisterCustomersCommand).GetProperty(nameof(RegisterCustomersCommand.SiteName));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_SiteBuilder_Customers_Register");
    }

    [Fact]
    public void IsValid_WithValidRegisterUsernames_ReturnsTrue()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullRegisterUsernames_ReturnsFalse()
    {
        // Arrange
        var parameters = new RegisterCustomersParams(
            RegisterUsernames: null,
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 2,
            SiteType: 3,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        );
        var command = new RegisterCustomersCommand(parameters);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyRegisterUsernames_ReturnsFalse()
    {
        // Arrange
        var parameters = new RegisterCustomersParams(
            RegisterUsernames: "",
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 2,
            SiteType: 3,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        );
        var command = new RegisterCustomersCommand(parameters);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceRegisterUsernames_ReturnsFalse()
    {
        // Arrange
        var parameters = new RegisterCustomersParams(
            RegisterUsernames: "   ",
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 2,
            SiteType: 3,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        );
        var command = new RegisterCustomersCommand(parameters);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("user1", true)]
    [InlineData("user1,user2", true)]
    [InlineData("user1,user2,user3", true)]
    [InlineData("a", true)]
    [InlineData("user with spaces", true)]
    public void IsValid_WithVariousValidRegisterUsernames_ReturnsTrue(string registerUsernames, bool expected)
    {
        // Arrange
        var parameters = new RegisterCustomersParams(
            RegisterUsernames: registerUsernames,
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 2,
            SiteType: 3,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        );
        var command = new RegisterCustomersCommand(parameters);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("\t", false)]
    [InlineData("\n", false)]
    public void IsValid_WithInvalidRegisterUsernames_ReturnsFalse(string registerUsernames, bool expected)
    {
        // Arrange
        var parameters = new RegisterCustomersParams(
            RegisterUsernames: registerUsernames,
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 2,
            SiteType: 3,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        );
        var command = new RegisterCustomersCommand(parameters);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void MutableProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act
        command.ErrCode = 500;
        command.ErrMsg = "Test error message";
        command.SiteId = 456;
        command.Customers = "customer1,customer2";
        command.FailedCurrencies = "USD,EUR";
        command.ErrExtraInfo = "Extra error info";

        // Assert
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Test error message");
        command.SiteId.Should().Be(456);
        command.Customers.Should().Be("customer1,customer2");
        command.FailedCurrencies.Should().Be("USD,EUR");
        command.ErrExtraInfo.Should().Be("Extra error info");
    }

    [Fact]
    public void MutableProperties_DefaultValues()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act & Assert
        command.ErrCode.Should().Be(0);
        command.ErrMsg.Should().BeNull();
        command.SiteId.Should().Be(0);
        command.Customers.Should().BeNull();
        command.FailedCurrencies.Should().BeNull();
        command.ErrExtraInfo.Should().BeNull();
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void GetSettingKey_ReturnsSameValueForDifferentInstances()
    {
        // Arrange
        var parameters1 = CreateValidParams();
        var parameters2 = new RegisterCustomersParams(
            RegisterUsernames: "user4,user5",
            SiteName: "TestSite2",
            IsChinaView: false,
            SpecialPlatform: 1,
            SiteType: 2,
            ProductStakeLevel: "level2",
            IP: "192.168.1.2",
            Mode: 2,
            SiteDBGroupId: 456
        );
        var command1 = new RegisterCustomersCommand(parameters1);
        var command2 = new RegisterCustomersCommand(parameters2);

        // Act
        var result1 = command1.GetSettingKey();
        var result2 = command2.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("API_SiteBuilder_Customers_Register");
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var parameters1 = CreateValidParams();
        var parameters2 = new RegisterCustomersParams(
            RegisterUsernames: "user4,user5",
            SiteName: "TestSite2",
            IsChinaView: false,
            SpecialPlatform: 1,
            SiteType: 2,
            ProductStakeLevel: "level2",
            IP: "192.168.1.2",
            Mode: 2,
            SiteDBGroupId: 456
        );

        // Act
        var command1 = new RegisterCustomersCommand(parameters1);
        var command2 = new RegisterCustomersCommand(parameters2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.RegisterUsernames.Should().Be(parameters1.RegisterUsernames);
        command1.SiteName.Should().Be(parameters1.SiteName);
        command2.RegisterUsernames.Should().Be(parameters2.RegisterUsernames);
        command2.SiteName.Should().Be(parameters2.SiteName);
    }

    [Theory]
    [InlineData(true, (byte)1, (byte)2, (byte)3, 123)]
    [InlineData(false, (byte)0, (byte)1, (byte)1, 456)]
    [InlineData(true, (byte)255, (byte)255, (byte)0, 999)]
    public void Constructor_WithVariousParams_InitializesCorrectly(bool isChinaView, byte specialPlatform, byte siteType, byte mode, int siteDBGroupId)
    {
        // Arrange
        var parameters = new RegisterCustomersParams(
            RegisterUsernames: "testuser",
            SiteName: "TestSite",
            IsChinaView: isChinaView,
            SpecialPlatform: specialPlatform,
            SiteType: siteType,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: mode,
            SiteDBGroupId: siteDBGroupId
        );

        // Act
        var command = new RegisterCustomersCommand(parameters);

        // Assert
        command.IsChinaView.Should().Be(isChinaView);
        command.SpecialPlatform.Should().Be(specialPlatform);
        command.SiteType.Should().Be(siteType);
        command.Mode.Should().Be(mode);
        command.SiteDBGroupId.Should().Be(siteDBGroupId);
    }

    [Fact]
    public void ValidationLogic_OnlyDependsOnRegisterUsernames()
    {
        // Arrange
        var validParams = CreateValidParams();
        var invalidParams = new RegisterCustomersParams(
            RegisterUsernames: null,
            SiteName: validParams.SiteName,
            IsChinaView: validParams.IsChinaView,
            SpecialPlatform: validParams.SpecialPlatform,
            SiteType: validParams.SiteType,
            ProductStakeLevel: validParams.ProductStakeLevel,
            IP: validParams.IP,
            Mode: validParams.Mode,
            SiteDBGroupId: validParams.SiteDBGroupId
        );

        var validCommand = new RegisterCustomersCommand(validParams);
        var invalidCommand = new RegisterCustomersCommand(invalidParams);

        // Act & Assert
        validCommand.IsValid().Should().BeTrue();
        invalidCommand.IsValid().Should().BeFalse();

        // Validation should be deterministic
        validCommand.IsValid().Should().Be(validCommand.IsValid());
        invalidCommand.IsValid().Should().Be(invalidCommand.IsValid());
    }

    [Fact]
    public void IsValid_MatchesConditionNotNullOrWhiteSpace()
    {
        // Arrange
        var validCommand = new RegisterCustomersCommand(new RegisterCustomersParams(
            RegisterUsernames: "validuser",
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 1,
            SiteType: 1,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        ));

        var nullCommand = new RegisterCustomersCommand(new RegisterCustomersParams(
            RegisterUsernames: null,
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 1,
            SiteType: 1,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        ));

        var emptyCommand = new RegisterCustomersCommand(new RegisterCustomersParams(
            RegisterUsernames: "",
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 1,
            SiteType: 1,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        ));

        var whitespaceCommand = new RegisterCustomersCommand(new RegisterCustomersParams(
            RegisterUsernames: "   ",
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 1,
            SiteType: 1,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        ));

        // Act & Assert
        validCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(validCommand.RegisterUsernames));
        nullCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(nullCommand.RegisterUsernames));
        emptyCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(emptyCommand.RegisterUsernames));
        whitespaceCommand.IsValid().Should().Be(!string.IsNullOrWhiteSpace(whitespaceCommand.RegisterUsernames));
    }

    [Fact]
    public void AllReadOnlyProperties_AreConsistent()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act - Call properties multiple times
        var registerUsernames1 = command.RegisterUsernames;
        var registerUsernames2 = command.RegisterUsernames;
        var siteName1 = command.SiteName;
        var siteName2 = command.SiteName;
        var isChinaView1 = command.IsChinaView;
        var isChinaView2 = command.IsChinaView;

        // Assert - All calls should return same values
        registerUsernames1.Should().Be(registerUsernames2);
        siteName1.Should().Be(siteName2);
        isChinaView1.Should().Be(isChinaView2);

        registerUsernames1.Should().Be(parameters.RegisterUsernames);
        siteName1.Should().Be(parameters.SiteName);
        isChinaView1.Should().Be(parameters.IsChinaView);
    }

    [Fact]
    public void Command_BehaviorIsConsistentAcrossInstances()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command1 = new RegisterCustomersCommand(parameters);
        var command2 = new RegisterCustomersCommand(parameters);

        // Act & Assert
        command1.RegisterUsernames.Should().Be(command2.RegisterUsernames);
        command1.SiteName.Should().Be(command2.SiteName);
        command1.GetSettingKey().Should().Be(command2.GetSettingKey());
        command1.IsValid().Should().Be(command2.IsValid());

        // They should behave identically but be different instances
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void AllPropertiesFromParams_AreAccessible()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act & Assert
        command.RegisterUsernames.Should().Be(parameters.RegisterUsernames);
        command.SiteName.Should().Be(parameters.SiteName);
        command.IsChinaView.Should().Be(parameters.IsChinaView);
        command.SpecialPlatform.Should().Be(parameters.SpecialPlatform);
        command.SiteType.Should().Be(parameters.SiteType);
        command.ProductStakeLevel.Should().Be(parameters.ProductStakeLevel);
        command.IP.Should().Be(parameters.IP);
        command.Mode.Should().Be(parameters.Mode);
        command.SiteDBGroupId.Should().Be(parameters.SiteDBGroupId);
    }

    [Fact]
    public void MutableProperties_CanBeModifiedIndependently()
    {
        // Arrange
        var parameters = CreateValidParams();
        var command = new RegisterCustomersCommand(parameters);

        // Act
        command.ErrCode = 500;
        command.ErrMsg = "Error occurred";
        command.SiteId = 999;
        command.Customers = "newcustomer1,newcustomer2";

        // Assert
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Error occurred");
        command.SiteId.Should().Be(999);
        command.Customers.Should().Be("newcustomer1,newcustomer2");

        // Read-only properties should remain unchanged
        command.RegisterUsernames.Should().Be(parameters.RegisterUsernames);
        command.SiteName.Should().Be(parameters.SiteName);
        command.IsValid().Should().BeTrue();
    }

    private RegisterCustomersParams CreateValidParams()
    {
        return new RegisterCustomersParams(
            RegisterUsernames: "user1,user2,user3",
            SiteName: "TestSite",
            IsChinaView: true,
            SpecialPlatform: 2,
            SiteType: 3,
            ProductStakeLevel: "level1",
            IP: "192.168.1.1",
            Mode: 1,
            SiteDBGroupId: 123
        );
    }
}