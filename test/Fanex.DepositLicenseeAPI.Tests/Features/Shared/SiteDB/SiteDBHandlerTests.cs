namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared.SiteDB;

using Data.Repository;
using DepositLicenseeAPI.Features.Shared.SiteDB;

public class SiteDBHandlerTests
{
    private readonly SiteDBHandler _handler;
    private readonly Mock<IDynamicRepository> _mockRepository;

    public SiteDBHandlerTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _handler = new SiteDBHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task GetSiteId_WithValidSiteName_ReturnsSiteId()
    {
        // Arrange
        var siteName = "TestSite";
        var expectedSiteId = "SITE123";

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetSiteIdCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is GetSiteIdCommand getSiteCmd)
                {
                    getSiteCmd.SiteId = expectedSiteId;
                }
            });

        // Act
        var result = await _handler.GetSiteId(siteName);

        // Assert
        result.Should().Be(expectedSiteId);
        _mockRepository.Verify(x => x.ExecuteAsync(
            It.Is<GetSiteIdCommand>(cmd => cmd.SiteName == siteName),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSiteId_CallsRepositoryWithCorrectCommand()
    {
        // Arrange
        var siteName = "TestSite";

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetSiteIdCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        // Act
        await _handler.GetSiteId(siteName);

        // Assert
        _mockRepository.Verify(x => x.ExecuteAsync(
            It.Is<GetSiteIdCommand>(cmd => cmd.SiteName == siteName),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBySiteDBGroupId_CallsRepositoryWithCorrectCriteria()
    {
        // Arrange
        var siteDBGroupId = 123;
        var expectedConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1 };

        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetCustomerSiteDBCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _handler.GetBySiteDBGroupId(siteDBGroupId);

        // Assert
        result.Should().Be(expectedConfig);
        _mockRepository.Verify(x => x.GetAsync<SiteDBConfig>(
            It.Is<GetCustomerSiteDBCriteria>(criteria => criteria.SiteDBGroupId == siteDBGroupId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBySite_CallsRepositoryWithCorrectCriteria()
    {
        // Arrange
        var site = "TestSite";
        var expectedConfig = new SiteDBConfig { Site = site, CustDBGroupId = 1 };

        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _handler.GetBySite(site);

        // Assert
        result.Should().Be(expectedConfig);
        _mockRepository.Verify(x => x.GetAsync<SiteDBConfig>(
            It.Is<GetSiteDBConfigCriteria>(criteria => criteria.Site == site),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByCustId_CallsRepositoryWithCorrectCriteria()
    {
        // Arrange
        var custId = 456;
        var expectedConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1 };

        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigByCustCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _handler.GetByCustId(custId);

        // Assert
        result.Should().Be(expectedConfig);
        _mockRepository.Verify(x => x.GetAsync<SiteDBConfig>(
            It.Is<GetSiteDBConfigByCustCriteria>(criteria => criteria.CustId == custId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSiteId_WithNullSiteId_ReturnsNull()
    {
        // Arrange
        var siteName = "TestSite";

        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetSiteIdCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<INonQueryCommand, string, CancellationToken>((cmd, connStr, ct) =>
            {
                if (cmd is GetSiteIdCommand getSiteCmd)
                {
                    getSiteCmd.SiteId = null;
                }
            });

        // Act
        var result = await _handler.GetSiteId(siteName);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("Site1")]
    [InlineData("Site2")]
    [InlineData("LongSiteName123")]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetSiteId_WithVariousSiteNames_CallsRepository(string siteName)
    {
        // Arrange
        _mockRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetSiteIdCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        // Act
        await _handler.GetSiteId(siteName);

        // Assert
        _mockRepository.Verify(x => x.ExecuteAsync(
            It.Is<GetSiteIdCommand>(cmd => cmd.SiteName == siteName),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetBySiteDBGroupId_WithVariousIds_CallsRepository(int siteDBGroupId)
    {
        // Arrange
        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetCustomerSiteDBCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        await _handler.GetBySiteDBGroupId(siteDBGroupId);

        // Assert
        _mockRepository.Verify(x => x.GetAsync<SiteDBConfig>(
            It.Is<GetCustomerSiteDBCriteria>(criteria => criteria.SiteDBGroupId == siteDBGroupId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByCustId_WithVariousIds_CallsRepository(int custId)
    {
        // Arrange
        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigByCustCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SiteDBConfig)null);

        // Act
        await _handler.GetByCustId(custId);

        // Assert
        _mockRepository.Verify(x => x.GetAsync<SiteDBConfig>(
            It.Is<GetSiteDBConfigByCustCriteria>(criteria => criteria.CustId == custId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handler_ImplementsISiteDBHandler()
    {
        // Arrange & Act & Assert
        _handler.Should().BeAssignableTo<ISiteDBHandler>();
    }

    [Fact]
    public async Task GetBySiteDBGroupId_ReturnsRepositoryResult()
    {
        // Arrange
        var siteDBGroupId = 123;
        var expectedConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };

        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetCustomerSiteDBCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _handler.GetBySiteDBGroupId(siteDBGroupId);

        // Assert
        result.Should().BeSameAs(expectedConfig);
    }

    [Fact]
    public async Task GetBySite_ReturnsRepositoryResult()
    {
        // Arrange
        var site = "TestSite";
        var expectedConfig = new SiteDBConfig { Site = site, CustDBGroupId = 1, SiteDBGroupId = 2 };

        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _handler.GetBySite(site);

        // Assert
        result.Should().BeSameAs(expectedConfig);
    }

    [Fact]
    public async Task GetByCustId_ReturnsRepositoryResult()
    {
        // Arrange
        var custId = 456;
        var expectedConfig = new SiteDBConfig { Site = "TestSite", CustDBGroupId = 1, SiteDBGroupId = 2 };

        _mockRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigByCustCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _handler.GetByCustId(custId);

        // Assert
        result.Should().BeSameAs(expectedConfig);
    }
}