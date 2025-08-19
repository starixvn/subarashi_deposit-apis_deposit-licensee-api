namespace Fanex.DepositLicenseeAPI.Tests.HealthChecks;

using System.Collections.ObjectModel;
using Data.Vault;
using DepositLicenseeAPI.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class VaultHealthCheckTests
{
    private readonly VaultHealthCheck _healthCheck;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<VaultDbAliasService> _mockVaultDbAliasService;

    public VaultHealthCheckTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockVaultDbAliasService = new Mock<VaultDbAliasService>();
        _healthCheck = new VaultHealthCheck(_mockConfiguration.Object, _mockVaultDbAliasService.Object);
    }

    [Fact]
    public void Constructor_WithValidDependencies_InitializesCorrectly()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>().Object;
        var vaultService = new Mock<VaultDbAliasService>().Object;

        // Act
        var healthCheck = new VaultHealthCheck(configuration, vaultService);

        // Assert
        healthCheck.Should().NotBeNull();
        healthCheck.Should().BeAssignableTo<IHealthCheck>();
    }

    [Fact]
    public async Task CheckHealthAsync_ReturnsHealthyResult()
    {
        // Arrange
        var context = new HealthCheckContext();
        SetupMockConfiguration(new Dictionary<string, string>());

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Description.Should().Be("Vault Info");
    }

    [Fact]
    public async Task CheckHealthAsync_WithConnectionStrings_RetrievesVaultInfo()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["Database1.ConnectionString"] = "connection1",
            ["Database2.ConnectionString"] = "connection2"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("Database1")).Returns("192.168.1.1");
        _mockVaultDbAliasService.Setup(v => v.GetIp("Database2")).Returns("192.168.1.2");

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().ContainKey("Database1");
        result.Data.Should().ContainKey("Database2");
        result.Data["Database1"].Should().Be("192.168.1.1");
        result.Data["Database2"].Should().Be("192.168.1.2");
    }

    [Fact]
    public async Task CheckHealthAsync_WithNoConnectionStrings_ReturnsEmptyData()
    {
        // Arrange
        var context = new HealthCheckContext();
        SetupMockConfiguration(new Dictionary<string, string>());

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().BeEmpty();
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_WithDuplicateAliases_OnlyIncludesDistinct()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["Database1.ConnectionString"] = "connection1",
            ["Database1.ReadOnly"] = "connection1-readonly",
            ["Database2.ConnectionString"] = "connection2"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("Database1")).Returns("192.168.1.1");
        _mockVaultDbAliasService.Setup(v => v.GetIp("Database2")).Returns("192.168.1.2");

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().HaveCount(2);
        result.Data.Should().ContainKey("Database1");
        result.Data.Should().ContainKey("Database2");
        result.Data["Database1"].Should().Be("192.168.1.1");
        result.Data["Database2"].Should().Be("192.168.1.2");
    }

    [Fact]
    public async Task CheckHealthAsync_WithComplexConnectionStringNames_ExtractsCorrectAliases()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["MainDB.Primary.ConnectionString"] = "connection1",
            ["UserDB.Secondary.ReadOnly"] = "connection2",
            ["LogDB.Archive.ConnectionString"] = "connection3"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("MainDB")).Returns("10.0.0.1");
        _mockVaultDbAliasService.Setup(v => v.GetIp("UserDB")).Returns("10.0.0.2");
        _mockVaultDbAliasService.Setup(v => v.GetIp("LogDB")).Returns("10.0.0.3");

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().HaveCount(3);
        result.Data.Should().ContainKey("MainDB");
        result.Data.Should().ContainKey("UserDB");
        result.Data.Should().ContainKey("LogDB");
        result.Data["MainDB"].Should().Be("10.0.0.1");
        result.Data["UserDB"].Should().Be("10.0.0.2");
        result.Data["LogDB"].Should().Be("10.0.0.3");
    }

    [Fact]
    public async Task CheckHealthAsync_DataIsReadOnly()
    {
        // Arrange
        var context = new HealthCheckContext();
        SetupMockConfiguration(new Dictionary<string, string>
        {
            ["TestDB.ConnectionString"] = "connection"
        });

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
        SetupMockConfiguration(new Dictionary<string, string>());

        // Act
        var result = await _healthCheck.CheckHealthAsync(context, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_CallsVaultServiceForEachAlias()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["DB1.ConnectionString"] = "connection1",
            ["DB2.ConnectionString"] = "connection2"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("DB1")).Returns("192.168.1.1");
        _mockVaultDbAliasService.Setup(v => v.GetIp("DB2")).Returns("192.168.1.2");

        // Act
        await _healthCheck.CheckHealthAsync(context);

        // Assert
        _mockVaultDbAliasService.Verify(v => v.GetIp("DB1"), Times.Once);
        _mockVaultDbAliasService.Verify(v => v.GetIp("DB2"), Times.Once);
    }

    [Fact]
    public async Task CheckHealthAsync_WithNullVaultServiceResponse_HandlesGracefully()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["TestDB.ConnectionString"] = "connection"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("TestDB")).Returns((string)null);

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Data.Should().ContainKey("TestDB");
        result.Data["TestDB"].Should().BeNull();
    }

    [Fact]
    public async Task CheckHealthAsync_WithEmptyVaultServiceResponse_HandlesGracefully()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["TestDB.ConnectionString"] = "connection"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("TestDB")).Returns("");

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Data.Should().ContainKey("TestDB");
        result.Data["TestDB"].Should().Be("");
    }

    [Fact]
    public async Task CheckHealthAsync_AlwaysReturnsHealthyStatus()
    {
        // Arrange
        var context = new HealthCheckContext();

        // Test with various scenarios
        var scenarios = new[]
        {
            new Dictionary<string, string>(), // Empty
            new Dictionary<string, string> { ["DB1.Connection"] = "conn1" }, // One connection
            new Dictionary<string, string> // Multiple connections
            {
                ["DB1.ConnectionString"] = "conn1",
                ["DB2.ConnectionString"] = "conn2",
                ["DB3.ReadOnly"] = "conn3"
            }
        };

        foreach (var connectionStrings in scenarios)
        {
            SetupMockConfiguration(connectionStrings);

            // Setup vault service to return various responses
            foreach (var kvp in connectionStrings)
            {
                var alias = kvp.Key.Split('.').FirstOrDefault();
                _mockVaultDbAliasService.Setup(v => v.GetIp(alias)).Returns($"192.168.1.{connectionStrings.Count}");
            }

            // Act
            var result = await _healthCheck.CheckHealthAsync(context);

            // Assert
            result.Status.Should().Be(HealthStatus.Healthy, $"Should be healthy for scenario with {connectionStrings.Count} connections");
        }
    }

    [Fact]
    public async Task CheckHealthAsync_ImplementsIHealthCheckCorrectly()
    {
        // Arrange
        IHealthCheck healthCheck = _healthCheck;
        var context = new HealthCheckContext();
        SetupMockConfiguration(new Dictionary<string, string>());

        // Act
        var result = await healthCheck.CheckHealthAsync(context);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_WithConnectionStringsContainingNullNames_HandlesGracefully()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["ValidDB.ConnectionString"] = "connection1",
            [".ConnectionString"] = "connection2", // Empty alias
            ["..ConnectionString"] = "connection3" // Multiple dots
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp(It.IsAny<string>())).Returns("192.168.1.1");

        // Act & Assert
        var act = async () => await _healthCheck.CheckHealthAsync(context);
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public void VaultHealthCheck_HasCorrectConstructorSignature()
    {
        // Arrange & Act & Assert
        var constructor = typeof(VaultHealthCheck).GetConstructors().Single();
        var parameters = constructor.GetParameters();

        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(IConfiguration));
        parameters[1].ParameterType.Should().Be(typeof(VaultDbAliasService));
    }

    [Fact]
    public void VaultHealthCheck_ImplementsIHealthCheck()
    {
        // Arrange & Act & Assert
        typeof(VaultHealthCheck).Should().Implement<IHealthCheck>();
    }

    [Fact]
    public async Task CheckHealthAsync_WithMixedConnectionStringFormats_ExtractsAllAliases()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["Database"] = "simple",
            ["DB.ConnectionString"] = "standard",
            ["Service.DB.Primary"] = "complex",
            ["Another.Service.DB.Backup.Connection"] = "very complex"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("Database")).Returns("192.168.1.1");
        _mockVaultDbAliasService.Setup(v => v.GetIp("DB")).Returns("192.168.1.2");
        _mockVaultDbAliasService.Setup(v => v.GetIp("Service")).Returns("192.168.1.3");
        _mockVaultDbAliasService.Setup(v => v.GetIp("Another")).Returns("192.168.1.4");

        // Act
        var result = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result.Data.Should().HaveCount(4);
        result.Data.Should().ContainKey("Database");
        result.Data.Should().ContainKey("DB");
        result.Data.Should().ContainKey("Service");
        result.Data.Should().ContainKey("Another");
    }

    [Fact]
    public async Task CheckHealthAsync_ReturnsTaskFromResult()
    {
        // Arrange
        var context = new HealthCheckContext();
        SetupMockConfiguration(new Dictionary<string, string>());

        // Act
        var task = _healthCheck.CheckHealthAsync(context);

        // Assert
        task.Should().NotBeNull();
        task.Should().BeOfType<Task<HealthCheckResult>>();

        var result = await task;
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CheckHealthAsync_ConsistentResultStructure()
    {
        // Arrange
        var context = new HealthCheckContext();
        var connectionStrings = new Dictionary<string, string>
        {
            ["TestDB.ConnectionString"] = "connection"
        };

        SetupMockConfiguration(connectionStrings);
        _mockVaultDbAliasService.Setup(v => v.GetIp("TestDB")).Returns("192.168.1.1");

        // Act
        var result1 = await _healthCheck.CheckHealthAsync(context);
        var result2 = await _healthCheck.CheckHealthAsync(context);

        // Assert
        result1.Status.Should().Be(result2.Status);
        result1.Description.Should().Be(result2.Description);
        result1.Data.Keys.Should().BeEquivalentTo(result2.Data.Keys);
    }

    private void SetupMockConfiguration(Dictionary<string, string> connectionStrings)
    {
        var connectionStringsSection = new Mock<IConfigurationSection>();
        var children = connectionStrings.Select(kvp =>
        {
            var child = new Mock<IConfigurationSection>();
            child.Setup(c => c["Name"]).Returns(kvp.Key);
            child.Setup(c => c.Value).Returns(kvp.Value);
            return child.Object;
        }).ToArray();

        connectionStringsSection.Setup(c => c.GetChildren()).Returns(children);
        _mockConfiguration.Setup(c => c.GetSection("ConnectionStrings")).Returns(connectionStringsSection.Object);
    }
}