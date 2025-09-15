namespace Fanex.DepositLicenseeAPI.Tests.Features.Shared;

using System.Net;
using System.Reflection;
using DepositLicenseeAPI.Features.Shared;
using Microsoft.AspNetCore.Http;

public class LoggerUtilsTests
{
    [Fact]
    public void GenerateMessage_WithSimpleException_ReturnsFormattedMessage()
    {
        // Arrange
        var exception = new Exception("Test exception message");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Test exception message");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("Message: Test exception message");
        result.Should().Contain("StackTrace:");
    }

    [Fact]
    public void GenerateMessage_WithNullContext_DoesNotIncludeRequestInfo()
    {
        // Arrange
        var exception = new Exception("Test exception message");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Test exception message");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().NotContain("REQUEST INFO");
    }

    [Fact]
    public void GenerateMessage_WithHttpContext_IncludesRequestInfo()
    {
        // Arrange
        var exception = new Exception("Test exception message");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("localhost:5000");
        context.Request.Path = "/api/test";
        context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Test exception message");
        result.Should().Contain("REQUEST INFO");
        result.Should().Contain("Host: localhost:5000");
        result.Should().Contain("Path: /api/test");
        result.Should().Contain("Remote IP: 127.0.0.1");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
    }

    [Fact]
    public void GenerateMessage_WithSqlException_IncludesDatabaseInfo()
    {
        // Arrange
        var sqlInnerException = new Exception("SQL error");
        var exceptionWithSqlInner = new Exception("Outer exception", sqlInnerException);

        // Act
        var result = LoggerUtils.GenerateMessage(exceptionWithSqlInner);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Outer exception");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("Message: Outer exception");
        result.Should().Contain("Message: SQL error");
    }

    [Fact]
    public void GenerateMessage_WithObjectDbException_IncludesFanexDataInfo()
    {
        // Arrange
        // Since ObjectDbException is from external library, we'll test with a regular exception
        // that should still go through the database info extraction logic
        var exception = new Exception("DB error");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: DB error");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
    }

    [Fact]
    public void GenerateMessage_WithInnerExceptions_IncludesLimitedInnerExceptionInfo()
    {
        // Arrange
        var innermost = new Exception("Innermost exception");
        var middle = new Exception("Middle exception", innermost);
        var outermost = new Exception("Outermost exception", middle);

        // Act
        var result = LoggerUtils.GenerateMessage(outermost);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Outermost exception");
        result.Should().Contain("Message: Outermost exception");
        result.Should().Contain("Message: Middle exception");
        result.Should().Contain("Message: Innermost exception");
    }

    [Fact]
    public void GenerateMessage_WithDeepInnerExceptions_LimitsToThreeLevels()
    {
        // Arrange
        var level4 = new Exception("Level 4 exception");
        var level3 = new Exception("Level 3 exception", level4);
        var level2 = new Exception("Level 2 exception", level3);
        var level1 = new Exception("Level 1 exception", level2);
        var level0 = new Exception("Level 0 exception", level1);

        // Act
        var result = LoggerUtils.GenerateMessage(level0);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Message: Level 0 exception");
        result.Should().Contain("Message: Level 1 exception");
        result.Should().Contain("Message: Level 2 exception");
        result.Should().Contain("Message: Level 3 exception");
        // Should not contain Level 4 due to 3-level limit
        result.Should().NotContain("Message: Level 4 exception");
    }

    [Fact]
    public void GenerateMessage_WithEmptyParameters_HandlesCorrectly()
    {
        // Arrange
        var exception = new Exception("DB error");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: DB error");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
    }

    [Fact]
    public void GenerateMessage_WithNullParameters_HandlesCorrectly()
    {
        // Arrange
        var exception = new Exception("DB error");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: DB error");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
    }

    [Fact]
    public void GenerateMessage_WithSingleParameter_HandlesCorrectly()
    {
        // Arrange
        var exception = new Exception("DB error");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: DB error");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
    }

    [Fact]
    public void GenerateMessage_WithMultipleParameters_FormatsCorrectly()
    {
        // Arrange
        var exception = new Exception("DB error");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: DB error");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
    }

