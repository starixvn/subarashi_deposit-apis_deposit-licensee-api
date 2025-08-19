namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees;

using Data.Repository;
using DepositLicenseeAPI.Features.Licensees;
using DepositLicenseeAPI.Features.Licensees.Criteria;
using DepositLicenseeAPI.Features.Shared;
using DepositLicenseeAPI.Features.Shared.SiteDB;
using DepositLicenseeAPI.Features.UserLog;
using Logging;
using Microsoft.Extensions.Configuration;

public class BaseHandlerTests
{
    private readonly IConfiguration _configuration;
    private readonly TestableBaseHandler _handler;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ISiteDBHandler> _mockSiteDBHandler;
    private readonly Mock<IUserLogHandler> _mockUserLogHandler;

    public BaseHandlerTests()
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

        _handler = new TestableBaseHandler(
            _configuration,
            _mockRepository.Object,
            _mockSiteDBHandler.Object,
            _mockUserLogHandler.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task ValidateRequest_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var siteName = "TestSite";
        byte mode = 1;
        var request = CreateValidCurrencyRequest();
        var custDBGroupId = 100;

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<SiteBuilderValidateCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is SiteBuilderValidateCommand validateCmd)
                {
                    validateCmd.ErrCode = 0;
                    validateCmd.ErrMsg = "";
                    validateCmd.RegisterUsernames = "user1,user2";
                }
            });

        // Act
        var result = await _handler.TestValidateRequest(siteName, mode, request, custDBGroupId);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(0);
        result.ErrorMessage.Should().BeEmpty();
        result.RegisterUsernames.Should().Be("user1,user2");
        result.IsOK.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateRequest_WithValidationFailure_ReturnsErrorResult()
    {
        // Arrange
        var siteName = "TestSite";
        byte mode = 1;
        var request = CreateValidCurrencyRequest();
        var custDBGroupId = 100;

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
        var result = await _handler.TestValidateRequest(siteName, mode, request, custDBGroupId);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(400);
        result.ErrorMessage.Should().Be("Validation failed");
        result.RegisterUsernames.Should().BeEmpty();
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterCustomers_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var registerUsernames = "user1,user2";
        var siteName = "TestSite";
        var productStakeLevel = new[] { new StakeLevel(1, 1, 1.0m, 100.0m) };
        var ip = "192.168.1.1";
        byte mode = 1;
        var siteDBGroupId = 10;

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterCustomersCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterCustomersCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.SiteId = 123;
                    regCmd.Customers = "customer1,customer2";
                    regCmd.FailedCurrencies = "USD,EUR";
                }
            });

        // Act
        var result = await _handler.TestRegisterCustomers(registerUsernames, siteName, productStakeLevel, ip, mode, siteDBGroupId);

        // Assert
        result.result.Should().NotBeNull();
        result.result.ErrorCode.Should().Be(0);
        result.result.SiteId.Should().Be(123);
        result.result.Customers.Should().Be("customer1,customer2");
        result.failedCurrencies.Should().Equal("USD", "EUR");
    }

    [Fact]
    public async Task RegisterCustomers_WithErrExtraInfo_LogsExtraInfo()
    {
        // Arrange
        var registerUsernames = "user1,user2";
        var siteName = "TestSite";
        var productStakeLevel = new[] { new StakeLevel(1, 1, 1.0m, 100.0m) };
        var ip = "192.168.1.1";
        byte mode = 1;
        var siteDBGroupId = 10;

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterCustomersCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterCustomersCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.SiteId = 123;
                    regCmd.Customers = "customer1,customer2";
                    regCmd.ErrExtraInfo = "Some extra error information";
                }
            });

        // Act
        await _handler.TestRegisterCustomers(registerUsernames, siteName, productStakeLevel, ip, mode, siteDBGroupId);

        // Assert
        _mockLogger.Verify(x => x.ErrorAsync(It.Is<string>(msg => msg.Contains("Some extra error information"))), Times.Once);
    }

    [Fact]
    public async Task RegisterCustomers_WithNullFailedCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var registerUsernames = "user1,user2";
        var siteName = "TestSite";
        var productStakeLevel = new[] { new StakeLevel(1, 1, 1.0m, 100.0m) };
        var ip = "192.168.1.1";
        byte mode = 1;
        var siteDBGroupId = 10;

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterCustomersCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterCustomersCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.SiteId = 123;
                    regCmd.Customers = "customer1,customer2";
                    regCmd.FailedCurrencies = null;
                }
            });

        // Act
        var result = await _handler.TestRegisterCustomers(registerUsernames, siteName, productStakeLevel, ip, mode, siteDBGroupId);

        // Assert
        result.failedCurrencies.Should().BeEmpty();
    }

    [Fact]
    public async Task WriteLogs_WithMultipleLogParams_WritesAllLogs()
    {
        // Arrange
        var custDBGroupId = 100;
        var logParams = new List<LogParam>
        {
            new() { CustId = 1, ActionId = 1, TrackingValue = "TEST1" },
            new() { CustId = 2, ActionId = 2, TrackingValue = "TEST2" },
            new() { CustId = 3, ActionId = 3, TrackingValue = "TEST3" }
        };

        // Act
        await _handler.TestWriteLogs(custDBGroupId, logParams);

        // Assert
        _mockUserLogHandler.Verify(
            x => x.WriteLog(custDBGroupId, It.IsAny<LogParam>()),
            Times.Exactly(logParams.Count));
    }

    [Fact]
    public async Task WriteLogs_WithEmptyLogParams_DoesNotCallWriteLog()
    {
        // Arrange
        var custDBGroupId = 100;
        var logParams = new List<LogParam>();

        // Act
        await _handler.TestWriteLogs(custDBGroupId, logParams);

        // Assert
        _mockUserLogHandler.Verify(
            x => x.WriteLog(It.IsAny<int>(), It.IsAny<LogParam>()),
            Times.Never);
    }

    [Fact]
    public async Task InsertBalanceException_WithValidData_CallsRepository()
    {
        // Arrange
        var insBalExceptCustList = "123,456,789";
        var siteDBGroupId = 10;

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertBalanceExceptionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.TestInsertBalanceException(insBalExceptCustList, siteDBGroupId);

        // Assert
        _mockRepository.Verify(
            x => x.ExecuteAsync(It.Is<InsertBalanceExceptionCommand>(cmd =>
                cmd.InsBalExceptCustList == insBalExceptCustList &&
                cmd.SiteDBGroupId == siteDBGroupId), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task InsertBalanceException_WhenExceptionThrown_LogsError()
    {
        // Arrange
        var insBalExceptCustList = "123,456,789";
        var siteDBGroupId = 10;
        var exception = new Exception("Database error");

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertBalanceExceptionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        await _handler.TestInsertBalanceException(insBalExceptCustList, siteDBGroupId);

        // Assert
        _mockLogger.Verify(x => x.ErrorAsync("Database error", exception), Times.Once);
    }

    [Fact]
    public async Task RollbackMultiple_WithCustIds_CallsRepository()
    {
        // Arrange
        var custIds = "123,456,789";

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RollbackCreateLicenseeCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.TestRollbackMultiple(custIds);

        // Assert
        result.Should().Be(1);
        _mockRepository.Verify(
            x => x.ExecuteAsync(It.Is<RollbackCreateLicenseeCommand>(cmd => cmd.CustIds == custIds), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task InsertCompanyWinLimit_WithValidLicenseeId_ReturnsSuccessResult()
    {
        // Arrange
        var licenseeId = 100;

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCompanyWinLimitCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCompanyWinLimitCommand insertCmd)
                {
                    insertCmd.ErrCode = 0;
                    insertCmd.ErrMsg = "";
                }
            });

        // Act
        var result = await _handler.TestInsertCompanyWinLimit(licenseeId);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(0);
        result.ErrorMessage.Should().BeEmpty();
        result.IsOK.Should().BeTrue();
    }

    [Fact]
    public async Task InsertCompanyWinLimit_WithError_ReturnsErrorResult()
    {
        // Arrange
        var licenseeId = 100;

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertCompanyWinLimitCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is InsertCompanyWinLimitCommand insertCmd)
                {
                    insertCmd.ErrCode = 500;
                    insertCmd.ErrMsg = "Insert failed";
                }
            });

        // Act
        var result = await _handler.TestInsertCompanyWinLimit(licenseeId);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(500);
        result.ErrorMessage.Should().Be("Insert failed");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public void BaseHandler_InitializesAllDependencies()
    {
        // Act & Assert
        _handler.Configuration.Should().Be(_configuration);
        _handler.Repository.Should().Be(_mockRepository.Object);
        _handler.SiteDBHandler.Should().Be(_mockSiteDBHandler.Object);
        _handler.UserLogHandler.Should().Be(_mockUserLogHandler.Object);
        _handler.Logger.Should().Be(_mockLogger.Object);
        _handler.SettingStatus.Should().Be(1);
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

    private class TestableBaseHandler : BaseHandler
    {
        public TestableBaseHandler(
            IConfiguration configuration,
            IDynamicRepository repository,
            ISiteDBHandler siteDBHandler,
            IUserLogHandler userLogHandler,
            ILogger logger)
            : base(configuration, repository, siteDBHandler, userLogHandler, logger)
        {
        }

        public IConfiguration Configuration => configuration;
        public IDynamicRepository Repository => repository;
        public ISiteDBHandler SiteDBHandler => siteDBHandler;
        public IUserLogHandler UserLogHandler => userLogHandler;
        public ILogger Logger => logger;
        public new byte SettingStatus => base.SettingStatus;

        public Task<ValidateCreateLicenseeResult> TestValidateRequest(string siteName, byte mode, CreateCurrencyRequest request, int custDBGroupId)
            => ValidateRequest(siteName, mode, request, custDBGroupId);

        public Task<(RegisterCustomersResult result, List<string> failedCurrencies)> TestRegisterCustomers(
            string registerUsernames,
            string siteName,
            StakeLevel[] productStakeLevel,
            string ip,
            byte mode,
            int siteDBGroupId,
            bool isChinaView = false,
            byte specialPlatform = 0,
            byte siteType = 0)
            => RegisterCustomers(new RegisterCustomersOptions(registerUsernames, siteName, productStakeLevel, ip, mode, siteDBGroupId, isChinaView, specialPlatform, siteType));

        public Task TestWriteLogs(int custDBGroupId, IEnumerable<LogParam> logParams)
            => WriteLogs(custDBGroupId, logParams);

        public Task TestInsertBalanceException(string insBalExceptCustList, int siteDBGroupId)
            => InsertBalanceException(insBalExceptCustList, siteDBGroupId);

        public Task<int> TestRollbackMultiple(string custIds)
            => RollbackMultiple(custIds);

        public Task<Result> TestInsertCompanyWinLimit(int licenseeId)
            => InsertCompanyWinLimit(licenseeId);
    }
}