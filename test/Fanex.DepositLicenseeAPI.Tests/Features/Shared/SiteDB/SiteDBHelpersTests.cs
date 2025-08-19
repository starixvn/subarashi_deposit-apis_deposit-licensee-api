namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared.SiteDB;

using DepositLicenseeAPI.Features.Shared.SiteDB;

public class SiteDBHelpersTests
{
    [Fact]
    public void GetSPNameInSiteDB_WithZeroSiteDBGroupId_ReturnsOriginalSPName()
    {
        // Arrange
        var siteDBGroupId = 0;
        var spName = "TestStoredProcedure";

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be(spName);
    }

    [Fact]
    public void GetSPNameInSiteDB_WithPositiveSiteDBGroupId_ReturnsPrefixedSPName()
    {
        // Arrange
        var siteDBGroupId = 123;
        var spName = "TestStoredProcedure";

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be($"SiteDB{siteDBGroupId}_{spName}");
        result.Should().Be("SiteDB123_TestStoredProcedure");
    }

    [Theory]
    [InlineData(0, "SP_Test", "SP_Test")]
    [InlineData(1, "SP_Test", "SiteDB1_SP_Test")]
    [InlineData(100, "API_SomeProcess", "SiteDB100_API_SomeProcess")]
    [InlineData(999, "Proc_Name", "SiteDB999_Proc_Name")]
    [InlineData(0, "", "")]
    [InlineData(5, "", "SiteDB5_")]
    public void GetSPNameInSiteDB_WithVariousInputs_ReturnsExpectedResults(int siteDBGroupId, string spName, string expected)
    {
        // Arrange & Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetSPNameInSiteDB_WithNullSPName_HandlesCorrectly()
    {
        // Arrange
        var siteDBGroupId = 123;
        string spName = null;

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be($"SiteDB{siteDBGroupId}_");
        result.Should().Be("SiteDB123_");
    }

    [Fact]
    public void GetSPNameInSiteDB_WithNullSPNameAndZeroGroupId_ReturnsNull()
    {
        // Arrange
        var siteDBGroupId = 0;
        string spName = null;

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(-1, "SP_Test")]
    [InlineData(-100, "API_Process")]
    [InlineData(int.MinValue, "Test_SP")]
    public void GetSPNameInSiteDB_WithNegativeSiteDBGroupId_ReturnsPrefixedSPName(int siteDBGroupId, string spName)
    {
        // Arrange & Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be($"SiteDB{siteDBGroupId}_{spName}");
        result.Should().StartWith($"SiteDB{siteDBGroupId}_");
        result.Should().EndWith(spName);
    }

    [Theory]
    [InlineData(int.MaxValue, "SP_Test")]
    [InlineData(999999, "API_Process")]
    public void GetSPNameInSiteDB_WithLargeSiteDBGroupId_ReturnsPrefixedSPName(int siteDBGroupId, string spName)
    {
        // Arrange & Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be($"SiteDB{siteDBGroupId}_{spName}");
    }

    [Fact]
    public void GetSPNameInSiteDB_WithEmptyString_HandlesCorrectly()
    {
        // Arrange
        var siteDBGroupId = 123;
        var spName = "";

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be("SiteDB123_");
    }

    [Fact]
    public void GetSPNameInSiteDB_WithWhitespaceString_PreservesWhitespace()
    {
        // Arrange
        var siteDBGroupId = 123;
        var spName = "   ";

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be("SiteDB123_   ");
    }

    [Fact]
    public void GetSPNameInSiteDB_WithSpecialCharacters_PreservesCharacters()
    {
        // Arrange
        var siteDBGroupId = 123;
        var spName = "SP_Test@#$%^&*()";

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be("SiteDB123_SP_Test@#$%^&*()");
    }

    [Fact]
    public void GetSPNameInSiteDB_WithLongSPName_HandlesCorrectly()
    {
        // Arrange
        var siteDBGroupId = 123;
        var spName = new string('A', 1000); // Very long string

        // Act
        var result = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result.Should().Be($"SiteDB123_{spName}");
        result.Should().HaveLength(1000 + "SiteDB123_".Length);
    }

    [Fact]
    public void GetSPNameInSiteDB_IsPureFunctionForZeroGroupId()
    {
        // Arrange
        var siteDBGroupId = 0;
        var spName = "TestSP";

        // Act
        var result1 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);
        var result2 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);
        var result3 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result1.Should().Be(result2);
        result2.Should().Be(result3);
        result1.Should().Be(spName);
    }

    [Fact]
    public void GetSPNameInSiteDB_IsPureFunctionForNonZeroGroupId()
    {
        // Arrange
        var siteDBGroupId = 123;
        var spName = "TestSP";

        // Act
        var result1 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);
        var result2 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);
        var result3 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);

        // Assert
        result1.Should().Be(result2);
        result2.Should().Be(result3);
        result1.Should().Be("SiteDB123_TestSP");
    }

    [Fact]
    public void GetSPNameInSiteDB_DifferentGroupIdsSameSpName_ReturnsDifferentResults()
    {
        // Arrange
        var spName = "TestSP";

        // Act
        var result1 = SiteDBHelpers.GetSPNameInSiteDB(1, spName);
        var result2 = SiteDBHelpers.GetSPNameInSiteDB(2, spName);
        var result3 = SiteDBHelpers.GetSPNameInSiteDB(0, spName);

        // Assert
        result1.Should().Be("SiteDB1_TestSP");
        result2.Should().Be("SiteDB2_TestSP");
        result3.Should().Be("TestSP");

        result1.Should().NotBe(result2);
        result1.Should().NotBe(result3);
        result2.Should().NotBe(result3);
    }

    [Fact]
    public void GetSPNameInSiteDB_SameGroupIdDifferentSpNames_ReturnsDifferentResults()
    {
        // Arrange
        var siteDBGroupId = 123;

        // Act
        var result1 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, "SP1");
        var result2 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, "SP2");
        var result3 = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, "");

        // Assert
        result1.Should().Be("SiteDB123_SP1");
        result2.Should().Be("SiteDB123_SP2");
        result3.Should().Be("SiteDB123_");

        result1.Should().NotBe(result2);
        result1.Should().NotBe(result3);
        result2.Should().NotBe(result3);
    }

    [Fact]
    public void GetSPNameInSiteDB_LogicMatchesTernaryExpression()
    {
        // Arrange
        var testCases = new[]
        {
            (0, "SP1"),
            (1, "SP2"),
            (123, "SP3"),
            (-1, "SP4"),
            (0, null),
            (5, "")
        };

        // Act & Assert
        foreach (var (siteDBGroupId, spName) in testCases)
        {
            var actualResult = SiteDBHelpers.GetSPNameInSiteDB(siteDBGroupId, spName);
            var expectedResult = siteDBGroupId == 0 ? spName : $"SiteDB{siteDBGroupId}_{spName}";

            actualResult.Should().Be(expectedResult,
                $"For siteDBGroupId={siteDBGroupId} and spName='{spName}'");
        }
    }

    [Fact]
    public void GetSPNameInSiteDB_ReflectsExactBehaviorFromSourceCode()
    {
        // Arrange & Act & Assert
        // Test the exact ternary condition: siteDBGroupId == 0
        SiteDBHelpers.GetSPNameInSiteDB(0, "test").Should().Be("test");
        SiteDBHelpers.GetSPNameInSiteDB(1, "test").Should().Be("SiteDB1_test");
        SiteDBHelpers.GetSPNameInSiteDB(-1, "test").Should().Be("SiteDB-1_test");

        // Edge cases
        SiteDBHelpers.GetSPNameInSiteDB(0, null).Should().BeNull();
        SiteDBHelpers.GetSPNameInSiteDB(1, null).Should().Be("SiteDB1_");
        SiteDBHelpers.GetSPNameInSiteDB(0, "").Should().Be("");
        SiteDBHelpers.GetSPNameInSiteDB(1, "").Should().Be("SiteDB1_");
    }

    [Fact]
    public void GetSPNameInSiteDB_IsStaticMethod()
    {
        // Arrange & Act & Assert
        var method = typeof(SiteDBHelpers).GetMethod(nameof(SiteDBHelpers.GetSPNameInSiteDB));
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();
    }

    [Fact]
    public void SiteDBHelpers_IsStaticClass()
    {
        // Arrange & Act & Assert
        var type = typeof(SiteDBHelpers);
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
    }
}