namespace Fanex.DepositLicenseeAPI.Tests.Extensions;

using System.Reflection;
using Data.Repository;
using Data.Vault;
using DepositLicenseeAPI.Extensions;
using DepositLicenseeAPI.Features.Accounts;
using DepositLicenseeAPI.Features.Licensees;
using DepositLicenseeAPI.Features.Shared.SiteDB;
using DepositLicenseeAPI.Features.UserLog;
using Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _services;

    public ServiceCollectionExtensionsTests()
    {
        _services = new ServiceCollection();

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
        {
            ["Sentry:Environment"] = "Test",
            ["Sentry:Dsn"] = "https://test@sentry.io/123456",
            ["Sentry:MaxBreadcrumbs"] = "100",
            ["Sentry:SendDefaultPii"] = "false",
            ["Environment"] = "Test"
        });
        _configuration = configurationBuilder.Build();
    }

    [Fact]
    public void AddLogging_ReturnsServiceCollection()
    {
        // Arrange & Act
        var result = _services.AddLogging(_configuration);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddLogging_RegistersILogger()
    {
        // Arrange & Act
        _services.AddLogging(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        var logger = serviceProvider.GetService<ILogger>();
        logger.Should().NotBeNull();
        logger.Should().BeOfType<Logger>();
    }

    [Fact]
    public void AddLogging_ConfiguresSerilog()
    {
        // Arrange & Act
        _services.AddLogging(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        // Verify that Serilog ILogger is registered
        var serilogLogger = serviceProvider.GetService<Serilog.ILogger>();
        serilogLogger.Should().NotBeNull();
    }

    [Fact]
    public void AddLogging_WithNullConfiguration_ThrowsException()
    {
        // Arrange
        IConfiguration nullConfig = null;

        // Act
        Action act = () => _services.AddLogging(nullConfig);

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void AddLogging_WithEmptyConfiguration_ThrowsException()
    {
        // Arrange
        var emptyConfig = new ConfigurationBuilder().Build();

        // Act
        Action act = () => _services.AddLogging(emptyConfig);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddLogging_WithMissingConfigValues_ThrowsException()
    {
        // Arrange
        var partialConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Test"
                // Missing Sentry:Dsn will cause exception
            })
            .Build();

        // Act & Assert
        var act = () => _services.AddLogging(partialConfig);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddDependencyInjection_ReturnsServiceCollection()
    {
        // Arrange & Act
        var result = _services.AddDependencyInjection(_configuration);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddDependencyInjection_RegistersIDynamicRepository()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        var service = serviceProvider.GetService<IDynamicRepository>();
        service.Should().NotBeNull();
        service.Should().BeOfType<DynamicRepository>();
    }

    [Fact]
    public void AddDependencyInjection_RegistersIUserLogHandler()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);

        // Assert
        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IUserLogHandler));
        serviceDescriptor.Should().NotBeNull();
        serviceDescriptor.ImplementationType.Should().Be(typeof(UserLogHandler));
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddDependencyInjection_RegistersISiteDBHandler()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        var service = serviceProvider.GetService<ISiteDBHandler>();
        service.Should().NotBeNull();
        service.Should().BeOfType<SiteDBHandler>();
    }

    [Fact]
    public void AddDependencyInjection_RegistersILicenseesHandler()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);

        // Assert
        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ILicenseesHandler));
        serviceDescriptor.Should().NotBeNull();
        serviceDescriptor.ImplementationType.Should().Be(typeof(LicenseesHandler));
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddDependencyInjection_RegistersICurrencyHandler()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);

        // Assert
        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ICurrencyHandler));
        serviceDescriptor.Should().NotBeNull();
        serviceDescriptor.ImplementationType.Should().Be(typeof(CurrencyHandler));
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddDependencyInjection_RegistersIAccountsHandler()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);

        // Assert
        var serviceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IAccountsHandler));
        serviceDescriptor.Should().NotBeNull();
        serviceDescriptor.ImplementationType.Should().Be(typeof(AccountsHandler));
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddDependencyInjection_RegistersVaultDbAliasService()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        var service = serviceProvider.GetService<VaultDbAliasService>();
        service.Should().NotBeNull();
        service.Should().BeOfType<VaultDbAliasServiceImp>();
    }

    [Fact]
    public void AddDependencyInjection_AllServicesAreSingleton()
    {
        // Arrange & Act
        _services.AddDependencyInjection(_configuration);

        // Assert
        // Verify that all registered services have Singleton lifetime
        var serviceTypes = new[]
        {
            typeof(IDynamicRepository),
            typeof(IUserLogHandler),
            typeof(ISiteDBHandler),
            typeof(ILicenseesHandler),
            typeof(ICurrencyHandler),
            typeof(IAccountsHandler),
            typeof(VaultDbAliasService)
        };

        foreach (var serviceType in serviceTypes)
        {
            var descriptor = _services.FirstOrDefault(s => s.ServiceType == serviceType);
            descriptor.Should().NotBeNull($"Service {serviceType.Name} should be registered");
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton, $"Service {serviceType.Name} should be singleton");
        }
    }

    [Fact]
    public void AddDependencyInjection_WithNullConfiguration_DoesNotThrow()
    {
        // Arrange
        IConfiguration nullConfig = null;

        // Act & Assert
        var act = () => _services.AddDependencyInjection(nullConfig);
        act.Should().NotThrow();
    }

    [Fact]
    public void AddLogging_ExtensionMethod_ExtendsIServiceCollection()
    {
        // Arrange & Act & Assert
        var method = typeof(ServiceCollectionExtensions).GetMethod(nameof(ServiceCollectionExtensions.AddLogging));
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(IServiceCollection));
        parameters[1].ParameterType.Should().Be(typeof(IConfiguration));

        method.ReturnType.Should().Be(typeof(IServiceCollection));
    }

    [Fact]
    public void AddDependencyInjection_ExtensionMethod_ExtendsIServiceCollection()
    {
        // Arrange & Act & Assert
        var method = typeof(ServiceCollectionExtensions).GetMethod(nameof(ServiceCollectionExtensions.AddDependencyInjection));
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(IServiceCollection));
        parameters[1].ParameterType.Should().Be(typeof(IConfiguration));

        method.ReturnType.Should().Be(typeof(IServiceCollection));
    }

    [Fact]
    public void ServiceCollectionExtensions_IsStaticClass()
    {
        // Arrange & Act & Assert
        var type = typeof(ServiceCollectionExtensions);
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
    }

    [Fact]
    public void AddLogging_ConfiguresHttpLogging()
    {
        // Arrange & Act
        _services.AddLogging(_configuration);

        // Assert
        // Verify that HTTP logging was configured by checking if services were added
        _services.Should().NotBeEmpty();
        _services.Should().Contain(s => s.ServiceType.Name.Contains("HttpLogging") ||
                                        s.ServiceType.FullName.Contains("HttpLogging") ||
                                        s.ServiceType.FullName.Contains("Microsoft.AspNetCore.HttpLogging"));
    }

    [Fact]
    public void AddLogging_CanBeCalled_MultipleTimesWithoutError()
    {
        // Arrange & Act & Assert
        var act = () =>
        {
            _services.AddLogging(_configuration);
            _services.AddLogging(_configuration);
            _services.AddLogging(_configuration);
        };

        act.Should().NotThrow();
    }

    [Fact]
    public void AddDependencyInjection_CanBeCalled_MultipleTimesWithoutError()
    {
        // Arrange & Act & Assert
        var act = () =>
        {
            _services.AddDependencyInjection(_configuration);
            _services.AddDependencyInjection(_configuration);
            _services.AddDependencyInjection(_configuration);
        };

        act.Should().NotThrow();
    }

    [Fact]
    public void AddLogging_WithDifferentConfigurations_ThrowsForMissingDsn()
    {
        // Arrange
        var config1 = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Development",
                ["Environment"] = "Development"
                // Missing Sentry:Dsn
            })
            .Build();

        var config2 = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Production",
                ["Environment"] = "Production"
                // Missing Sentry:Dsn
            })
            .Build();

        // Act & Assert
        var act1 = () => _services.AddLogging(config1);
        var act2 = () => _services.AddLogging(config2);

        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ExtensionMethods_CanBeChained()
    {
        // Arrange & Act
        var result = _services
            .AddLogging(_configuration)
            .AddDependencyInjection(_configuration);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddLogging_RegistersCorrectServiceLifetime()
    {
        // Arrange & Act
        _services.AddLogging(_configuration);

        // Assert
        var loggerDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ILogger));
        loggerDescriptor.Should().NotBeNull();
        loggerDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }


    [Fact]
    public void AddLogging_WithInvalidSentryDsn_ThrowsException()
    {
        // Arrange
        var invalidConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Test",
                ["Sentry:Dsn"] = "invalid-dsn-url",
                ["Environment"] = "Test"
            })
            .Build();

        // Act & Assert
        var act = () => _services.AddLogging(invalidConfig);
        act.Should().Throw<UriFormatException>();
    }

    [Fact]
    public void AddLogging_WithMissingEnvironment_HandlesGracefully()
    {
        // Arrange
        var configWithoutEnv = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Dsn"] = "https://test@sentry.io/123456"
                // Missing Environment
            })
            .Build();

        // Act & Assert
        var act = () => _services.AddLogging(configWithoutEnv);
        act.Should().NotThrow();
    }

    [Fact]
    public void ServiceCollectionExtensions_OnlyContainsExtensionMethods()
    {
        // Arrange & Act & Assert
        var type = typeof(ServiceCollectionExtensions);
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

        methods.Should().HaveCount(2);
        methods.Should().Contain(m => m.Name == nameof(ServiceCollectionExtensions.AddLogging));
        methods.Should().Contain(m => m.Name == nameof(ServiceCollectionExtensions.AddDependencyInjection));

        // All methods should be extension methods (first parameter should be 'this IServiceCollection')
        foreach (var method in methods)
        {
            var firstParam = method.GetParameters().FirstOrDefault();
            firstParam.Should().NotBeNull();
            firstParam.ParameterType.Should().Be(typeof(IServiceCollection));
        }
    }

    [Fact]
    public void AddLogging_WithCompleteConfiguration_ConfiguresAllComponents()
    {
        // Arrange
        var fullConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Production",
                ["Sentry:Dsn"] = "https://test@sentry.io/123456",
                ["Sentry:MaxBreadcrumbs"] = "50",
                ["Sentry:SendDefaultPii"] = "true",
                ["Environment"] = "Production"
            })
            .Build();

        // Act
        var result = _services.AddLogging(fullConfig);

        // Assert
        result.Should().BeSameAs(_services);

        // Build service provider to verify registrations
        var serviceProvider = _services.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger>();
        logger.Should().NotBeNull();
    }

    [Fact]
    public void AddLogging_WithNumericConfigValues_ParsesCorrectly()
    {
        // Arrange
        var configWithNumbers = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Test",
                ["Sentry:Dsn"] = "https://test@sentry.io/123456",
                ["Sentry:MaxBreadcrumbs"] = "75", // Numeric value
                ["Sentry:SendDefaultPii"] = "false", // Boolean value
                ["Environment"] = "Test"
            })
            .Build();

        // Act & Assert
        var act = () => _services.AddLogging(configWithNumbers);
        act.Should().NotThrow();
    }

    [Fact]
    public void AddLogging_WithInvalidBooleanConfig_HandlesGracefully()
    {
        // Arrange
        var configWithInvalidBool = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Test",
                ["Sentry:Dsn"] = "https://test@sentry.io/123456",
                ["Sentry:SendDefaultPii"] = "invalid-bool", // Invalid boolean
                ["Environment"] = "Test"
            })
            .Build();

        // Act & Assert
        var act = () => _services.AddLogging(configWithInvalidBool);
        act.Should().NotThrow(); // Should handle invalid boolean gracefully
    }

    [Fact]
    public void AddLogging_WithInvalidNumberConfig_HandlesGracefully()
    {
        // Arrange
        var configWithInvalidNumber = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Sentry:Environment"] = "Test",
                ["Sentry:Dsn"] = "https://test@sentry.io/123456",
                ["Sentry:MaxBreadcrumbs"] = "invalid-number", // Invalid number
                ["Environment"] = "Test"
            })
            .Build();

        // Act & Assert
        var act = () => _services.AddLogging(configWithInvalidNumber);
        act.Should().NotThrow(); // Should handle invalid number gracefully and use default
    }

    [Fact]
    public void AddDependencyInjection_ServiceResolution_WorksCorrectly()
    {
        // Arrange
        _services.AddSingleton(_configuration); // Register configuration
        _services.AddLogging(_configuration); // Need to add logging first
        _services.AddDependencyInjection(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Act & Assert - Verify all services can be resolved
        var dynamicRepo = serviceProvider.GetService<IDynamicRepository>();
        var userLogHandler = serviceProvider.GetService<IUserLogHandler>();
        var siteDBHandler = serviceProvider.GetService<ISiteDBHandler>();
        var licenseesHandler = serviceProvider.GetService<ILicenseesHandler>();
        var currencyHandler = serviceProvider.GetService<ICurrencyHandler>();
        var accountsHandler = serviceProvider.GetService<IAccountsHandler>();
        var vaultService = serviceProvider.GetService<VaultDbAliasService>();

        dynamicRepo.Should().NotBeNull();
        userLogHandler.Should().NotBeNull();
        siteDBHandler.Should().NotBeNull();
        licenseesHandler.Should().NotBeNull();
        currencyHandler.Should().NotBeNull();
        accountsHandler.Should().NotBeNull();
        vaultService.Should().NotBeNull();
    }

    [Fact]
    public void ServiceCollectionExtensions_HasCorrectNamespace()
    {
        // Arrange & Act & Assert
        var type = typeof(ServiceCollectionExtensions);
        type.Namespace.Should().Be("Fanex.DepositLicenseeAPI.Extensions");
    }

    [Fact]
    public void ServiceCollectionExtensions_AllMethodsReturnIServiceCollection()
    {
        // Arrange & Act
        var type = typeof(ServiceCollectionExtensions);
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.DeclaringType == type);

        // Assert
        foreach (var method in methods)
        {
            method.ReturnType.Should().Be(typeof(IServiceCollection),
                $"Method {method.Name} should return IServiceCollection for fluent API");
        }
    }
}