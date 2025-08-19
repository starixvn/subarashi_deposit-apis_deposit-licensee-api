namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts;

using System.ComponentModel.DataAnnotations;
using DepositLicenseeAPI.Features.Accounts;
using DepositLicenseeAPI.Features.Shared;

public class AccountModelsTests
{
    [Fact]
    public void CreateDummyMemberResult_CreatesCorrectly()
    {
        // Arrange
        var errorCode = 0;
        var errorMessage = "Success";
        var custId = 123;
        var username = "testuser";

        // Act
        var result = new CreateDummyMemberResult(errorCode, errorMessage, custId, username);

        // Assert
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
        result.CustId.Should().Be(custId);
        result.Username.Should().Be(username);
    }

    [Fact]
    public void CreateDummyMemberResult_InheritsFromResult()
    {
        // Arrange & Act
        var result = new CreateDummyMemberResult(0, "Success", 123, "testuser");

        // Assert
        result.Should().BeAssignableTo<Result>();
    }

    [Fact]
    public void CreateDummyMemberResult_WithErrorCode_SetsIsOKCorrectly()
    {
        // Arrange & Act
        var successResult = new CreateDummyMemberResult(0, "Success", 123, "testuser");
        var errorResult = new CreateDummyMemberResult(404, "Not found", 0, "");

        // Assert
        successResult.IsOK.Should().BeTrue();
        successResult.IsNotOK.Should().BeFalse();
        errorResult.IsOK.Should().BeFalse();
        errorResult.IsNotOK.Should().BeTrue();
    }

    [Fact]
    public void CreateDummyMemberRequest_CreatesCorrectly()
    {
        // Arrange
        var agentId = 456;
        var creator = 789;
        var ip = "192.168.1.100";

        // Act
        var request = new CreateDummyMemberRequest(agentId, creator, ip);

        // Assert
        request.AgentId.Should().Be(agentId);
        request.Creator.Should().Be(creator);
        request.IP.Should().Be(ip);
    }

    [Fact]
    public void CreateDummyMemberRequest_WithNullIP_CreatesCorrectly()
    {
        // Arrange & Act
        var request = new CreateDummyMemberRequest(456, 789, null);

        // Assert
        request.AgentId.Should().Be(456);
        request.Creator.Should().Be(789);
        request.IP.Should().BeNull();
    }

    [Fact]
    public void CreateDummyMemberRequest_WithEmptyIP_CreatesCorrectly()
    {
        // Arrange & Act
        var request = new CreateDummyMemberRequest(456, 789, "");

        // Assert
        request.AgentId.Should().Be(456);
        request.Creator.Should().Be(789);
        request.IP.Should().BeEmpty();
    }

    [Fact]
    public void CreateDummyMemberRequest_Properties_AreNotNull()
    {
        // Arrange
        var type = typeof(CreateDummyMemberRequest);
        var properties = new[] { "AgentId", "Creator", "IP" };

        // Act & Assert
        foreach (var propertyName in properties)
        {
            var propertyInfo = type.GetProperty(propertyName);
            propertyInfo.Should().NotBeNull($"{propertyName} property should exist");
        }
    }

    [Fact]
    public void CreateDummyMemberRequest_IPProperty_DoesNotHaveRequiredAttribute()
    {
        // Arrange
        var type = typeof(CreateDummyMemberRequest);
        var propertyInfo = type.GetProperty("IP");

        // Act
        var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false)
            .Cast<RequiredAttribute>().FirstOrDefault();

        // Assert
        requiredAttribute.Should().BeNull("IP property should not have Required attribute");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(int.MaxValue, int.MaxValue)]
    [InlineData(int.MinValue, int.MinValue)]
    public void CreateDummyMemberRequest_WithVariousValues_CreatesCorrectly(int agentId, int creator)
    {
        // Arrange & Act
        var request = new CreateDummyMemberRequest(agentId, creator, "test-ip");

        // Assert
        request.AgentId.Should().Be(agentId);
        request.Creator.Should().Be(creator);
    }

    [Theory]
    [InlineData(0, "Success", 123, "user1")]
    [InlineData(404, "Not found", 0, "")]
    [InlineData(-1, "Error", -1, null)]
    public void CreateDummyMemberResult_WithVariousValues_CreatesCorrectly(
        int errorCode,
        string errorMessage,
        int custId,
        string username)
    {
        // Arrange & Act
        var result = new CreateDummyMemberResult(errorCode, errorMessage, custId, username);

        // Assert
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
        result.CustId.Should().Be(custId);
        result.Username.Should().Be(username);
    }
}