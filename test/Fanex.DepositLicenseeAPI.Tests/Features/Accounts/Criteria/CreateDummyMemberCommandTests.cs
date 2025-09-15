namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts.Criteria;

using Data.Repository;
using DepositLicenseeAPI.Features.Accounts.Criteria;

public class CreateDummyMemberCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var custDBGroupId = 123;
        var custId = 456;
        var username = "testuser";
        var creator = 789;
        var recommend = 999;
        var ip = "192.168.1.1";

        // Act
        var command = new CreateDummyMemberCommand(custDBGroupId, custId, username, creator, recommend, ip);

        // Assert
        command.Should().NotBeNull();
        command.CustDBGroupId.Should().Be(custDBGroupId);
        command.CustId.Should().Be(custId);
        command.Username.Should().Be(username);
        command.Creator.Should().Be(creator);
        command.Recommend.Should().Be(recommend);
        command.IP.Should().Be(ip);
    }

    [Fact]
    public void CustDBGroupId_IsReadOnly()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act & Assert
        var property = typeof(CreateDummyMemberCommand).GetProperty(nameof(CreateDummyMemberCommand.CustDBGroupId));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void Username_IsReadOnly()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act & Assert
        var property = typeof(CreateDummyMemberCommand).GetProperty(nameof(CreateDummyMemberCommand.Username));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void Creator_IsReadOnly()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act & Assert
        var property = typeof(CreateDummyMemberCommand).GetProperty(nameof(CreateDummyMemberCommand.Creator));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void Recommend_IsReadOnly()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act & Assert
        var property = typeof(CreateDummyMemberCommand).GetProperty(nameof(CreateDummyMemberCommand.Recommend));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void IP_IsReadOnly()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act & Assert
        var property = typeof(CreateDummyMemberCommand).GetProperty(nameof(CreateDummyMemberCommand.IP));
        property.Should().NotBeNull();
        property.CanWrite.Should().BeFalse();
        property.CanRead.Should().BeTrue();
    }

    [Fact]
    public void CustId_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");
        var newCustId = 111;

        // Act
        command.CustId = newCustId;

        // Assert
        command.CustId.Should().Be(newCustId);
    }

    [Fact]
    public void MutableProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        command.CustId = 555;
        command.ErrCode = 500;
        command.ErrMsg = "Test error message";
        command.ErrExtraInfo = "Extra error info";

        // Assert
        command.CustId.Should().Be(555);
        command.ErrCode.Should().Be(500);
        command.ErrMsg.Should().Be("Test error message");
        command.ErrExtraInfo.Should().Be("Extra error info");
    }

    [Fact]
    public void MutableProperties_DefaultValues()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act & Assert
        command.CustId.Should().Be(456); // Set from constructor
        command.ErrCode.Should().Be(0);
        command.ErrMsg.Should().BeNull();
        command.ErrExtraInfo.Should().BeNull();
    }

    [Fact]
    public void GetSettingKey_ReturnsCorrectValue()
    {
        // Arrange
        var custDBGroupId = 123;
        var command = new CreateDummyMemberCommand(custDBGroupId, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be($"SiteDB{custDBGroupId}_Acc_Dep_CustomerInsMember_Ag");
        result.Should().Be("SiteDB123_Acc_Dep_CustomerInsMember_Ag");
    }

    [Fact]
    public void GetSettingKey_WithZeroCustDBGroupId_ReturnsBaseName()
    {
        // Arrange
        var custDBGroupId = 0;
        var command = new CreateDummyMemberCommand(custDBGroupId, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        var result = command.GetSettingKey();

        // Assert
        result.Should().Be("Acc_Dep_CustomerInsMember_Ag");
    }

    [Fact]
    public void IsValid_WithValidParameters_ReturnsTrue()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithZeroCustId_ReturnsFalse()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 0, "testuser", 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNegativeCustId_ReturnsFalse()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, -1, "testuser", 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullUsername_ReturnsFalse()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, null, 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyUsername_ReturnsFalse()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "", 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceUsername_ReturnsFalse()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "   ", 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, "user1", true)]
    [InlineData(100, "user100", true)]
    [InlineData(999, "testuser", true)]
    [InlineData(int.MaxValue, "maxuser", true)]
    public void IsValid_WithValidCombinations_ReturnsTrue(int custId, string username, bool expected)
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, custId, username, 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, "user", false)]
    [InlineData(-1, "user", false)]
    [InlineData(123, null, false)]
    [InlineData(123, "", false)]
    [InlineData(123, "   ", false)]
    [InlineData(0, null, false)]
    [InlineData(-1, "", false)]
    public void IsValid_WithInvalidCombinations_ReturnsFalse(int custId, string username, bool expected)
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, custId, username, 789, 999, "192.168.1.1");

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InheritsFromNonQueryCommand()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act & Assert
        command.Should().BeAssignableTo<NonQueryCommand>();
    }

    [Fact]
    public void Constructor_CreatesIndependentInstances()
    {
        // Arrange
        var custDBGroupId1 = 111;
        var custId1 = 222;
        var username1 = "user1";
        var creator1 = 333;
        var recommend1 = 444;
        var ip1 = "192.168.1.1";

        var custDBGroupId2 = 555;
        var custId2 = 666;
        var username2 = "user2";
        var creator2 = 777;
        var recommend2 = 888;
        var ip2 = "192.168.1.2";

        // Act
        var command1 = new CreateDummyMemberCommand(custDBGroupId1, custId1, username1, creator1, recommend1, ip1);
        var command2 = new CreateDummyMemberCommand(custDBGroupId2, custId2, username2, creator2, recommend2, ip2);

        // Assert
        command1.Should().NotBeSameAs(command2);
        command1.CustDBGroupId.Should().Be(custDBGroupId1);
        command1.CustId.Should().Be(custId1);
        command1.Username.Should().Be(username1);
        command1.Creator.Should().Be(creator1);
        command1.Recommend.Should().Be(recommend1);
        command1.IP.Should().Be(ip1);
        command2.CustDBGroupId.Should().Be(custDBGroupId2);
        command2.CustId.Should().Be(custId2);
        command2.Username.Should().Be(username2);
        command2.Creator.Should().Be(creator2);
        command2.Recommend.Should().Be(recommend2);
        command2.IP.Should().Be(ip2);
    }

    [Theory]
    [InlineData(1, 1, "user1", 1, 1, "192.168.1.1")]
    [InlineData(100, 200, "user200", 300, 400, "10.0.0.1")]
    [InlineData(0, 300, "user300", -100, 500, null)]
    [InlineData(-50, -400, "user-400", int.MaxValue, int.MinValue, "")]
    public void Constructor_WithVariousParameters_InitializesCorrectly(int custDBGroupId, int custId, string username, int creator, int recommend, string ip)
    {
        // Arrange & Act
        var command = new CreateDummyMemberCommand(custDBGroupId, custId, username, creator, recommend, ip);

        // Assert
        command.CustDBGroupId.Should().Be(custDBGroupId);
        command.CustId.Should().Be(custId);
        command.Username.Should().Be(username);
        command.Creator.Should().Be(creator);
        command.Recommend.Should().Be(recommend);
        command.IP.Should().Be(ip);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(-50)]
    public void GetSettingKey_UsesSiteDBHelpers(int custDBGroupId)
    {
        // Arrange
        var command = new CreateDummyMemberCommand(custDBGroupId, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        var result = command.GetSettingKey();

        // Assert
        if (custDBGroupId == 0)
        {
            result.Should().Be("Acc_Dep_CustomerInsMember_Ag");
        }
        else
        {
            result.Should().Be($"SiteDB{custDBGroupId}_Acc_Dep_CustomerInsMember_Ag");
        }
    }

    [Fact]
    public void ValidationLogic_RequiresBothCustIdAndUsername()
    {
        // Arrange & Act & Assert
        new CreateDummyMemberCommand(123, 1, "valid", 789, 999, "192.168.1.1").IsValid().Should().BeTrue();
        new CreateDummyMemberCommand(123, 100, "valid", 789, 999, "192.168.1.1").IsValid().Should().BeTrue();

        new CreateDummyMemberCommand(123, 0, "valid", 789, 999, "192.168.1.1").IsValid().Should().BeFalse();
        new CreateDummyMemberCommand(123, -1, "valid", 789, 999, "192.168.1.1").IsValid().Should().BeFalse();
        new CreateDummyMemberCommand(123, 100, null, 789, 999, "192.168.1.1").IsValid().Should().BeFalse();
        new CreateDummyMemberCommand(123, 100, "", 789, 999, "192.168.1.1").IsValid().Should().BeFalse();
        new CreateDummyMemberCommand(123, 100, "   ", 789, 999, "192.168.1.1").IsValid().Should().BeFalse();
    }

    [Fact]
    public void IsValid_MatchesValidationCondition()
    {
        // Arrange
        var validCommand = new CreateDummyMemberCommand(123, 42, "validuser", 789, 999, "192.168.1.1");
        var invalidCustIdCommand = new CreateDummyMemberCommand(123, 0, "validuser", 789, 999, "192.168.1.1");
        var invalidUsernameCommand = new CreateDummyMemberCommand(123, 42, null, 789, 999, "192.168.1.1");
        var bothInvalidCommand = new CreateDummyMemberCommand(123, 0, "", 789, 999, "192.168.1.1");

        // Act & Assert
        validCommand.IsValid().Should().Be(validCommand.CustId > 0 && !string.IsNullOrWhiteSpace(validCommand.Username));
        invalidCustIdCommand.IsValid().Should().Be(invalidCustIdCommand.CustId > 0 && !string.IsNullOrWhiteSpace(invalidCustIdCommand.Username));
        invalidUsernameCommand.IsValid().Should().Be(invalidUsernameCommand.CustId > 0 && !string.IsNullOrWhiteSpace(invalidUsernameCommand.Username));
        bothInvalidCommand.IsValid().Should().Be(bothInvalidCommand.CustId > 0 && !string.IsNullOrWhiteSpace(bothInvalidCommand.Username));
    }

    [Fact]
    public void GetSettingKey_IsConsistentForSameInstance()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        var result1 = command.GetSettingKey();
        var result2 = command.GetSettingKey();

        // Assert
        result1.Should().Be(result2);
        result1.Should().Be("SiteDB123_Acc_Dep_CustomerInsMember_Ag");
    }

    [Fact]
    public void ValidationBoundaryConditions_AreTested()
    {
        // Arrange & Act & Assert
        // Test CustId boundary at zero
        new CreateDummyMemberCommand(123, 0, "user", 789, 999, "192.168.1.1").IsValid().Should().BeFalse();
        new CreateDummyMemberCommand(123, 1, "user", 789, 999, "192.168.1.1").IsValid().Should().BeTrue();

        // Test negative CustId boundary
        new CreateDummyMemberCommand(123, -1, "user", 789, 999, "192.168.1.1").IsValid().Should().BeFalse();

        // Test extreme CustId values
        new CreateDummyMemberCommand(123, int.MaxValue, "user", 789, 999, "192.168.1.1").IsValid().Should().BeTrue();
        new CreateDummyMemberCommand(123, int.MinValue, "user", 789, 999, "192.168.1.1").IsValid().Should().BeFalse();

        // Test username edge cases
        new CreateDummyMemberCommand(123, 100, "a", 789, 999, "192.168.1.1").IsValid().Should().BeTrue(); // Single character
        new CreateDummyMemberCommand(123, 100, " a ", 789, 999, "192.168.1.1").IsValid().Should().BeTrue(); // Whitespace around valid content
    }

    [Fact]
    public void Command_HasNoMutableStateForReadOnlyProperties()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");
        var initialCustDBGroupId = command.CustDBGroupId;
        var initialUsername = command.Username;
        var initialCreator = command.Creator;
        var initialRecommend = command.Recommend;
        var initialIP = command.IP;
        var initialSettingKey = command.GetSettingKey();

        // Act - Call methods multiple times
        var custDBGroupId1 = command.CustDBGroupId;
        var username1 = command.Username;
        var creator1 = command.Creator;
        var recommend1 = command.Recommend;
        var ip1 = command.IP;
        var settingKey1 = command.GetSettingKey();

        // Assert - All calls should return same values
        custDBGroupId1.Should().Be(initialCustDBGroupId);
        username1.Should().Be(initialUsername);
        creator1.Should().Be(initialCreator);
        recommend1.Should().Be(initialRecommend);
        ip1.Should().Be(initialIP);
        settingKey1.Should().Be(initialSettingKey);
    }

    [Fact]
    public void AllMutableProperties_CanBeSetToNull()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        command.ErrMsg = null;
        command.ErrExtraInfo = null;

        // Assert
        command.ErrMsg.Should().BeNull();
        command.ErrExtraInfo.Should().BeNull();
    }

    [Fact]
    public void CustId_CanBeModifiedAfterConstruction()
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");
        var originalCustId = command.CustId;

        // Act
        command.CustId = 777;

        // Assert
        command.CustId.Should().Be(777);
        command.CustId.Should().NotBe(originalCustId);

        // Validation should reflect the new value
        command.IsValid().Should().BeTrue();

        // Setting to invalid value should affect validation
        command.CustId = 0;
        command.IsValid().Should().BeFalse();
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(500)]
    [InlineData(int.MaxValue)]
    public void CustId_AcceptsVariousValues(int custId)
    {
        // Arrange
        var command = new CreateDummyMemberCommand(123, 456, "testuser", 789, 999, "192.168.1.1");

        // Act
        command.CustId = custId;

        // Assert
        command.CustId.Should().Be(custId);
    }

    [Fact]
    public void ValidationDoesNotDependOnOtherParameters()
    {
        // Arrange & Act & Assert
        // Validation should only depend on CustId and Username, not on other constructor parameters
        new CreateDummyMemberCommand(-999, 1, "valid", -888, -777, null).IsValid().Should().BeTrue();
        new CreateDummyMemberCommand(int.MaxValue, 1, "valid", int.MinValue, 0, "").IsValid().Should().BeTrue();
        new CreateDummyMemberCommand(0, 1, "valid", 0, 0, "   ").IsValid().Should().BeTrue();

        new CreateDummyMemberCommand(999, 0, "valid", 888, 777, "192.168.1.1").IsValid().Should().BeFalse();
        new CreateDummyMemberCommand(999, 1, null, 888, 777, "192.168.1.1").IsValid().Should().BeFalse();
    }
}