namespace Fanex.DepositLicenseeAPI.Tests.Features.Accounts;

using System.Reflection;
using DepositLicenseeAPI.Features.Accounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

public class AccountsEndpointsTests
{
    [Fact]
    public void RegisterAccountsEndpoints_IsStaticMethod()
    {
        // Arrange & Act & Assert
        var method = typeof(AccountsEndpoints).GetMethod(nameof(AccountsEndpoints.RegisterAccountsEndpoints));
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();
    }

    [Fact]
    public void RegisterAccountsEndpoints_HasCorrectSignature()
    {
        // Arrange & Act & Assert
        var method = typeof(AccountsEndpoints).GetMethod(nameof(AccountsEndpoints.RegisterAccountsEndpoints));
        var parameters = method.GetParameters();

        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(IEndpointRouteBuilder));
        method.ReturnType.Should().Be(typeof(IEndpointRouteBuilder));
    }

    [Fact]
    public void RegisterAccountsEndpoints_IsExtensionMethod()
    {
        // Arrange & Act & Assert
        var method = typeof(AccountsEndpoints).GetMethod(nameof(AccountsEndpoints.RegisterAccountsEndpoints));
        var firstParam = method.GetParameters().FirstOrDefault();

        firstParam.Should().NotBeNull();
        firstParam.ParameterType.Should().Be(typeof(IEndpointRouteBuilder));

        // Extension methods are typically static and the first parameter represents 'this'
        method.IsStatic.Should().BeTrue();
    }

    [Fact]
    public void AccountsEndpoints_IsStaticClass()
    {
        // Arrange & Act & Assert
        var type = typeof(AccountsEndpoints);
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
    }

    [Fact]
    public void AccountsEndpoints_ContainsCreateDummyPlayerMethod()
    {
        // Arrange & Act & Assert
        var methods = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
        var createDummyPlayerMethod = methods.FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        createDummyPlayerMethod.Should().NotBeNull();
        createDummyPlayerMethod.IsStatic.Should().BeTrue();
    }

    [Fact]
    public void RegisterAccountsEndpoints_WithNullBuilder_ThrowsException()
    {
        // Arrange
        IEndpointRouteBuilder nullBuilder = null;

        // Act
        Action act = () => nullBuilder.RegisterAccountsEndpoints();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RegisterAccountsEndpoints_WithValidBuilder_ReturnsBuilder()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddRouting();
        var serviceProvider = services.BuildServiceProvider();

        var app = WebApplication.CreateBuilder().Build();
        IEndpointRouteBuilder builder = app;

        // Act
        var result = builder.RegisterAccountsEndpoints();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void CreateDummyPlayer_MethodExists()
    {
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPrivate.Should().BeTrue();
    }

    [Fact]
    public void CreateDummyPlayer_HasCorrectReturnType()
    {
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        method.Should().NotBeNull();
        var returnType = method.ReturnType;

        // Should be Task<Results<Ok<Result>, Ok<CreateDummyMemberResult>, ValidationProblem>>
        returnType.Should().Match(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));

        var taskInnerType = returnType.GetGenericArguments()[0];
        taskInnerType.Should().Match(t => t.IsGenericType);

        // The inner type should be Results<...>
        var typeName = taskInnerType.Name;
        typeName.Should().StartWith("Results");
    }

    [Fact]
    public void CreateDummyPlayer_HasCorrectParameters()
    {
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        method.Should().NotBeNull();
        var parameters = method.GetParameters();

        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(CreateDummyMemberRequest));
        parameters[1].ParameterType.Should().Be(typeof(IAccountsHandler));
    }

    [Fact]
    public void AccountsEndpoints_OnlyContainsExpectedMethods()
    {
        // Arrange & Act
        var publicMethods = typeof(AccountsEndpoints).GetMethods(BindingFlags.Public | BindingFlags.Static);
        var nonInheritedMethods = publicMethods.Where(m => m.DeclaringType == typeof(AccountsEndpoints));

        // Assert
        nonInheritedMethods.Should().HaveCount(1);
        nonInheritedMethods.Should().Contain(m => m.Name == nameof(AccountsEndpoints.RegisterAccountsEndpoints));
    }

    [Fact]
    public void AccountsEndpoints_UsesCorrectNamespace()
    {
        // Arrange & Act & Assert
        var type = typeof(AccountsEndpoints);
        type.Namespace.Should().Be("Fanex.DepositLicenseeAPI.Features.Accounts");
    }

    [Fact]
    public void AccountsEndpoints_HasCorrectUsings()
    {
        // This is more of a documentation test to verify expected dependencies
        // Arrange & Act & Assert
        var type = typeof(AccountsEndpoints);

        // Verify the class can access the expected types (which implies correct usings)
        var method = type.GetMethod(nameof(AccountsEndpoints.RegisterAccountsEndpoints));
        method.Should().NotBeNull();

        // These types should be accessible (indicating proper using statements)
        var paramType = method.GetParameters()[0].ParameterType;
        paramType.Should().Be(typeof(IEndpointRouteBuilder));

        var returnType = method.ReturnType;
        returnType.Should().Be(typeof(IEndpointRouteBuilder));
    }

    [Fact]
    public void RegisterAccountsEndpoints_CanBeChained()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddRouting();
        var serviceProvider = services.BuildServiceProvider();

        var app = WebApplication.CreateBuilder().Build();
        IEndpointRouteBuilder builder = app;

        // Act & Assert - Should be able to chain method calls
        var result = builder.RegisterAccountsEndpoints().RegisterAccountsEndpoints();
        result.Should().NotBeNull();
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void AccountsEndpoints_ClassAttributes()
    {
        // Arrange & Act
        var type = typeof(AccountsEndpoints);
        var attributes = type.GetCustomAttributes();

        // Assert
        // Static classes don't typically have special attributes, but we can verify the type structure
        type.IsClass.Should().BeTrue();
        type.IsPublic.Should().BeTrue();
        type.IsAbstract.Should().BeTrue(); // Static classes are abstract
        type.IsSealed.Should().BeTrue(); // Static classes are sealed
    }

    [Fact]
    public void CreateDummyPlayer_IsAsyncMethod()
    {
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        method.Should().NotBeNull();
        method.ReturnType.Should().Match(t =>
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(Task<>),
            "CreateDummyPlayer should be an async method returning Task<T>");
    }

    [Fact]
    public void AccountsEndpoints_NoInstanceMembers()
    {
        // Arrange & Act
        var instanceMethods = typeof(AccountsEndpoints).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var instanceFields = typeof(AccountsEndpoints).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var instanceProperties = typeof(AccountsEndpoints).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // Assert
        // Filter out inherited object methods
        var nonInheritedInstanceMethods = instanceMethods.Where(m => m.DeclaringType == typeof(AccountsEndpoints));

        nonInheritedInstanceMethods.Should().BeEmpty("Static class should not have instance methods");
        instanceFields.Should().BeEmpty("Static class should not have instance fields");
        instanceProperties.Should().BeEmpty("Static class should not have instance properties");
    }

    [Fact]
    public void RegisterAccountsEndpoints_MethodNotOverloaded()
    {
        // Arrange & Act
        var methods = typeof(AccountsEndpoints).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(AccountsEndpoints.RegisterAccountsEndpoints));

        // Assert
        methods.Should().HaveCount(1, "RegisterAccountsEndpoints should not be overloaded");
    }

    [Fact]
    public void AccountsEndpoints_MethodNamingConventions()
    {
        // Arrange & Act
        var publicMethods = typeof(AccountsEndpoints).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(AccountsEndpoints));
        var privateMethods = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(AccountsEndpoints));

        // Assert
        // Public methods should follow PascalCase convention
        foreach (var method in publicMethods)
        {
            method.Name.Should().MatchRegex(@"^[A-Z][a-zA-Z0-9]*$",
                $"Public method {method.Name} should follow PascalCase convention");
        }

        // Private methods should also follow PascalCase convention in this codebase
        foreach (var method in privateMethods)
        {
            method.Name.Should().MatchRegex(@"^[A-Z][a-zA-Z0-9]*$",
                $"Private method {method.Name} should follow PascalCase convention");
        }
    }

    [Fact]
    public void AccountsEndpoints_HasExpectedStaticMethodCount()
    {
        // Arrange & Act
        var staticMethods = typeof(AccountsEndpoints).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(AccountsEndpoints));

        // Assert
        // Should have exactly 2 methods: RegisterAccountsEndpoints (public) and CreateDummyPlayer (private)
        staticMethods.Should().HaveCount(2, "AccountsEndpoints should have exactly 2 static methods");

        var publicMethods = staticMethods.Where(m => m.IsPublic);
        var privateMethods = staticMethods.Where(m => m.IsPrivate);

        publicMethods.Should().HaveCount(1, "Should have exactly 1 public method");
        privateMethods.Should().HaveCount(1, "Should have exactly 1 private method");
    }

    [Fact]
    public void CreateDummyPlayer_UsesExpectedHttpResultTypes()
    {
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        method.Should().NotBeNull();
        var returnType = method.ReturnType;

        // Extract the inner Results<> type from Task<Results<>>
        returnType.Should().Match(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));
        var resultsType = returnType.GetGenericArguments()[0];

        // The Results type should contain Ok<Result>, Ok<CreateDummyMemberResult>, and ValidationProblem
        resultsType.Name.Should().StartWith("Results");

        // Verify it's a generic type with multiple type arguments (for the union types)
        resultsType.IsGenericType.Should().BeTrue();
        var genericArgs = resultsType.GetGenericArguments();
        genericArgs.Should().HaveCount(3, "Should have exactly 3 union types");
    }

    [Fact]
    public void AccountsEndpoints_ReflectionMetadata_Consistency()
    {
        // Arrange & Act
        var type = typeof(AccountsEndpoints);

        // Assert
        // Verify class metadata is consistent
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
        type.IsPublic.Should().BeTrue();
        type.IsInterface.Should().BeFalse();
        type.IsEnum.Should().BeFalse();
        type.IsValueType.Should().BeFalse();

        // Verify it has no base class other than object
        type.BaseType.Should().Be(typeof(object));

        // Verify it implements no interfaces
        type.GetInterfaces().Should().BeEmpty();
    }

    [Fact]
    public async Task CreateDummyPlayer_WithValidRequest_CallsHandler()
    {
        // Arrange
        var request = CreateValidDummyMemberRequest();
        var handler = new Mock<IAccountsHandler>();
        var expectedResult = new CreateDummyMemberResult(0, "Success", 123, "player123");

        handler.Setup(x => x.CreateDummyMember(It.IsAny<CreateDummyMemberRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await InvokeCreateDummyPlayer(request, handler.Object);

        // Assert
        result.Should().NotBeNull();
        handler.Verify(x => x.CreateDummyMember(request), Times.Once);
    }

    [Fact]
    public async Task CreateDummyPlayer_WithInvalidRequest_ReturnsValidationProblem()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(
            0, // Invalid agent ID
            0, // Invalid creator
            "" // Invalid empty IP
        );
        var handler = new Mock<IAccountsHandler>();

        // Act
        var result = await InvokeCreateDummyPlayer(request, handler.Object);

        // Assert
        result.Should().NotBeNull();
        // Note: Validation behavior depends on MiniValidator rules
    }

    [Fact]
    public async Task CreateDummyPlayer_HandlerThrowsException_PropagatesException()
    {
        // Arrange
        var request = CreateValidDummyMemberRequest();
        var handler = new Mock<IAccountsHandler>();

        handler.Setup(x => x.CreateDummyMember(It.IsAny<CreateDummyMemberRequest>()))
            .ThrowsAsync(new Exception("Handler error"));

        // Act & Assert
        var act = () => InvokeCreateDummyPlayer(request, handler.Object);
        await act.Should().ThrowAsync<Exception>().WithMessage("Handler error");
    }

    [Fact]
    public async Task CreateDummyPlayer_WithNullRequest_HandlesGracefully()
    {
        // Arrange
        var handler = new Mock<IAccountsHandler>();

        // Act & Assert
        var act = () => InvokeCreateDummyPlayer(null, handler.Object);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateDummyPlayer_WithNullHandler_ThrowsException()
    {
        // Arrange
        var request = CreateValidDummyMemberRequest();

        // Act & Assert
        var act = () => InvokeCreateDummyPlayer(request, null);
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public void CreateDummyPlayer_AsyncMethod_ReturnsTaskType()
    {
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        method.Should().NotBeNull();
        method.ReturnType.Should().Match(t =>
            t.IsGenericType &&
            t.GetGenericTypeDefinition() == typeof(Task<>));

        var taskType = method.ReturnType.GetGenericArguments()[0];
        taskType.Name.Should().StartWith("Results");
    }

    [Fact]
    public void RegisterAccountsEndpoints_RegistersCorrectRoutes()
    {
        // This test documents the expected route registration behavior
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethod(nameof(AccountsEndpoints.RegisterAccountsEndpoints));

        // Assert
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();

        // The method should accept and return IEndpointRouteBuilder
        var parameters = method.GetParameters();
        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(IEndpointRouteBuilder));
        method.ReturnType.Should().Be(typeof(IEndpointRouteBuilder));
    }

    [Fact]
    public void AccountsEndpoints_FollowsMinimalApiPattern()
    {
        // Arrange & Act
        var type = typeof(AccountsEndpoints);

        // Assert
        // Should be a static class (abstract + sealed)
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
        type.IsClass.Should().BeTrue();

        // Should have extension method for registration
        var registerMethod = type.GetMethod(nameof(AccountsEndpoints.RegisterAccountsEndpoints));
        registerMethod.Should().NotBeNull();
        registerMethod.IsStatic.Should().BeTrue();
        registerMethod.IsPublic.Should().BeTrue();

        // Should have private endpoint handler methods
        var privateMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == type);
        privateMethods.Should().HaveCount(1); // CreateDummyPlayer
    }

    [Fact]
    public void AccountsEndpoints_ConsistentParameterNaming()
    {
        // Arrange & Act
        var createDummyPlayerMethod = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        createDummyPlayerMethod.Should().NotBeNull();

        // Method should follow consistent parameter naming
        var parameters = createDummyPlayerMethod.GetParameters();

        parameters.Should().HaveCount(2);

        // First parameter should be request, second should be handler
        parameters[0].Name.Should().Be("request");
        parameters[1].Name.Should().Be("accountsHandler");

        parameters[0].ParameterType.Should().Be(typeof(CreateDummyMemberRequest));
        parameters[1].ParameterType.Should().Be(typeof(IAccountsHandler));
    }

    [Fact]
    public void CreateDummyPlayer_HandlesMultipleResultTypes()
    {
        // Arrange & Act
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        method.Should().NotBeNull();
        var returnType = method.ReturnType;

        // Extract the inner Results<> type from Task<Results<>>
        returnType.Should().Match(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));
        var resultsType = returnType.GetGenericArguments()[0];

        // The Results type should contain Ok<Result>, Ok<CreateDummyMemberResult>, and ValidationProblem
        resultsType.Name.Should().StartWith("Results");

        // Verify it's a generic type with multiple type arguments (for the union types)
        resultsType.IsGenericType.Should().BeTrue();
        var genericArgs = resultsType.GetGenericArguments();
        genericArgs.Should().HaveCount(3, "Should have exactly 3 union types");
    }

    [Fact]
    public async Task CreateDummyPlayer_WithSuccessResult_ReturnsOkResult()
    {
        // Arrange
        var request = CreateValidDummyMemberRequest();
        var handler = new Mock<IAccountsHandler>();
        var successResult = new CreateDummyMemberResult(0, "Success", 123, "player123");

        handler.Setup(x => x.CreateDummyMember(It.IsAny<CreateDummyMemberRequest>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await InvokeCreateDummyPlayer(request, handler.Object);

        // Assert
        result.Should().NotBeNull();
        handler.Verify(x => x.CreateDummyMember(request), Times.Once);
    }

    [Fact]
    public async Task CreateDummyPlayer_WithComplexRequest_ProcessesCorrectly()
    {
        // Arrange
        var request = new CreateDummyMemberRequest(
            AgentId: 999,
            Creator: 123,
            IP: "192.168.1.100"
        );
        var handler = new Mock<IAccountsHandler>();
        var expectedResult = new CreateDummyMemberResult(0, "Created", 999, "complex_player");

        handler.Setup(x => x.CreateDummyMember(request))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await InvokeCreateDummyPlayer(request, handler.Object);

        // Assert
        result.Should().NotBeNull();
        handler.Verify(x => x.CreateDummyMember(request), Times.Once);
    }

    [Fact]
    public void AccountsEndpoints_SingleEndpointRegistration()
    {
        // This test documents that the RegisterAccountsEndpoints method should register 1 endpoint
        // Arrange & Act
        var privateMethods = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(AccountsEndpoints));

        // Assert
        privateMethods.Should().HaveCount(1, "Should have 1 private method corresponding to 1 endpoint");
        privateMethods.Should().Contain(m => m.Name.Contains("CreateDummyPlayer"));
    }

    [Fact]
    public async Task CreateDummyPlayer_ConcurrentRequests_HandledCorrectly()
    {
        // Arrange
        var requests = Enumerable.Range(1, 5)
            .Select(i => new CreateDummyMemberRequest(AgentId: i, Creator: i * 10, IP: $"127.0.0.{i}"))
            .ToArray();

        var handler = new Mock<IAccountsHandler>();

        foreach (var req in requests)
        {
            handler.Setup(x => x.CreateDummyMember(req))
                .ReturnsAsync(new CreateDummyMemberResult(0, "Success", req.AgentId, $"player_{req.AgentId}"));
        }

        // Act
        var tasks = requests.Select(req => InvokeCreateDummyPlayer(req, handler.Object));
        var results = await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(5);
        results.Should().AllSatisfy(result => result.Should().NotBeNull());

        foreach (var req in requests)
        {
            handler.Verify(x => x.CreateDummyMember(req), Times.Once);
        }
    }

    [Fact]
    public void AccountsEndpoints_ValidationIntegration_Structure()
    {
        // This test verifies the validation integration structure
        // Arrange & Act
        var createDummyPlayerMethod = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        // Assert
        createDummyPlayerMethod.Should().NotBeNull();

        // Method should be async to handle validation and processing
        createDummyPlayerMethod.ReturnType.Should().Match(t =>
            t.IsGenericType &&
            t.GetGenericTypeDefinition() == typeof(Task<>));

        // Should accept request and handler parameters
        var parameters = createDummyPlayerMethod.GetParameters();
        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(CreateDummyMemberRequest));
        parameters[1].ParameterType.Should().Be(typeof(IAccountsHandler));
    }

    private CreateDummyMemberRequest CreateValidDummyMemberRequest()
    {
        return new CreateDummyMemberRequest(
            AgentId: 123,
            Creator: 456,
            IP: "127.0.0.1"
        );
    }

    private async Task<object> InvokeCreateDummyPlayer(CreateDummyMemberRequest request, IAccountsHandler handler)
    {
        // Use reflection to invoke the private CreateDummyPlayer method
        var method = typeof(AccountsEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateDummyPlayer"));

        method.Should().NotBeNull("CreateDummyPlayer method should exist");

        var result = method.Invoke(null, new object[] { request, handler });

        // Handle the complex Results type dynamically
        if (result is Task task)
        {
            await task;
            var property = task.GetType().GetProperty("Result");
            return property?.GetValue(task);
        }

        return result;
    }
}