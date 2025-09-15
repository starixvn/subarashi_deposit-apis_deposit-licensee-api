namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts;

using Data.Repository;
using DepositLicenseeAPI.Features.Accounts;
using DepositLicenseeAPI.Features.Accounts.Criteria;
using DepositLicenseeAPI.Features.Shared.SiteDB;
using Logging;

public class AccountsHandlerTests
{
    private readonly AccountsHandler _handler;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ISiteDBHandler> _mockSiteDBHandler;

    public AccountsHandlerTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockSiteDBHandler = new Mock<ISiteDBHandler>();
        _mockLogger = new Mock<ILogger>();

        _handler = new AccountsHandler(_mockRepository.Object, _mockSiteDBHandler.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateDummyMember_WhenSiteDBConfigIsNull_ReturnsError911()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(123, 456, "192.168.1.1");

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _handler.CreateDummyMember(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(911);
        result.ErrorMessage.Should().Be("Create customer failed. Please try again.");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateDummyMember_WhenCheckOddsAdjustmentFails_ReturnsOddsAdjustmentError()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(123, 456, "192.168.1.1");
        var siteDBConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };
        var siteId = "SITE001";

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockSiteDBHandler
            .Setup(x => x.GetSiteId(siteDBConfig.Site))
            .ReturnsAsync(siteId);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CheckOddsAdjCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CheckOddsAdjCommand checkCmd)
                {
                    checkCmd.ErrCode = 400;
                    checkCmd.ErrMsg = "Odds adjustment check failed";
                }
            });

        // Act
        var result = await _handler.CreateDummyMember(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(400);
        result.ErrorMessage.Should().Be("Odds adjustment check failed");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateDummyMember_WhenGenerateDummyMemberFails_ReturnsGenerationError()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(123, 456, "192.168.1.1");
        var siteDBConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };
        var siteId = "SITE001";

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockSiteDBHandler
            .Setup(x => x.GetSiteId(siteDBConfig.Site))
            .ReturnsAsync(siteId);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CheckOddsAdjCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CheckOddsAdjCommand checkCmd)
                {
                    checkCmd.ErrCode = 0;
                    checkCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterDummyMemberCommand regCmd)
                {
                    regCmd.ErrCode = 500;
                    regCmd.ErrMsg = "Failed to generate dummy member";
                    regCmd.CustId = 0;
                    regCmd.Username = "";
                }
            });

        // Act
        var result = await _handler.CreateDummyMember(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(500);
        result.ErrorMessage.Should().Be("Failed to generate dummy member");
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateDummyMember_WhenCreateMemberFails_ReturnsCreateMemberError()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(123, 456, "192.168.1.1");
        var siteDBConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };
        var siteId = "SITE001";

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockSiteDBHandler
            .Setup(x => x.GetSiteId(siteDBConfig.Site))
            .ReturnsAsync(siteId);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CheckOddsAdjCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CheckOddsAdjCommand checkCmd)
                {
                    checkCmd.ErrCode = 0;
                    checkCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterDummyMemberCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.CustId = 789;
                    regCmd.Username = "testuser123";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateDummyMemberCommand createCmd)
                {
                    createCmd.ErrCode = 600;
                    createCmd.ErrMsg = "Failed to create member";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RollbackCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.CreateDummyMember(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(600);
        result.ErrorMessage.Should().Be("Failed to create member");
        result.IsOK.Should().BeFalse();

        _mockRepository.Verify(x => x.ExecuteAsync(It.IsAny<RollbackCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateDummyMember_WhenAllOperationsSucceed_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(123, 456, "192.168.1.1");
        var siteDBConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };
        var siteId = "SITE001";

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockSiteDBHandler
            .Setup(x => x.GetSiteId(siteDBConfig.Site))
            .ReturnsAsync(siteId);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CheckOddsAdjCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CheckOddsAdjCommand checkCmd)
                {
                    checkCmd.ErrCode = 0;
                    checkCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterDummyMemberCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.CustId = 789;
                    regCmd.Username = "testuser123";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateDummyMemberCommand createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertBalanceExceptionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.CreateDummyMember(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(0);
        result.ErrorMessage.Should().BeEmpty();
        result.IsOK.Should().BeTrue();

        var createDummyResult = result.Should().BeOfType<CreateDummyMemberResult>().Subject;
        createDummyResult.CustId.Should().Be(789);
        createDummyResult.Username.Should().Be("testuser123");

        _mockRepository.Verify(x => x.ExecuteAsync(It.IsAny<InsertBalanceExceptionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(x => x.ExecuteAsync(It.IsAny<RollbackCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateDummyMember_WhenCreateMemberHasErrExtraInfo_LogsExtraInfo()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(123, 456, "192.168.1.1");
        var siteDBConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };
        var siteId = "SITE001";

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockSiteDBHandler
            .Setup(x => x.GetSiteId(siteDBConfig.Site))
            .ReturnsAsync(siteId);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CheckOddsAdjCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CheckOddsAdjCommand checkCmd)
                {
                    checkCmd.ErrCode = 0;
                    checkCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is RegisterDummyMemberCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.CustId = 789;
                    regCmd.Username = "testuser123";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is CreateDummyMemberCommand createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                    createCmd.ErrExtraInfo = "Some extra error information";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertBalanceExceptionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.CreateDummyMember(request);

        // Assert
        result.Should().NotBeNull();
        result.IsOK.Should().BeTrue();

        _mockLogger.Verify(x => x.ErrorAsync(It.Is<string>(msg => msg.Contains("Some extra error information"))), Times.Once);
    }

    [Theory]
    [InlineData(123, 456, "192.168.1.1")]
    [InlineData(0, 0, null)]
    [InlineData(int.MaxValue, int.MaxValue, "")]
    public async Task CreateDummyMember_WithVariousRequestParameters_HandlesCorrectly(int agentId, int creator, string ip)
    {
        // Arrange
        var request = new CreateDummyMemberRequest(agentId, creator, ip);

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _handler.CreateDummyMember(request);

        // Assert
        result.Should().NotBeNull();
        result.ErrorCode.Should().Be(911);
        result.IsOK.Should().BeFalse();
    }

    [Fact]
    public async Task CreateDummyMember_VerifiesRepositoryCallsInCorrectOrder()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(123, 456, "192.168.1.1");
        var siteDBConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };
        var siteId = "SITE001";
        var callOrder = new List<string>();

        _mockSiteDBHandler
            .Setup(x => x.GetByCustId(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockSiteDBHandler
            .Setup(x => x.GetSiteId(siteDBConfig.Site))
            .ReturnsAsync(siteId);

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CheckOddsAdjCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                callOrder.Add("CheckOdds");
                if (cmd is CheckOddsAdjCommand checkCmd)
                {
                    checkCmd.ErrCode = 0;
                    checkCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                callOrder.Add("RegisterDummy");
                if (cmd is RegisterDummyMemberCommand regCmd)
                {
                    regCmd.ErrCode = 0;
                    regCmd.ErrMsg = "";
                    regCmd.CustId = 789;
                    regCmd.Username = "test";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateDummyMemberCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                callOrder.Add("CreateMember");
                if (cmd is CreateDummyMemberCommand createCmd)
                {
                    createCmd.ErrCode = 0;
                    createCmd.ErrMsg = "";
                }
            });

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<InsertBalanceExceptionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) => { callOrder.Add("InsertBalance"); })
            .ReturnsAsync(1);

        // Act
        await _handler.CreateDummyMember(request);

        // Assert
        callOrder.Should().Equal("CheckOdds", "RegisterDummy", "CreateMember", "InsertBalance");
    }
}