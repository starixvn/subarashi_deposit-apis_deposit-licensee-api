namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared.SiteDB;

using DepositLicenseeAPI.Features.Shared.SiteDB;

public class SiteDBConfigTests
{
    [Fact]
    public void SiteDBConfig_DefaultConstructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var config = new SiteDBConfig();

        // Assert
        config.Should().NotBeNull();
        config.SiteDBGroupId.Should().Be(0);
        config.LinkedServerName.Should().BeNull();
        config.ClientAccessName.Should().BeNull();
        config.AGIP.Should().BeNull();
        config.CustDBGroupId.Should().Be(0);
        config.LinkedServerNameDBCust.Should().BeNull();
        config.ClientAccessNameDBCust.Should().BeNull();
        config.AGIP_DBCust.Should().BeNull();
        config.Site.Should().BeNull();
    }

    [Fact]
    public void SiteDBConfig_WithInitializer_SetsPropertiesCorrectly()
    {
        // Arrange
        var siteDBGroupId = 123;
        var linkedServerName = "TestLinkedServer";
        var clientAccessName = "TestClientAccess";
        var agip = "192.168.1.1";
        var custDBGroupId = 456;
        var linkedServerNameDBCust = "TestLinkedServerCust";
        var clientAccessNameDBCust = "TestClientAccessCust";
        var agipDBCust = "192.168.1.2";
        var site = "TestSite";

        // Act
        var config = new SiteDBConfig
        {
            SiteDBGroupId = siteDBGroupId,
            LinkedServerName = linkedServerName,
            ClientAccessName = clientAccessName,
            AGIP = agip,
            CustDBGroupId = custDBGroupId,
            LinkedServerNameDBCust = linkedServerNameDBCust,
            ClientAccessNameDBCust = clientAccessNameDBCust,
            AGIP_DBCust = agipDBCust,
            Site = site
        };

        // Assert
        config.SiteDBGroupId.Should().Be(siteDBGroupId);
        config.LinkedServerName.Should().Be(linkedServerName);
        config.ClientAccessName.Should().Be(clientAccessName);
        config.AGIP.Should().Be(agip);
        config.CustDBGroupId.Should().Be(custDBGroupId);
        config.LinkedServerNameDBCust.Should().Be(linkedServerNameDBCust);
        config.ClientAccessNameDBCust.Should().Be(clientAccessNameDBCust);
        config.AGIP_DBCust.Should().Be(agipDBCust);
        config.Site.Should().Be(site);
    }

    [Theory]
    [InlineData(1, "Server1", "Access1", "10.0.0.1", 2, "ServerCust1", "AccessCust1", "10.0.0.2", "Site1")]
    [InlineData(100, "ServerProd", "AccessProd", "192.168.100.1", 200, "ServerProdCust", "AccessProdCust", "192.168.100.2", "ProdSite")]
    [InlineData(0, null, null, null, 0, null, null, null, null)]
    [InlineData(-1, "", "", "", -2, "", "", "", "")]
    public void SiteDBConfig_WithVariousValues_SetsPropertiesCorrectly(
        int siteDBGroupId,
        string linkedServerName,
        string clientAccessName,
        string agip,
        int custDBGroupId,
        string linkedServerNameDBCust,
        string clientAccessNameDBCust,
        string agipDBCust,
        string site)
    {
        // Arrange & Act
        var config = new SiteDBConfig
        {
            SiteDBGroupId = siteDBGroupId,
            LinkedServerName = linkedServerName,
            ClientAccessName = clientAccessName,
            AGIP = agip,
            CustDBGroupId = custDBGroupId,
            LinkedServerNameDBCust = linkedServerNameDBCust,
            ClientAccessNameDBCust = clientAccessNameDBCust,
            AGIP_DBCust = agipDBCust,
            Site = site
        };

        // Assert
        config.SiteDBGroupId.Should().Be(siteDBGroupId);
        config.LinkedServerName.Should().Be(linkedServerName);
        config.ClientAccessName.Should().Be(clientAccessName);
        config.AGIP.Should().Be(agip);
        config.CustDBGroupId.Should().Be(custDBGroupId);
        config.LinkedServerNameDBCust.Should().Be(linkedServerNameDBCust);
        config.ClientAccessNameDBCust.Should().Be(clientAccessNameDBCust);
        config.AGIP_DBCust.Should().Be(agipDBCust);
        config.Site.Should().Be(site);
    }

    [Fact]
    public void SiteDBConfig_IsRecord_SupportsValueEquality()
    {
        // Arrange
        var config1 = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "TestServer",
            ClientAccessName = "TestAccess",
            AGIP = "192.168.1.1",
            CustDBGroupId = 456,
            LinkedServerNameDBCust = "TestServerCust",
            ClientAccessNameDBCust = "TestAccessCust",
            AGIP_DBCust = "192.168.1.2",
            Site = "TestSite"
        };

        var config2 = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "TestServer",
            ClientAccessName = "TestAccess",
            AGIP = "192.168.1.1",
            CustDBGroupId = 456,
            LinkedServerNameDBCust = "TestServerCust",
            ClientAccessNameDBCust = "TestAccessCust",
            AGIP_DBCust = "192.168.1.2",
            Site = "TestSite"
        };

        // Act & Assert
        config1.Should().Be(config2);
        config1.Equals(config2).Should().BeTrue();
        (config1 == config2).Should().BeTrue();
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }

    [Fact]
    public void SiteDBConfig_DifferentValues_AreNotEqual()
    {
        // Arrange
        var config1 = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "TestServer",
            Site = "TestSite"
        };

        var config2 = new SiteDBConfig
        {
            SiteDBGroupId = 124, // Different SiteDBGroupId
            LinkedServerName = "TestServer",
            Site = "TestSite"
        };

        // Act & Assert
        config1.Should().NotBe(config2);
        config1.Equals(config2).Should().BeFalse();
        (config1 == config2).Should().BeFalse();
    }

    [Fact]
    public void SiteDBConfig_WithOperator_CreatesNewInstanceWithModifications()
    {
        // Arrange
        var originalConfig = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "OriginalServer",
            ClientAccessName = "OriginalAccess",
            Site = "OriginalSite"
        };

        // Act
        var modifiedConfig = originalConfig with { LinkedServerName = "ModifiedServer" };

        // Assert
        modifiedConfig.Should().NotBeSameAs(originalConfig);
        modifiedConfig.SiteDBGroupId.Should().Be(originalConfig.SiteDBGroupId);
        modifiedConfig.LinkedServerName.Should().Be("ModifiedServer");
        modifiedConfig.ClientAccessName.Should().Be(originalConfig.ClientAccessName);
        modifiedConfig.Site.Should().Be(originalConfig.Site);

        // Original should remain unchanged
        originalConfig.LinkedServerName.Should().Be("OriginalServer");
    }

    [Fact]
    public void SiteDBConfig_PropertiesAreReadable()
    {
        // Arrange
        var config = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "TestServer",
            Site = "TestSite"
        };

        // Act & Assert
        var properties = typeof(SiteDBConfig).GetProperties();

        foreach (var property in properties)
        {
            property.CanRead.Should().BeTrue($"Property {property.Name} should be readable");
            property.Should().NotBeNull($"Property {property.Name} should exist");
        }

        properties.Should().HaveCount(9, "SiteDBConfig should have 9 properties");
    }

    [Fact]
    public void SiteDBConfig_ToString_ReturnsFormattedString()
    {
        // Arrange
        var config = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "TestServer",
            ClientAccessName = "TestAccess",
            AGIP = "192.168.1.1",
            CustDBGroupId = 456,
            Site = "TestSite"
        };

        // Act
        var result = config.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("123");
        result.Should().Contain("TestServer");
        result.Should().Contain("TestAccess");
        result.Should().Contain("192.168.1.1");
        result.Should().Contain("456");
        result.Should().Contain("TestSite");
    }

    [Fact]
    public void SiteDBConfig_WithNullStrings_HandlesCorrectly()
    {
        // Arrange & Act
        var config = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = null,
            ClientAccessName = null,
            AGIP = null,
            LinkedServerNameDBCust = null,
            ClientAccessNameDBCust = null,
            AGIP_DBCust = null,
            Site = null
        };

        // Assert
        config.LinkedServerName.Should().BeNull();
        config.ClientAccessName.Should().BeNull();
        config.AGIP.Should().BeNull();
        config.LinkedServerNameDBCust.Should().BeNull();
        config.ClientAccessNameDBCust.Should().BeNull();
        config.AGIP_DBCust.Should().BeNull();
        config.Site.Should().BeNull();
        config.SiteDBGroupId.Should().Be(123);
    }

    [Fact]
    public void SiteDBConfig_WithEmptyStrings_HandlesCorrectly()
    {
        // Arrange & Act
        var config = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "",
            ClientAccessName = "",
            AGIP = "",
            LinkedServerNameDBCust = "",
            ClientAccessNameDBCust = "",
            AGIP_DBCust = "",
            Site = ""
        };

        // Assert
        config.LinkedServerName.Should().Be("");
        config.ClientAccessName.Should().Be("");
        config.AGIP.Should().Be("");
        config.LinkedServerNameDBCust.Should().Be("");
        config.ClientAccessNameDBCust.Should().Be("");
        config.AGIP_DBCust.Should().Be("");
        config.Site.Should().Be("");
    }

    [Fact]
    public void SiteDBConfig_SupportsRecordSemantics()
    {
        // Arrange
        var config = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "TestServer",
            ClientAccessName = "TestAccess",
            AGIP = "192.168.1.1",
            CustDBGroupId = 456,
            LinkedServerNameDBCust = "TestServerCust",
            ClientAccessNameDBCust = "TestAccessCust",
            AGIP_DBCust = "192.168.1.2",
            Site = "TestSite"
        };

        // Act & Assert
        config.SiteDBGroupId.Should().Be(123);
        config.LinkedServerName.Should().Be("TestServer");
        config.ClientAccessName.Should().Be("TestAccess");
        config.AGIP.Should().Be("192.168.1.1");
        config.CustDBGroupId.Should().Be(456);
        config.LinkedServerNameDBCust.Should().Be("TestServerCust");
        config.ClientAccessNameDBCust.Should().Be("TestAccessCust");
        config.AGIP_DBCust.Should().Be("192.168.1.2");
        config.Site.Should().Be("TestSite");
    }

    [Fact]
    public void SiteDBConfig_CopyConstructor_CreatesExactCopy()
    {
        // Arrange
        var originalConfig = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "TestServer",
            ClientAccessName = "TestAccess",
            AGIP = "192.168.1.1",
            CustDBGroupId = 456,
            LinkedServerNameDBCust = "TestServerCust",
            ClientAccessNameDBCust = "TestAccessCust",
            AGIP_DBCust = "192.168.1.2",
            Site = "TestSite"
        };

        // Act
        var copiedConfig = originalConfig with { };

        // Assert
        copiedConfig.Should().NotBeSameAs(originalConfig);
        copiedConfig.Should().Be(originalConfig);
        copiedConfig.SiteDBGroupId.Should().Be(originalConfig.SiteDBGroupId);
        copiedConfig.LinkedServerName.Should().Be(originalConfig.LinkedServerName);
        copiedConfig.ClientAccessName.Should().Be(originalConfig.ClientAccessName);
        copiedConfig.AGIP.Should().Be(originalConfig.AGIP);
        copiedConfig.CustDBGroupId.Should().Be(originalConfig.CustDBGroupId);
        copiedConfig.LinkedServerNameDBCust.Should().Be(originalConfig.LinkedServerNameDBCust);
        copiedConfig.ClientAccessNameDBCust.Should().Be(originalConfig.ClientAccessNameDBCust);
        copiedConfig.AGIP_DBCust.Should().Be(originalConfig.AGIP_DBCust);
        copiedConfig.Site.Should().Be(originalConfig.Site);
    }

    [Fact]
    public void SiteDBConfig_MultiplePropertiesModification_WorksCorrectly()
    {
        // Arrange
        var originalConfig = new SiteDBConfig
        {
            SiteDBGroupId = 123,
            LinkedServerName = "OldServer",
            Site = "OldSite"
        };

        // Act
        var modifiedConfig = originalConfig with
        {
            LinkedServerName = "NewServer",
            Site = "NewSite",
            CustDBGroupId = 999
        };

        // Assert
        modifiedConfig.SiteDBGroupId.Should().Be(123); // Unchanged
        modifiedConfig.LinkedServerName.Should().Be("NewServer"); // Modified
        modifiedConfig.Site.Should().Be("NewSite"); // Modified
        modifiedConfig.CustDBGroupId.Should().Be(999); // Modified

        // Original unchanged
        originalConfig.LinkedServerName.Should().Be("OldServer");
        originalConfig.Site.Should().Be("OldSite");
        originalConfig.CustDBGroupId.Should().Be(0);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void SiteDBConfig_WithVariousIntegerValues_HandlesCorrectly(int value)
    {
        // Arrange & Act
        var config = new SiteDBConfig
        {
            SiteDBGroupId = value,
            CustDBGroupId = value
        };

        // Assert
        config.SiteDBGroupId.Should().Be(value);
        config.CustDBGroupId.Should().Be(value);
    }
}