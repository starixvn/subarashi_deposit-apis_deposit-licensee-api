namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees;
using DepositLicenseeAPI.Features.Licensees.Criteria;
using DepositLicenseeAPI.Features.Shared.SiteDB;
using DepositLicenseeAPI.Features.UserLog;
using Logging;
using Microsoft.Extensions.Configuration;

public class CurrencyHandlerTests
{
    private readonly IConfiguration _configuration;
    private readonly CurrencyHandler _handler;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ISiteDBHandler> _mockSiteDBHandler;
    private readonly Mock<IUserLogHandler> _mockUserLogHandler;

    public CurrencyHandlerTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "SettingStatus", "1" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _mockRepository = new Mock<IDynamicRepository>();
        _mockSiteDBHandler = new Mock<ISiteDBHandler>();
        _mockUserLogHandler = new Mock<IUserLogHandler>();
        _mockLogger = new Mock<ILogger>();

        _handler = new CurrencyHandler(
            _configuration,
            _mockRepository.Object,
            _mockSiteDBHandler.Object,
            _mockUserLogHandler.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateCurrency_WhenSiteDBConfigIsNull_ReturnsError911()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(911);
        result.ErrorMessage.Should().Be("Create customer failed. Please try again.");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateCurrency_WhenValidationFails_ReturnsValidationError()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<SiteBuilderValidateCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is SiteBuilderValidateCommand validateCmd)
                {
                    validateCmd.ErrCode = 400;
                    validateCmd.ErrMsg = "Validation failed";
                    validateCmd.RegisterUsernames = "";
                }
            });

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(400);
        result.ErrorMessage.Should().Be("Validation failed");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateCurrency_WhenRegisterCustomersFails_ReturnsRegistrationError()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<SiteBuilderValidateCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is SiteBuilderValidateCommand validateCmd)
                {
                    validateCmd.ErrCode = 0;
                    validateCmd.ErrMsg = "";
                    validateCmd.RegisterUsernames = "testuser1,testuser2";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterCustomersCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterCustomersCommand regCmd)
                {
                    regCmd.ErrCode = 500;
                    regCmd.ErrMsg = "Registration failed";
                    regCmd.SiteId = 0;
                    regCmd.Customers = "";
                }
            });

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(500);
        result.ErrorMessage.Should().Be("Registration failed");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateCurrency_WhenCreateCurrencyFails_ReturnsCreateError()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        SetupSuccessfulValidationAndRegistration(request);

        _mockRepository
            .Setup(x => x.FetchAsync<LogParam>(It.IsAny<CreateCurrencyCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ICriteria, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateCurrencyCriteria createCmd)
                {
                    createCmd.ErrCode = 600;
                    createCmd.ErrMsg = "Currency creation failed";
                }
            })
            .ReturnsAsync(new List<LogParam>());

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(600);
        result.ErrorMessage.Should().Be("Currency creation failed");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateCurrency_WhenInsertCurrencyDataFails_ReturnsSuccess()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        SetupSuccessfulValidationAndRegistration(request);

        var logParams = new List<LogParam>
        {
            new() { CustId = 123, ActionId = 1, TrackingValue = "CREATE" }
        };

        _mockRepository
            .Setup(x => x.FetchAsync<LogParam>(It.IsAny<CreateCurrencyCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ICriteria, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateCurrencyCriteria createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                    createCmd.CurrencyList = "USD,EUR";
                    createCmd.PT_Sportsbook = 0.75m;
                }
            })
            .ReturnsAsync(logParams);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCurrencyCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCurrencyCommand insertCmd)
                {
                    insertCmd.ErrCode = 700;
                    insertCmd.ErrMsg = "Insert currency failed";
                }
            });

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(0);
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.IsOK.Should().BeTrue();
    }

    [Fact]
    public async Task CreateCurrency_WhenInsertCompanyWinLimitFails_ReturnsSuccess()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        SetupSuccessfulValidationAndRegistration(request);

        var logParams = new List<LogParam>
        {
            new() { CustId = 123, ActionId = 1, TrackingValue = "CREATE" }
        };

        _mockRepository
            .Setup(x => x.FetchAsync<LogParam>(It.IsAny<CreateCurrencyCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ICriteria, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateCurrencyCriteria createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                    createCmd.CurrencyList = "USD,EUR";
                    createCmd.PT_Sportsbook = 0.75m;
                }
            })
            .ReturnsAsync(logParams);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCurrencyCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCurrencyCommand insertCmd)
                {
                    insertCmd.ErrCode = 0;
                    insertCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCompanyWinLimitCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCompanyWinLimitCommand winLimitCmd)
                {
                    winLimitCmd.ErrCode = 800;
                    winLimitCmd.ErrMsg = "Insert company win limit failed";
                }
            });

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(0);
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.IsOK.Should().BeTrue();
    }

    [Fact]
    public async Task CreateCurrency_WhenAllOperationsSucceed_ReturnsSuccessResult()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        SetupSuccessfulValidationAndRegistration(request);

        var logParams = new List<LogParam>
        {
            new() { CustId = 123, ActionId = 1, TrackingValue = "CREATE" }
        };

        _mockRepository
            .Setup(x => x.FetchAsync<LogParam>(It.IsAny<CreateCurrencyCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ICriteria, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateCurrencyCriteria createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                    createCmd.CurrencyList = "USD,EUR";
                    createCmd.PT_Sportsbook = 0.75m;
                }
            })
            .ReturnsAsync(logParams);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCurrencyCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCompanyWinLimitCommand winLimitCmd)
                {
                    winLimitCmd.ErrCode = 0;
                    winLimitCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCompanyWinLimitCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCompanyWinLimitCommand winLimitCmd)
                {
                    winLimitCmd.ErrCode = 0;
                    winLimitCmd.ErrMsg = "";
                }
            });

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(0);
        result.ErrorMessage.Should().BeEmpty();
        result.IsOK.Should().BeTrue();
    }

    [Fact]
    public async Task CreateCurrency_WithInsBalExceptCustList_InsertsBalanceException()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        SetupSuccessfulValidationAndRegistration(request);

        _mockRepository
            .Setup(x => x.FetchAsync<LogParam>(It.IsAny<CreateCurrencyCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ICriteria, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateCurrencyCriteria createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                    createCmd.InsBalExceptCustList = "123,456,789";
                    createCmd.CurrencyList = "USD";
                    createCmd.PT_Sportsbook = 0.5m;
                }
            })
            .ReturnsAsync(new List<LogParam>());

        SetupSuccessfulCurrencyInsertion();

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.IsOK.Should().BeTrue();

        await Task.Delay(100);

        _mockRepository.Verify(
            x => x.ExecuteAsync(It.Is<InsertBalanceExceptionCommand>(cmd =>
                cmd.InsBalExceptCustList == "123,456,789" &&
                cmd.SiteDBGroupId == siteDBConfig.SiteDBGroupId), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateCurrency_WithListSuperRollBack_ExecutesRollback()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        SetupSuccessfulValidationAndRegistration(request);

        _mockRepository
            .Setup(x => x.FetchAsync<LogParam>(It.IsAny<CreateCurrencyCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ICriteria, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateCurrencyCriteria createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                    createCmd.ListSuperRollBack = "123,456,789";
                    createCmd.CurrencyList = "USD";
                    createCmd.PT_Sportsbook = 0.5m;
                }
            })
            .ReturnsAsync(new List<LogParam>());

        SetupSuccessfulCurrencyInsertion();

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RollbackCreateLicenseeCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.IsOK.Should().BeTrue();

        _mockRepository.Verify(
            x => x.ExecuteAsync(It.Is<RollbackCreateLicenseeCommand>(cmd => cmd.CustIds == "123,456,789"), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateCurrency_WithLogParams_WritesLogs()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var siteDBConfig = new SiteDBConfig { CustDBGroupId = 1, SiteDBGroupId = 1 };

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync(siteDBConfig);

        SetupSuccessfulValidationAndRegistration(request);

        var logParams = new List<LogParam>
        {
            new() { CustId = 123, ActionId = 1, TrackingValue = "CREATE" },
            new() { CustId = 124, ActionId = 1, TrackingValue = "CREATE" }
        };

        _mockRepository
            .Setup(x => x.FetchAsync<LogParam>(It.IsAny<CreateCurrencyCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ICriteria, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateCurrencyCriteria createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                    createCmd.CurrencyList = "USD";
                    createCmd.PT_Sportsbook = 0.5m;
                }
            })
            .ReturnsAsync(logParams);

        SetupSuccessfulCurrencyInsertion();

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.IsOK.Should().BeTrue();

        await Task.Delay(200);

        _mockUserLogHandler.Verify(
            x => x.WriteLog(siteDBConfig.CustDBGroupId, It.IsAny<LogParam>()),
            Times.Exactly(logParams.Count));
    }

    [Fact]
    public async Task CreateCurrency_WithNullArrays_HandlesCorrectly()
    {
        // Arrange
        var request = new CreateCurrencyRequest(
            "TestSite",
            null,
            null,
            null,
            null,
            "TestUser",
            "192.168.1.1",
            true,
            "Standard");

        _mockSiteDBHandler
            .Setup(x => x.GetBySite(request.SiteName))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _handler.CreateCurrency(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(911);
        result.IsOK.Should().BeFalse();
    }

    private void SetupSuccessfulValidationAndRegistration(CreateCurrencyRequest request)
    {
        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<SiteBuilderValidateCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is SiteBuilderValidateCommand validateCmd)
                {
                    validateCmd.ErrCode = 0;
                    validateCmd.ErrMsg = "";
                    validateCmd.RegisterUsernames = "testuser1,testuser2";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterCustomersCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterCustomersCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.SiteId = 100;
                    regCmd.Customers = "customer1,customer2";
                    regCmd.FailedCurrencies = "";
                }
            });
    }

    private void SetupSuccessfulCurrencyInsertion()
    {
        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCurrencyCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCurrencyCommand insertCmd)
                {
                    insertCmd.ErrCode = 0;
                    insertCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCompanyWinLimitCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCompanyWinLimitCommand winLimitCmd)
                {
                    winLimitCmd.ErrCode = 0;
                    winLimitCmd.ErrMsg = "";
                }
            });
    }

    private static CreateCurrencyRequest CreateValidCurrencyRequest()
    {
        return new CreateCurrencyRequest(
            "TestSite",
            new[] { new BetSetting(1, 1, 1, 1, 10m, 1000m, 0.1m, 0.2m) },
            new[] { new PositionTaking(1, 0.5m) },
            new[] { new StakeLevel(1, 1, 1m, 100m) },
            new[] { new Commission(1, 0.05m) },
            "TestUser",
            "192.168.1.1",
            true,
            "Standard");
    }
}