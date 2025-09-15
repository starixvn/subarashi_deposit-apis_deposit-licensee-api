namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees.Criteria;

public class SiteBuilderValidateCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var custDBGroupId = 123;
        var productBetSetting = "betSetting";
        var productPT = "pt";
        var siteName = "TestSite";
        var mode = (byte)1;
        var settingStatus = (byte)2;
        var isCreateSub = true;

        // Act
        var command = new SiteBuilderValidateCommand(custDBGroupId, productBetSetting, productPT, siteName, mode, settingStatus, isCreateSub);

        // Assert
        command.Should().NotBeNull();
        command.CustDBGroupId.Should().Be(custDBGroupId);
        command.ProductBetSetting.Should().Be(productBetSetting);
        command.ProductPT.Should().Be(productPT);
        command.SiteName.Should().Be(siteName);
        command.Mode.Should().Be(mode);
        command.SettingStatus.Should().Be(settingStatus);
        command.IsCreateSub.Should().Be(isCreateSub);
    }

    [Fact]
    public void CustDBGroupId_IsReadOnly()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        var property = typeof(SiteBuilderValidateCommand).GetProperty(nameof(SiteBuilderValidateCommand.CustDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void ProductBetSetting_IsReadOnly()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        var property = typeof(SiteBuilderValidateCommand).GetProperty(nameof(SiteBuilderValidateCommand.ProductBetSetting));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void ProductPT_IsReadOnly()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        var property = typeof(SiteBuilderValidateCommand).GetProperty(nameof(SiteBuilderValidateCommand.ProductPT));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void SiteName_IsReadOnly()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        var property = typeof(SiteBuilderValidateCommand).GetProperty(nameof(SiteBuilderValidateCommand.SiteName));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void Mode_IsReadOnly()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        var property = typeof(SiteBuilderValidateCommand).GetProperty(nameof(SiteBuilderValidateCommand.Mode));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void SettingStatus_IsReadOnly()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        var property = typeof(SiteBuilderValidateCommand).GetProperty(nameof(SiteBuilderValidateCommand.SettingStatus));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void IsCreateSub_IsReadOnly()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        var property = typeof(SiteBuilderValidateCommand).GetProperty(nameof(SiteBuilderValidateCommand.IsCreateSub));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void MutableProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        command.ErrCode = 500;
        command.ErrMsg = "Test error message";
        command.RegisterUsernames = "user1,user2";

        // Assert
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Test error message");
        command.RegisterUsernames.Should().Be("user1,user2");
    }

    [Fact]
    public void MutableProperties_DefaultValues()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        command.ErrCode.Should().Be(0);
        command.ErrMsg.Should().BeNull();
        command.RegisterUsernames.Should().BeNull();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var custDBGroupId = 123;
        var command = new SiteBuilderValidateCommand(custDBGroupId, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be($"SiteDB{custDBGroupId}_API_SiteBuilder_Validate");
        result.Should().Be("SiteDB123_API_SiteBuilder_Validate");
    }

    [Fact]
    public void GetSettingKey_WithZeroCustDBGroupId_ReturnsBaseName()
    {
        // Arrange
        var custDBGroupId = 0;
        var command = new SiteBuilderValidateCommand(custDBGroupId, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("API_SiteBuilder_Validate");
    }

    [Fact]
    public void IsValid_WithAllValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullProductPT_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", null, "TestSite", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyProductPT_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "", "TestSite", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceProductPT_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "   ", "TestSite", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullProductBetSetting_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, null, "pt", "TestSite", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyProductBetSetting_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "", "pt", "TestSite", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceProductBetSetting_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "   ", "pt", "TestSite", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", null, 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptySiteName_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceSiteName_ReturnsFalse()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "   ", 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("betSetting", "pt", "siteName", true)]
    [InlineData("a", "b", "c", true)]
    [InlineData("valid setting", "valid pt", "valid site", true)]
    public void IsValid_WithValidCombinations_ReturnsTrue(string productBetSetting, string productPT, string siteName, bool expected)
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, productBetSetting, productPT, siteName, 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "pt", "siteName", false)]
    [InlineData("", "pt", "siteName", false)]
    [InlineData("   ", "pt", "siteName", false)]
    [InlineData("betSetting", null, "siteName", false)]
    [InlineData("betSetting", "", "siteName", false)]
    [InlineData("betSetting", "   ", "siteName", false)]
    [InlineData("betSetting", "pt", null, false)]
    [InlineData("betSetting", "pt", "", false)]
    [InlineData("betSetting", "pt", "   ", false)]
    [InlineData(null, null, null, false)]
    [InlineData("", "", "", false)]
    public void IsValid_WithInvalidCombinations_ReturnsFalse(string productBetSetting, string productPT, string siteName, bool expected)
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, productBetSetting, productPT, siteName, 1, 2, true);

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var custDBGroupId1 = 111;
        var productBetSetting1 = "betSetting1";
        var productPT1 = "pt1";
        var siteName1 = "Site1";
        var mode1 = (byte)1;
        var settingStatus1 = (byte)2;
        var isCreateSub1 = true;

        var custDBGroupId2 = 222;
        var productBetSetting2 = "betSetting2";
        var productPT2 = "pt2";
        var siteName2 = "Site2";
        var mode2 = (byte)3;
        var settingStatus2 = (byte)4;
        var isCreateSub2 = false;

        // Act
        var command1 = new SiteBuilderValidateCommand(custDBGroupId1, productBetSetting1, productPT1, siteName1, mode1, settingStatus1, isCreateSub1);
        var command2 = new SiteBuilderValidateCommand(custDBGroupId2, productBetSetting2, productPT2, siteName2, mode2, settingStatus2, isCreateSub2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.CustDBGroupId.Should().Be(custDBGroupId1);
        command1.ProductBetSetting.Should().Be(productBetSetting1);
        command1.ProductPT.Should().Be(productPT1);
        command1.SiteName.Should().Be(siteName1);
        command1.Mode.Should().Be(mode1);
        command1.SettingStatus.Should().Be(settingStatus1);
        command1.IsCreateSub.Should().Be(isCreateSub1);
        command2.CustDBGroupId.Should().Be(custDBGroupId2);
        command2.ProductBetSetting.Should().Be(productBetSetting2);
        command2.ProductPT.Should().Be(productPT2);
        command2.SiteName.Should().Be(siteName2);
        command2.Mode.Should().Be(mode2);
        command2.SettingStatus.Should().Be(settingStatus2);
        command2.IsCreateSub.Should().Be(isCreateSub2);
    }

    [Theory]
    [InlineData(1, "betSetting1", "pt1", "Site1", (byte)1, (byte)2, true)]
    [InlineData(100, "betSetting100", "pt100", "Site100", (byte)255, (byte)0, false)]
    [InlineData(0, "a", "b", "c", (byte)0, (byte)255, true)]
    [InlineData(-1, "", "", "", (byte)128, (byte)128, false)]
    public void Constructor_WithVariousParameters_InitializesCorrectly(
        int custDBGroupId,
        string productBetSetting,
        string productPT,
        string siteName,
        byte mode,
        byte settingStatus,
        bool isCreateSub)
    {
        // Arrange & Act
        var command = new SiteBuilderValidateCommand(custDBGroupId, productBetSetting, productPT, siteName, mode, settingStatus, isCreateSub);

        // Assert
        command.CustDBGroupId.Should().Be(custDBGroupId);
        command.ProductBetSetting.Should().Be(productBetSetting);
        command.ProductPT.Should().Be(productPT);
        command.SiteName.Should().Be(siteName);
        command.Mode.Should().Be(mode);
        command.SettingStatus.Should().Be(settingStatus);
        command.IsCreateSub.Should().Be(isCreateSub);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(-50)]
    public void GetSettingKey_UsesSiteDBHelpers(int custDBGroupId)
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(custDBGroupId, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        var result = command.GetSettingKey();

        // Assert
        if (custDBGroupId == 0)
        {
            result.Should().Be("API_SiteBuilder_Validate");
        }
        else
        {
            result.Should().Be($"SiteDB{custDBGroupId}_API_SiteBuilder_Validate");
        }
    }

    [Fact]
    public void ValidationLogic_RequiresAllThreeStringFields()
    {
        // Arrange & Act & Assert
        new SiteBuilderValidateCommand(123, "valid", "valid", "valid", 1, 2, true).IsValid().Should().BeTrue();

        new SiteBuilderValidateCommand(123, null, "valid", "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "valid", null, "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "valid", "valid", null, 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "", "valid", "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "valid", "", "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "valid", "valid", "", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "   ", "valid", "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "valid", "   ", "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(123, "valid", "valid", "   ", 1, 2, true).IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesValidationCondition()
    {
        // Arrange
        var validCommand = new SiteBuilderValidateCommand(123, "validBet", "validPT", "validSite", 1, 2, true);
        var invalidBetCommand = new SiteBuilderValidateCommand(123, null, "validPT", "validSite", 1, 2, true);
        var invalidPTCommand = new SiteBuilderValidateCommand(123, "validBet", "", "validSite", 1, 2, true);
        var invalidSiteCommand = new SiteBuilderValidateCommand(123, "validBet", "validPT", "   ", 1, 2, true);

        // Act & Assert
        validCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(validCommand.ProductPT) &&
            !string.IsNullOrWhiteSpace(validCommand.ProductBetSetting) &&
            !string.IsNullOrWhiteSpace(validCommand.SiteName));

        invalidBetCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidBetCommand.ProductPT) &&
            !string.IsNullOrWhiteSpace(invalidBetCommand.ProductBetSetting) &&
            !string.IsNullOrWhiteSpace(invalidBetCommand.SiteName));

        invalidPTCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidPTCommand.ProductPT) &&
            !string.IsNullOrWhiteSpace(invalidPTCommand.ProductBetSetting) &&
            !string.IsNullOrWhiteSpace(invalidPTCommand.SiteName));

        invalidSiteCommand.IsValid().Should().Be(
            !string.IsNullOrWhiteSpace(invalidSiteCommand.ProductPT) &&
            !string.IsNullOrWhiteSpace(invalidSiteCommand.ProductBetSetting) &&
            !string.IsNullOrWhiteSpace(invalidSiteCommand.SiteName));
    }

    [Fact]
    public void GetSettingKey_IsConsistentForSameInstance()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        var result1 = command.GetSettingKey();
        var result2 = command.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("SiteDB123_API_SiteBuilder_Validate");
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);
        var initialCustDBGroupId = command.CustDBGroupId;
        var initialProductBetSetting = command.ProductBetSetting;
        var initialProductPT = command.ProductPT;
        var initialSiteName = command.SiteName;
        var initialMode = command.Mode;
        var initialSettingStatus = command.SettingStatus;
        var initialIsCreateSub = command.IsCreateSub;
        var initialSettingKey = command.GetSettingKey();
        var initialValid = command.IsValid();

        // Act - Call methods multiple times
        var custDBGroupId1 = command.CustDBGroupId;
        var productBetSetting1 = command.ProductBetSetting;
        var productPT1 = command.ProductPT;
        var siteName1 = command.SiteName;
        var mode1 = command.Mode;
        var settingStatus1 = command.SettingStatus;
        var isCreateSub1 = command.IsCreateSub;
        var settingKey1 = command.GetSettingKey();
        var valid1 = command.IsValid();

        // Assert - All calls should return same values
        custDBGroupId1.Should().Be(initialCustDBGroupId);
        productBetSetting1.Should().Be(initialProductBetSetting);
        productPT1.Should().Be(initialProductPT);
        siteName1.Should().Be(initialSiteName);
        mode1.Should().Be(initialMode);
        settingStatus1.Should().Be(initialSettingStatus);
        isCreateSub1.Should().Be(initialIsCreateSub);
        settingKey1.Should().Be(initialSettingKey);
        valid1.Should().Be(initialValid);
    }

    [Fact]
    public void AllMutableProperties_CanBeSetToNull()
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        command.ErrMsg = null;
        command.RegisterUsernames = null;

        // Assert
        command.ErrMsg.Should().BeNull();
        command.RegisterUsernames.Should().BeNull();
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(500)]
    [InlineData(int.MaxValue)]
    public void ErrCode_AcceptsVariousValues(int errCode)
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        command.ErrCode = errCode;

        // Assert
        command.ErrCode.Should().Be(errCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Error message")]
    [InlineData("   ")]
    [InlineData("Very long error message with special characters @#$%^&*()")]
    public void ErrMsg_AcceptsVariousValues(string errMsg)
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        command.ErrMsg = errMsg;

        // Assert
        command.ErrMsg.Should().Be(errMsg);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("user1,user2")]
    [InlineData("   ")]
    [InlineData("Very long usernames list with special characters @#$%^&*()")]
    public void RegisterUsernames_AcceptsVariousValues(string registerUsernames)
    {
        // Arrange
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, true);

        // Act
        command.RegisterUsernames = registerUsernames;

        // Assert
        command.RegisterUsernames.Should().Be(registerUsernames);
    }

    [Fact]
    public void ValidationDoesNotDependOnNumericOrBooleanParameters()
    {
        // Arrange & Act & Assert
        // Validation should only depend on the three string fields, not on numeric or boolean parameters
        new SiteBuilderValidateCommand(-999, "valid", "valid", "valid", 255, 255, false).IsValid().Should().BeTrue();
        new SiteBuilderValidateCommand(int.MaxValue, "valid", "valid", "valid", 0, 0, true).IsValid().Should().BeTrue();
        new SiteBuilderValidateCommand(0, "valid", "valid", "valid", 128, 64, false).IsValid().Should().BeTrue();

        new SiteBuilderValidateCommand(999, null, "valid", "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(999, "valid", "", "valid", 1, 2, true).IsValid().Should().BeFalse();
        new SiteBuilderValidateCommand(999, "valid", "valid", "   ", 1, 2, true).IsValid().Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsCreateSub_ReflectsParameterValue(bool isCreateSub)
    {
        // Arrange & Act
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, 2, isCreateSub);

        // Assert
        command.IsCreateSub.Should().Be(isCreateSub);
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)128)]
    [InlineData((byte)255)]
    public void Mode_ReflectsParameterValue(byte mode)
    {
        // Arrange & Act
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", mode, 2, true);

        // Assert
        command.Mode.Should().Be(mode);
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)128)]
    [InlineData((byte)255)]
    public void SettingStatus_ReflectsParameterValue(byte settingStatus)
    {
        // Arrange & Act
        var command = new SiteBuilderValidateCommand(123, "betSetting", "pt", "TestSite", 1, settingStatus, true);

        // Assert
        command.SettingStatus.Should().Be(settingStatus);
    }
}