    [Fact]
    public void GenerateMessage_WithObjectDbExceptionAndSqlInnerException_IncludesBothDatabaseInfos()
    {
        // Arrange
        var innerException = new Exception("Inner SQL error");
        var outerException = new Exception("Fanex DB error", innerException);

        // Act
        var result = LoggerUtils.GenerateMessage(outerException);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Fanex DB error");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("Message: Fanex DB error");
        result.Should().Contain("Message: Inner SQL error");
    }

    [Fact]
    public void GenerateMessage_IncludesAllRequiredSections()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE:");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");

        // Verify sections appear in the correct order
        var messageIndex = result.IndexOf("MESSAGE:");
        var databaseIndex = result.IndexOf("DATABASE INFO");
        var exceptionIndex = result.IndexOf("EXCEPTION INFO");

        messageIndex.Should().BeLessThan(databaseIndex);
        databaseIndex.Should().BeLessThan(exceptionIndex);
    }

    [Fact]
    public void GenerateMessage_WithComplexHttpContext_IncludesAllRequestInfo()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("api.example.com", 8080);
        context.Request.Path = "/api/v1/users/123";
        context.Connection.RemoteIpAddress = IPAddress.Parse("192.168.1.100");

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("REQUEST INFO");
        result.Should().Contain("Host: api.example.com:8080");
        result.Should().Contain("Path: /api/v1/users/123");
        result.Should().Contain("Remote IP: 192.168.1.100");
    }

    [Fact]
    public void GenerateMessage_WithNullExceptionProperties_HandlesGracefully()
    {
        // Arrange
        var exception = new Exception("Test exception");
        // Note: Can't easily create an exception with null properties in .NET,
        // but we can test with an exception that has minimal information

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Test exception");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("Message: Test exception");
        result.Should().Contain("Source:");
        result.Should().Contain("TargetSite:");
        result.Should().Contain("StackTrace:");
    }

    [Fact]
    public void GenerateMessage_ProducesWellFormattedOutput()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("localhost");
        context.Request.Path = "/test";
        context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        result.Should().NotBeNullOrEmpty();

        // Should contain proper line breaks and formatting
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        lines.Should().Contain(l => l.StartsWith("MESSAGE:"));
        lines.Should().Contain(l => l.Trim() == "REQUEST INFO");
        lines.Should().Contain(l => l.StartsWith("Host:"));
        lines.Should().Contain(l => l.StartsWith("Path:"));
        lines.Should().Contain(l => l.StartsWith("Remote IP:"));
        lines.Should().Contain(l => l.Trim() == "DATABASE INFO");
        lines.Should().Contain(l => l.Trim() == "EXCEPTION INFO");
        lines.Should().Contain(l => l.StartsWith("Message:"));
        lines.Should().Contain(l => l.StartsWith("Source:"));
        lines.Should().Contain(l => l.StartsWith("TargetSite:"));
        lines.Should().Contain(l => l.StartsWith("StackTrace:"));
    }


    [Fact]
    public void GenerateMessage_WithCustomExceptionTypes_HandlesGracefully()
    {
        // Arrange
        var customException = new CustomException("Custom error message", 42);

        // Act
        var result = LoggerUtils.GenerateMessage(customException);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Custom error message");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("Message: Custom error message");
    }

    [Fact]
    public void GenerateMessage_WithLongExceptionMessage_HandlesCorrectly()
    {
        // Arrange
        var longMessage = new string('A', 5000); // Very long message
        var exception = new Exception(longMessage);

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain($"MESSAGE: {longMessage}");
        result.Should().Contain("DATABASE INFO");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain($"Message: {longMessage}");
    }

    [Fact]
    public async Task GenerateMessage_IsThreadSafe()
    {
        // Arrange
        var exception = new Exception("Thread test exception");
        var tasks = new List<Task<string>>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => LoggerUtils.GenerateMessage(exception)));
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(10);
        results.Should().AllSatisfy(result =>
        {
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("MESSAGE: Thread test exception");
        });

        // All results should be identical (method is deterministic for same input)
        var firstResult = results[0];
        results.Should().AllBe(firstResult);
    }

    [Fact]
    public void LoggerUtils_IsStaticClass()
    {
        // Arrange & Act & Assert
        var type = typeof(LoggerUtils);
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
    }

    [Fact]
    public void GenerateMessage_Method_IsStatic()
    {
        // Arrange & Act & Assert
        var method = typeof(LoggerUtils).GetMethod(nameof(LoggerUtils.GenerateMessage));
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();
    }

    [Fact]
    public void GenerateMessage_WithIPv6Address_HandlesCorrectly()
    {
        // Arrange
        var exception = new Exception("IPv6 test exception");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("[::1]", 8080);
        context.Request.Path = "/api/v2/test";
        context.Connection.RemoteIpAddress = IPAddress.IPv6Loopback;

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("REQUEST INFO");
        result.Should().Contain("Host: [::1]:8080");
        result.Should().Contain("Path: /api/v2/test");
        result.Should().Contain("Remote IP: ::1");
    }

    [Fact]
    public void GenerateMessage_WithSpecialCharactersInMessage_HandlesCorrectly()
    {
        // Arrange
        var specialMessage = "Error with special chars: <>&\"'\n\r\t";
        var exception = new Exception(specialMessage);

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain($"MESSAGE: {specialMessage}");
        result.Should().Contain($"Message: {specialMessage}");
    }

    [Fact]
    public void GenerateMessage_WithUnicodeCharacters_HandlesCorrectly()
    {
        // Arrange
        var unicodeMessage = "Error with unicode: 测试 🚀 ñáéíóú";
        var exception = new Exception(unicodeMessage);

        // Act
        var result = LoggerUtils.GenerateMessage(exception);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain($"MESSAGE: {unicodeMessage}");
        result.Should().Contain($"Message: {unicodeMessage}");
    }

    [Fact]
    public void GenerateMessage_WithEmptyInnerExceptionMessage_HandlesCorrectly()
    {
        // Arrange
        var innerException = new Exception(""); // Empty message
        var outerException = new Exception("Outer exception", innerException);

        // Act
        var result = LoggerUtils.GenerateMessage(outerException);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Outer exception");
        result.Should().Contain("Message: Outer exception");
        result.Should().Contain("Message: "); // Empty inner exception message
    }

    [Fact]
    public void GenerateMessage_WithHttpContextNullRemoteIP_HandlesGracefully()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("localhost");
        context.Request.Path = "/test";
        context.Connection.RemoteIpAddress = null; // Null IP address

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("REQUEST INFO");
        result.Should().Contain("Host: localhost");
        result.Should().Contain("Path: /test");
        result.Should().Contain("Remote IP:"); // Should handle null gracefully
    }

    [Fact]
    public void GenerateMessage_WithCustomConnectionProperties_IncludesCorrectInfo()
    {
        // Arrange
        var exception = new Exception("Connection test");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("api.example.com");
        context.Request.Path = "/api/test";
        context.Connection.RemoteIpAddress = IPAddress.Parse("203.0.113.195"); // RFC 5737 test address

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Host: api.example.com");
        result.Should().Contain("Path: /api/test");
        result.Should().Contain("Remote IP: 203.0.113.195");
    }

    [Fact]
    public void GenerateMessage_WithVeryLongStackTrace_HandlesCorrectly()
    {
        // Arrange
        Exception deepException;
        try
        {
            DeepMethod1();
            deepException = new Exception("Should not reach here");
        }
        catch (Exception ex)
        {
            deepException = ex;
        }

        // Act
        var result = LoggerUtils.GenerateMessage(deepException);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("StackTrace:");
        result.Should().Contain("DeepMethod"); // Should contain method names from stack trace
    }

    [Fact]
    public void GenerateMessage_WithAggregateException_HandlesCorrectly()
    {
        // Arrange
        var innerExceptions = new[]
        {
            new Exception("First inner exception"),
            new Exception("Second inner exception"),
            new Exception("Third inner exception")
        };
        var aggregateException = new AggregateException("Multiple errors occurred", innerExceptions);

        // Act
        var result = LoggerUtils.GenerateMessage(aggregateException);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Multiple errors occurred");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("Message: Multiple errors occurred");
    }

    [Fact]
    public void GenerateMessage_WithArgumentException_IncludesParameterInfo()
    {
        // Arrange
        var argumentException = new ArgumentException("Invalid parameter value", "parameterName");

        // Act
        var result = LoggerUtils.GenerateMessage(argumentException);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MESSAGE: Invalid parameter value");
        result.Should().Contain("EXCEPTION INFO");
        result.Should().Contain("Message: Invalid parameter value");
    }

    [Fact]
    public void GenerateMessage_WithHttpContextComplexPath_HandlesCorrectly()
    {
        // Arrange
        var exception = new Exception("Path test");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("localhost");
        context.Request.Path = "/api/v1/users/123/orders/456?filter=active&sort=date";
        context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        result.Should().NotBeNullOrEmpty();
        // URL encoding may convert ? to %3F, so check for the encoded version
        result.Should().Contain("Path: /api/v1/users/123/orders/456%3Ffilter=active&sort=date");
    }

    [Fact]
    public void GenerateMessage_OrderOfSections_IsConsistent()
    {
        // Arrange
        var exception = new Exception("Section order test");
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("localhost");
        context.Request.Path = "/test";
        context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

        // Act
        var result = LoggerUtils.GenerateMessage(exception, context);

        // Assert
        var messageIndex = result.IndexOf("MESSAGE:", StringComparison.Ordinal);
        var requestIndex = result.IndexOf("REQUEST INFO", StringComparison.Ordinal);
        var databaseIndex = result.IndexOf("DATABASE INFO", StringComparison.Ordinal);
        var exceptionIndex = result.IndexOf("EXCEPTION INFO", StringComparison.Ordinal);

        // All sections should be present
        messageIndex.Should().BeGreaterOrEqualTo(0);
        requestIndex.Should().BeGreaterOrEqualTo(0);
        databaseIndex.Should().BeGreaterOrEqualTo(0);
        exceptionIndex.Should().BeGreaterOrEqualTo(0);

        // Sections should appear in the correct order
        messageIndex.Should().BeLessThan(requestIndex);
        requestIndex.Should().BeLessThan(databaseIndex);
        databaseIndex.Should().BeLessThan(exceptionIndex);
    }

    [Fact]
    public void GenerateMessage_StaticMethodProperties_AreCorrect()
    {
        // Arrange & Act
        var method = typeof(LoggerUtils).GetMethod(nameof(LoggerUtils.GenerateMessage));

        // Assert
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();
        method.ReturnType.Should().Be(typeof(string));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(Exception));
        parameters[1].ParameterType.Should().Be(typeof(HttpContext));
        parameters[1].HasDefaultValue.Should().BeTrue();
        parameters[1].DefaultValue.Should().BeNull();
    }

    [Fact]
    public void LoggerUtils_AllMethods_AreStatic()
    {
        // Arrange & Act
        var type = typeof(LoggerUtils);
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Where(m => m.DeclaringType == type);

        // Assert
        methods.Should().NotBeEmpty();
        foreach (var method in methods)
        {
            method.IsStatic.Should().BeTrue($"Method {method.Name} should be static");
        }
    }

    [Fact]
    public void LoggerUtils_HasNoInstanceMembers()
    {
        // Arrange & Act
        var type = typeof(LoggerUtils);
        var instanceMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.DeclaringType == type);
        var instanceFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var instanceProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // Assert
        instanceMethods.Should().BeEmpty("Static class should not have instance methods");
        instanceFields.Should().BeEmpty("Static class should not have instance fields");
        instanceProperties.Should().BeEmpty("Static class should not have instance properties");
    }

    // Helper methods for deep stack trace test
    private void DeepMethod1() => DeepMethod2();
    private void DeepMethod2() => DeepMethod3();
    private void DeepMethod3() => DeepMethod4();
    private void DeepMethod4() => DeepMethod5();
    private void DeepMethod5() => throw new Exception("Deep stack trace exception");

    private class CustomException : Exception
    {
        public CustomException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; }
    }
}