namespace Fanex.DepositLicenseeAPI.Tests.HealthChecks;

using System.Collections.ObjectModel;
using System.Net;
using DepositLicenseeAPI.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ServerHealthCheckTests
{
    private readonly ServerHealthCheck _healthCheck;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public ServerHealthCheckTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["Environment"]).Returns("Test");
        _healthCheck = new ServerHealthCheck(_mockHttpContextAccessor.Object, _mockConfiguration.Object);
    }

    [Fact]
    public void Constructor_WithValidDependencies_InitializesCorrectly()
    {
        // Arrange
        var httpContextAccessor = new Mock<IHttpContextAccessor>().Object;
        var configuration = new Mock<IConfiguration>().Object;

        // Act
        var healthCheck = new ServerHealthCheck(httpContextAccessor, configuration);

        // Assert
        healthCheck.Should().NotBeNull();
        healthCheck.Should().BeAssignableTo<IHealthCheck>();
    }

    [Fact]
    public async Task CheckHealthAsync_ReturnsHealthyResult()
    {
        // Arrange
        var context = new HealthCheckContext();

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Description.Should().Be("Server Info");
    }

    [Fact]
    public async Task CheckHealthAsync_IncludesEnvironmentInData()
    {
        // Arrange
        var context = new HealthCheckContext();
        _mockConfiguration.Setup(c => c["Environment"]).Returns("Production");

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().ContainKey("Env");
        result.Data["Env"].Should().Be("Production");
    }

    [Fact]
    public async Task CheckHealthAsync_IncludesHostNameInData()
    {
        // Arrange
        var context = new HealthCheckContext();

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().ContainKey("Host Name");
        result.Data["Host Name"].Should().NotBeNull();
        result.Data["Host Name"].Should().BeOfType<string>();
    }

    [Fact]
    public async Task CheckHealthAsync_IncludesServerIpAddresses()
    {
        // Arrange
        var context = new HealthCheckContext();

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().ContainKey("Host Name");
        // Should contain at least one IP address entry
        var ipKeys = result.Data.Keys.Where(k => k.StartsWith("IP")).ToList();
        ipKeys.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CheckHealthAsync_WithHttpContext_IncludesConnectionInfo()
    {
        // Arrange
        var context = new HealthCheckContext();
        var httpContext = new DefaultHttpContext();
        httpContext.Connection.LocalIpAddress = IPAddress.Parse("127.0.0.1");
        httpContext.Connection.LocalPort = 5000;
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("192.168.1.100");
        httpContext.Request.Host = new HostString("localhost:5000");
        httpContext.Request.Scheme = "https";

        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().ContainKey("Local Ip");
        result.Data.Should().ContainKey("Local Port");
        result.Data.Should().ContainKey("Client Ip");
        result.Data.Should().ContainKey("Host");
        result.Data.Should().ContainKey("Schema");

        result.Data["Local Ip"].Should().Be("127.0.0.1");
        result.Data["Local Port"].Should().Be(5000);
        result.Data["Client Ip"].Should().Be("192.168.1.100");
        result.Data["Host"].Should().Be(new HostString("localhost:5000"));
        result.Data["Schema"].Should().Be("https");
    }

    [Fact]
    public async Task CheckHealthAsync_WithNullHttpContext_DoesNotIncludeConnectionInfo()
    {
        // Arrange
        var context = new HealthCheckContext();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns((HttpContext)null);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().NotContainKey("Local Ip");
        result.Data.Should().NotContainKey("Local Port");
        result.Data.Should().NotContainKey("Client Ip");
        result.Data.Should().NotContainKey("Host");
        result.Data.Should().NotContainKey("Schema");
    }

    [Fact]
    public async Task CheckHealthAsync_IncludesServerTime()
    {
        // Arrange
        var context = new HealthCheckContext();
        var httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().ContainKey("Server Time");
        result.Data["Server Time"].Should().NotBeNull();
        result.Data["Server Time"].Should().BeOfType<string>();
    }

    [Fact]
    public async Task CheckHealthAsync_IncludesLastUpdatedInfo()
    {
        // Arrange
        var context = new HealthCheckContext();
        var httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().ContainKey("Last Updated");
        result.Data["Last Updated"].Should().NotBeNull();
        result.Data["Last Updated"].Should().BeOfType<string>();
    }

    [Fact]
    public async Task CheckHealthAsync_DataIsReadOnly()
    {
        // Arrange
        var context = new HealthCheckContext();

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().BeOfType<ReadOnlyDictionary<string, object>>();
    }

    [Fact]
    public async Task CheckHealthAsync_WithCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        var context = new HealthCheckContext();
        var cancellationToken = new CancellationToken();

        // Act
        var result = await _healthCheck.CheckHealthAsync(context, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_WithNullEnvironment_HandlesGracefully()
    {
        // Arrange
        var context = new HealthCheckContext();
        _mockConfiguration.Setup(c => c["Environment"]).Returns((string)null);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Data.Should().ContainKey("Env");
        result.Data["Env"].Should().BeNull();
    }

    [Fact]
    public async Task CheckHealthAsync_WithEmptyEnvironment_HandlesGracefully()
    {
        // Arrange
        var context = new HealthCheckContext();
        _mockConfiguration.Setup(c => c["Environment"]).Returns("");

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Data.Should().ContainKey("Env");
        result.Data["Env"].Should().Be("");
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Staging")]
    [InlineData("Production")]
    [InlineData("Test")]
    public async Task CheckHealthAsync_WithDifferentEnvironments_ReflectsCorrectly(string environment)
    {
        // Arrange
        var context = new HealthCheckContext();
        _mockConfiguration.Setup(c => c["Environment"]).Returns(environment);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data["Env"].Should().Be(environment);
    }

    [Fact]
    public async Task CheckHealthAsync_AlwaysReturnsHealthyStatus()
    {
        // Arrange
        var context = new HealthCheckContext();

        // Test with various configurations
        var configurations = new[]
        {
            null,
            "",
            "Development",
            "Production"
        };

        foreach (var env in configurations)
        {
            _mockConfiguration.Setup(c => c["Environment"]).Returns(env);

            // Act
            var result = await _healthCheck.CheckHealthAsync(context);

            // Assert
            result.Status.Should().Be(HealthStatus.Healthy, $"Should be healthy for environment: {env}");
        }
    }

    [Fact]
    public async Task CheckHealthAsync_WithPartialHttpContextInfo_HandlesNullValues()
    {
        // Arrange
        var context = new HealthCheckContext();
        var httpContext = new DefaultHttpContext();
        // Leave some connection properties as null
        httpContext.Connection.LocalIpAddress = null;
        httpContext.Connection.RemoteIpAddress = null;

        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Data.Should().ContainKey("Local Ip");
        result.Data.Should().ContainKey("Client Ip");
        result.Data["Local Ip"].Should().BeNull();
        result.Data["Client Ip"].Should().BeNull();
    }

    [Fact]
    public async Task CheckHealthAsync_ImplementsIHealthCheckCorrectly()
    {
        // Arrange
        IHealthCheck healthCheck = _healthCheck;
        var context = new HealthCheckContext();

        // Act
        var result = await healthCheck.CheckHealthAsync(context);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_ContainsRequiredDataKeys()
    {
        // Arrange
        var context = new HealthCheckContext();
        var httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        var requiredKeys = new[] { "Env", "Host Name" };
        foreach (var key in requiredKeys)
        {
            result.Data.Should().ContainKey(key, $"Data should contain key: {key}");
        }

        var contextRequiredKeys = new[] { "Server Time", "Last Updated" };
        foreach (var key in contextRequiredKeys)
        {
            result.Data.Should().ContainKey(key, $"Data should contain key: {key} when HttpContext is available");
        }
    }

    [Fact]
    public async Task CheckHealthAsync_ServerTimeIsRecentTime()
    {
        // Arrange
        var context = new HealthCheckContext();
        var httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);
        var beforeCheck = DateTimeOffset.Now;

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);
        var afterCheck = DateTimeOffset.Now;

        // Assert
        result.Data.Should().ContainKey("Server Time");
        var serverTimeStr = result.Data["Server Time"] as string;
        serverTimeStr.Should().NotBeNullOrEmpty();

        // Parse and verify the time is reasonable (within the test execution window)
        if (DateTime.TryParse(serverTimeStr, out DateTime serverTime))
        {
            serverTime.Should().BeOnOrAfter(beforeCheck.DateTime.AddSeconds(-1));
            serverTime.Should().BeOnOrBefore(afterCheck.DateTime.AddSeconds(1));
        }
    }

    [Fact]
    public async Task CheckHealthAsync_ConsistentResultStructure()
    {
        // Arrange
        var context = new HealthCheckContext();

        // Act
        var result1 = await _healthCheck.CheckHealthAsync(context);
        var result2 = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result1.Status.Should().Be(result2.Status);
        result1.Description.Should().Be(result2.Description);

        // Should have consistent keys (though values might differ for time-based entries)
        var commonKeys = result1.Data.Keys.Intersect(result2.Data.Keys);
        commonKeys.Should().Contain("Env");
        commonKeys.Should().Contain("Host Name");
    }

    [Fact]
    public void ServerHealthCheck_HasCorrectConstructorSignature()
    {
        // Arrange & Act & Assert
        var constructor = typeof(ServerHealthCheck).GetConstructors().Single();
        var parameters = constructor.GetParameters();

        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(IHttpContextAccessor));
        parameters[1].ParameterType.Should().Be(typeof(IConfiguration));
    }

    [Fact]
    public void ServerHealthCheck_ImplementsIHealthCheck()
    {
        // Arrange & Act & Assert
        typeof(ServerHealthCheck).Should().Implement<IHealthCheck>();
    }

    [Fact]
    public async Task CheckHealthAsync_WithComplexHttpContext_CapturesAllInfo()
    {
        // Arrange
        var context = new HealthCheckContext();
        var httpContext = new DefaultHttpContext();
        httpContext.Connection.LocalIpAddress = IPAddress.Parse("10.0.0.1");
        httpContext.Connection.LocalPort = 8080;
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("203.0.113.1");
        httpContext.Request.Host = new HostString("api.example.com", 443);
        httpContext.Request.Scheme = "https";

        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data["Local Ip"].Should().Be("10.0.0.1");
        result.Data["Local Port"].Should().Be(8080);
        result.Data["Client Ip"].Should().Be("203.0.113.1");
        result.Data["Host"].Should().Be(new HostString("api.example.com", 443));
        result.Data["Schema"].Should().Be("https");
    }
}