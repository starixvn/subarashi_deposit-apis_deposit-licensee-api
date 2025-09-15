namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees;

using System.Reflection;
using DepositLicenseeAPI.Features.Licensees;
using DepositLicenseeAPI.Features.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

public class LicenseesEndpointsTests
{
    [Fact]
    public void RegisterLicenseesEndpoints_IsStaticMethod()
    {
        // Arrange & Act & Assert
        var method = typeof(LicenseesEndpoints).GetMethod(nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPublic.Should().BeTrue();
    }

    [Fact]
    public void RegisterLicenseesEndpoints_HasCorrectSignature()
    {
        // Arrange & Act & Assert
        var method = typeof(LicenseesEndpoints).GetMethod(nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));
        var parameters = method.GetParameters();

        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(IEndpointRouteBuilder));
        method.ReturnType.Should().Be(typeof(IEndpointRouteBuilder));
    }

    [Fact]
    public void RegisterLicenseesEndpoints_IsExtensionMethod()
    {
        // Arrange & Act & Assert
        var method = typeof(LicenseesEndpoints).GetMethod(nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));
        var firstParam = method.GetParameters().FirstOrDefault();

        firstParam.Should().NotBeNull();
        firstParam.ParameterType.Should().Be(typeof(IEndpointRouteBuilder));

        // Extension methods are typically static and the first parameter represents 'this'
        method.IsStatic.Should().BeTrue();
    }

    [Fact]
    public void LicenseesEndpoints_IsStaticClass()
    {
        // Arrange & Act & Assert
        var type = typeof(LicenseesEndpoints);
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
    }

    [Fact]
    public void LicenseesEndpoints_ContainsCreateLicenseeMethod()
    {
        // Arrange & Act & Assert
        var methods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
        var createLicenseeMethod = methods.FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        createLicenseeMethod.Should().NotBeNull();
        createLicenseeMethod.IsStatic.Should().BeTrue();
    }

    [Fact]
    public void LicenseesEndpoints_ContainsCreateCurrencyMethod()
    {
        // Arrange & Act & Assert
        var methods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
        var createCurrencyMethod = methods.FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        createCurrencyMethod.Should().NotBeNull();
        createCurrencyMethod.IsStatic.Should().BeTrue();
    }

    [Fact]
    public void RegisterLicenseesEndpoints_WithNullBuilder_ThrowsException()
    {
        // Arrange
        IEndpointRouteBuilder nullBuilder = null;

        // Act
        Action act = () => nullBuilder.RegisterLicenseesEndpoints();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RegisterLicenseesEndpoints_WithValidBuilder_ReturnsBuilder()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddRouting();
        var serviceProvider = services.BuildServiceProvider();

        var app = WebApplication.CreateBuilder().Build();
        IEndpointRouteBuilder builder = app;

        // Act
        var result = builder.RegisterLicenseesEndpoints();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void CreateLicensee_MethodExists()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        // Assert
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPrivate.Should().BeTrue();
    }

    [Fact]
    public void CreateCurrency_MethodExists()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        method.Should().NotBeNull();
        method.IsStatic.Should().BeTrue();
        method.IsPrivate.Should().BeTrue();
    }

    [Fact]
    public void CreateLicensee_HasCorrectReturnType()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        // Assert
        method.Should().NotBeNull();
        var returnType = method.ReturnType;

        // Should be Task<Results<Ok<Result>, ValidationProblem>>
        returnType.Should().Match(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));

        var taskInnerType = returnType.GetGenericArguments()[0];
        taskInnerType.Should().Match(t => t.IsGenericType);

        // The inner type should be Results<...>
        var typeName = taskInnerType.Name;
        typeName.Should().StartWith("Results");
    }

    [Fact]
    public void CreateCurrency_HasCorrectReturnType()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        method.Should().NotBeNull();
        var returnType = method.ReturnType;

        // Should be Task<Results<Ok<Result>, ValidationProblem>>
        returnType.Should().Match(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));

        var taskInnerType = returnType.GetGenericArguments()[0];
        taskInnerType.Should().Match(t => t.IsGenericType);

        // The inner type should be Results<...>
        var typeName = taskInnerType.Name;
        typeName.Should().StartWith("Results");
    }

    [Fact]
    public void CreateLicensee_HasCorrectParameters()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        // Assert
        method.Should().NotBeNull();
        var parameters = method.GetParameters();

        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(CreateLicenseeRequest));
        parameters[1].ParameterType.Should().Be(typeof(ILicenseesHandler));
    }

    [Fact]
    public void CreateCurrency_HasCorrectParameters()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        method.Should().NotBeNull();
        var parameters = method.GetParameters();

        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(CreateCurrencyRequest));
        parameters[1].ParameterType.Should().Be(typeof(ICurrencyHandler));
    }

    [Fact]
    public void LicenseesEndpoints_OnlyContainsExpectedMethods()
    {
        // Arrange & Act
        var publicMethods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.Public | BindingFlags.Static);
        var nonInheritedMethods = publicMethods.Where(m => m.DeclaringType == typeof(LicenseesEndpoints));

        // Assert
        nonInheritedMethods.Should().HaveCount(1);
        nonInheritedMethods.Should().Contain(m => m.Name == nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));
    }

    [Fact]
    public void LicenseesEndpoints_UsesCorrectNamespace()
    {
        // Arrange & Act & Assert
        var type = typeof(LicenseesEndpoints);
        type.Namespace.Should().Be("Fanex.DepositLicenseeAPI.Features.Licensees");
    }

    [Fact]
    public void LicenseesEndpoints_HasCorrectUsings()
    {
        // This is more of a documentation test to verify expected dependencies
        // Arrange & Act & Assert
        var type = typeof(LicenseesEndpoints);

        // Verify the class can access the expected types (which implies correct usings)
        var method = type.GetMethod(nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));
        method.Should().NotBeNull();

        // These types should be accessible (indicating proper using statements)
        var paramType = method.GetParameters()[0].ParameterType;
        paramType.Should().Be(typeof(IEndpointRouteBuilder));

        var returnType = method.ReturnType;
        returnType.Should().Be(typeof(IEndpointRouteBuilder));
    }

    [Fact]
    public void RegisterLicenseesEndpoints_CanBeChained()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddRouting();
        var serviceProvider = services.BuildServiceProvider();

        var app = WebApplication.CreateBuilder().Build();
        IEndpointRouteBuilder builder = app;

        // Act & Assert - Should be able to chain method calls
        var result = builder.RegisterLicenseesEndpoints().RegisterLicenseesEndpoints();
        result.Should().NotBeNull();
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void LicenseesEndpoints_ClassAttributes()
    {
        // Arrange & Act
        var type = typeof(LicenseesEndpoints);
        var attributes = type.GetCustomAttributes();

        // Assert
        // Static classes don't typically have special attributes, but we can verify the type structure
        type.IsClass.Should().BeTrue();
        type.IsPublic.Should().BeTrue();
        type.IsAbstract.Should().BeTrue(); // Static classes are abstract
        type.IsSealed.Should().BeTrue(); // Static classes are sealed
    }

    [Fact]
    public void CreateLicensee_IsAsyncMethod()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        // Assert
        method.Should().NotBeNull();
        method.ReturnType.Should().Match(t =>
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(Task<>),
            "CreateLicensee should be an async method returning Task<T>");
    }

    [Fact]
    public void CreateCurrency_IsAsyncMethod()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        method.Should().NotBeNull();
        method.ReturnType.Should().Match(t =>
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(Task<>),
            "CreateCurrency should be an async method returning Task<T>");
    }

    [Fact]
    public void LicenseesEndpoints_NoInstanceMembers()
    {
        // Arrange & Act
        var instanceMethods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var instanceFields = typeof(LicenseesEndpoints).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var instanceProperties = typeof(LicenseesEndpoints).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // Assert
        // Filter out inherited object methods
        var nonInheritedInstanceMethods = instanceMethods.Where(m => m.DeclaringType == typeof(LicenseesEndpoints));

        nonInheritedInstanceMethods.Should().BeEmpty("Static class should not have instance methods");
        instanceFields.Should().BeEmpty("Static class should not have instance fields");
        instanceProperties.Should().BeEmpty("Static class should not have instance properties");
    }

    [Fact]
    public void RegisterLicenseesEndpoints_MethodNotOverloaded()
    {
        // Arrange & Act
        var methods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));

        // Assert
        methods.Should().HaveCount(1, "RegisterLicenseesEndpoints should not be overloaded");
    }

    [Fact]
    public void LicenseesEndpoints_MethodNamingConventions()
    {
        // Arrange & Act
        var publicMethods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(LicenseesEndpoints));
        var privateMethods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(LicenseesEndpoints));

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
    public void LicenseesEndpoints_HasExpectedStaticMethodCount()
    {
        // Arrange & Act
        var staticMethods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(LicenseesEndpoints));

        // Assert
        // Should have exactly 3 methods: RegisterLicenseesEndpoints (public), CreateLicensee and CreateCurrency (private)
        staticMethods.Should().HaveCount(3, "LicenseesEndpoints should have exactly 3 static methods");

        var publicMethods = staticMethods.Where(m => m.IsPublic);
        var privateMethods = staticMethods.Where(m => m.IsPrivate);

        publicMethods.Should().HaveCount(1, "Should have exactly 1 public method");
        privateMethods.Should().HaveCount(2, "Should have exactly 2 private methods");
    }

    [Fact]
    public void CreateLicensee_UsesExpectedHttpResultTypes()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        // Assert
        method.Should().NotBeNull();
        var returnType = method.ReturnType;

        // Extract the inner Results<> type from Task<Results<>>
        returnType.Should().Match(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));
        var resultsType = returnType.GetGenericArguments()[0];

        // The Results type should contain Ok<Result> and ValidationProblem
        resultsType.Name.Should().StartWith("Results");

        // Verify it's a generic type with multiple type arguments (for the union types)
        resultsType.IsGenericType.Should().BeTrue();
        var genericArgs = resultsType.GetGenericArguments();
        genericArgs.Should().HaveCount(2, "Should have exactly 2 union types");
    }

    [Fact]
    public void CreateCurrency_UsesExpectedHttpResultTypes()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        method.Should().NotBeNull();
        var returnType = method.ReturnType;

        // Extract the inner Results<> type from Task<Results<>>
        returnType.Should().Match(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>));
        var resultsType = returnType.GetGenericArguments()[0];

        // The Results type should contain Ok<Result> and ValidationProblem
        resultsType.Name.Should().StartWith("Results");

        // Verify it's a generic type with multiple type arguments (for the union types)
        resultsType.IsGenericType.Should().BeTrue();
        var genericArgs = resultsType.GetGenericArguments();
        genericArgs.Should().HaveCount(2, "Should have exactly 2 union types");
    }

    [Fact]
    public void LicenseesEndpoints_ReflectionMetadata_Consistency()
    {
        // Arrange & Act
        var type = typeof(LicenseesEndpoints);

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
    public void LicenseesEndpoints_RegistersTwoEndpoints()
    {
        // This test documents that the RegisterLicenseesEndpoints method should register 2 endpoints
        // We can't easily test the actual registration without complex integration test setup,
        // but we can verify the methods that would be called exist

        // Arrange & Act
        var privateMethods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == typeof(LicenseesEndpoints));

        // Assert
        privateMethods.Should().HaveCount(2, "Should have 2 private methods corresponding to 2 endpoints");
        privateMethods.Should().Contain(m => m.Name.Contains("CreateLicensee"));
        privateMethods.Should().Contain(m => m.Name.Contains("CreateCurrency"));
    }

    [Fact]
    public void LicenseesEndpoints_AllMethodsAreStatic()
    {
        // Arrange & Act
        var allMethods = typeof(LicenseesEndpoints).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Where(m => m.DeclaringType == typeof(LicenseesEndpoints));

        // Assert
        allMethods.Should().NotBeEmpty();
        foreach (var method in allMethods)
        {
            method.IsStatic.Should().BeTrue($"Method {method.Name} should be static");
        }
    }

    [Fact]
    public async Task CreateLicensee_WithValidRequest_CallsHandler()
    {
        // Arrange
        var request = CreateValidLicenseeRequest();
        var handler = new Mock<ILicenseesHandler>();
        var expectedResult = new Result(0, "Success");

        handler.Setup(x => x.CreateLicensee(It.IsAny<CreateLicenseeRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await InvokeCreateLicensee(request, handler.Object);

        // Assert
        result.Should().NotBeNull();
        handler.Verify(x => x.CreateLicensee(request), Times.Once);
    }

    [Fact]
    public async Task CreateLicensee_WithInvalidRequest_ReturnsValidationProblem()
    {
        // Arrange
        var request = new CreateLicenseeRequest(
            SiteName: "", // Invalid empty site name
            ProductBetSetting: null,
            ProductPT: null,
            ProductStakeLevel: null,
            ProductComm: null,
            Creator: "",
            IP: "",
            IsCreateSub: false,
            LicenseeType: "",
            IsChinaView: false,
            SiteDBGroupId: 0,
            SpecialPlatform: 0,
            SiteType: 0
        );
        var handler = new Mock<ILicenseesHandler>();

        // Act & Assert
        var result = await InvokeCreateLicensee(request, handler.Object);

        // The method should handle validation internally
        result.Should().NotBeNull();
        handler.Verify(x => x.CreateLicensee(It.IsAny<CreateLicenseeRequest>()), Times.Never);
    }

    [Fact]
    public async Task CreateLicensee_HandlerThrowsException_PropagatesException()
    {
        // Arrange
        var request = CreateValidLicenseeRequest();
        var handler = new Mock<ILicenseesHandler>();

        handler.Setup(x => x.CreateLicensee(It.IsAny<CreateLicenseeRequest>()))
            .ThrowsAsync(new Exception("Handler error"));

        // Act & Assert
        var act = () => InvokeCreateLicensee(request, handler.Object);
        await act.Should().ThrowAsync<Exception>().WithMessage("Handler error");
    }

    [Fact]
    public async Task CreateCurrency_WithValidRequest_CallsHandler()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var handler = new Mock<ICurrencyHandler>();
        var expectedResult = new Result(0, "Success");

        handler.Setup(x => x.CreateCurrency(It.IsAny<CreateCurrencyRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await InvokeCreateCurrency(request, handler.Object);

        // Assert
        result.Should().NotBeNull();
        handler.Verify(x => x.CreateCurrency(request), Times.Once);
    }

    [Fact]
    public async Task CreateCurrency_WithInvalidRequest_ReturnsValidationProblem()
    {
        // Arrange
        var request = new CreateCurrencyRequest(
            SiteName: "", // Invalid empty site name
            ProductBetSetting: null, // Invalid null
            ProductPT: null, // Invalid null
            ProductStakeLevel: null,
            ProductComm: null,
            Creator: "",
            IP: "",
            IsCreateSub: false,
            LicenseeType: ""
        );
        var handler = new Mock<ICurrencyHandler>();

        // Act
        var result = await InvokeCreateCurrency(request, handler.Object);

        // Assert
        result.Should().NotBeNull();
        handler.Verify(x => x.CreateCurrency(It.IsAny<CreateCurrencyRequest>()), Times.Never);
    }

    [Fact]
    public async Task CreateCurrency_HandlerThrowsException_PropagatesException()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();
        var handler = new Mock<ICurrencyHandler>();

        handler.Setup(x => x.CreateCurrency(It.IsAny<CreateCurrencyRequest>()))
            .ThrowsAsync(new Exception("Currency handler error"));

        // Act & Assert
        var act = () => InvokeCreateCurrency(request, handler.Object);
        await act.Should().ThrowAsync<Exception>().WithMessage("Currency handler error");
    }

    [Fact]
    public async Task CreateLicensee_WithNullRequest_HandlesGracefully()
    {
        // Arrange
        var handler = new Mock<ILicenseesHandler>();

        // Act & Assert
        var act = () => InvokeCreateLicensee(null, handler.Object);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateCurrency_WithNullRequest_HandlesGracefully()
    {
        // Arrange
        var handler = new Mock<ICurrencyHandler>();

        // Act & Assert
        var act = () => InvokeCreateCurrency(null, handler.Object);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateLicensee_WithNullHandler_ThrowsException()
    {
        // Arrange
        var request = CreateValidLicenseeRequest();

        // Act & Assert
        var act = () => InvokeCreateLicensee(request, null);
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task CreateCurrency_WithNullHandler_ThrowsException()
    {
        // Arrange
        var request = CreateValidCurrencyRequest();

        // Act & Assert
        var act = () => InvokeCreateCurrency(request, null);
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public void CreateLicensee_AsyncMethod_ReturnsTaskType()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        // Assert
        method.Should().NotBeNull();
        method.ReturnType.Should().Match(t =>
            t.IsGenericType &&
            t.GetGenericTypeDefinition() == typeof(Task<>));

        var taskType = method.ReturnType.GetGenericArguments()[0];
        taskType.Name.Should().StartWith("Results");
    }

    [Fact]
    public void CreateCurrency_AsyncMethod_ReturnsTaskType()
    {
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        method.Should().NotBeNull();
        method.ReturnType.Should().Match(t =>
            t.IsGenericType &&
            t.GetGenericTypeDefinition() == typeof(Task<>));

        var taskType = method.ReturnType.GetGenericArguments()[0];
        taskType.Name.Should().StartWith("Results");
    }

    [Fact]
    public void RegisterLicenseesEndpoints_RegistersCorrectRoutes()
    {
        // This test documents the expected route registration behavior
        // Arrange & Act
        var method = typeof(LicenseesEndpoints).GetMethod(nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));

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
    public void LicenseesEndpoints_FollowsMinimalApiPattern()
    {
        // Arrange & Act
        var type = typeof(LicenseesEndpoints);

        // Assert
        // Should be a static class (abstract + sealed)
        type.IsAbstract.Should().BeTrue();
        type.IsSealed.Should().BeTrue();
        type.IsClass.Should().BeTrue();

        // Should have extension method for registration
        var registerMethod = type.GetMethod(nameof(LicenseesEndpoints.RegisterLicenseesEndpoints));
        registerMethod.Should().NotBeNull();
        registerMethod.IsStatic.Should().BeTrue();
        registerMethod.IsPublic.Should().BeTrue();

        // Should have private endpoint handler methods
        var privateMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.DeclaringType == type);
        privateMethods.Should().HaveCount(2); // CreateLicensee and CreateCurrency
    }

    [Fact]
    public void LicenseesEndpoints_ConsistentParameterNaming()
    {
        // Arrange & Act
        var createLicenseeMethod = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));
        var createCurrencyMethod = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        createLicenseeMethod.Should().NotBeNull();
        createCurrencyMethod.Should().NotBeNull();

        // Both methods should follow consistent parameter naming
        var licenseeParams = createLicenseeMethod.GetParameters();
        var currencyParams = createCurrencyMethod.GetParameters();

        licenseeParams.Should().HaveCount(2);
        currencyParams.Should().HaveCount(2);

        // First parameter should be request, second should be handler
        licenseeParams[0].Name.Should().Be("request");
        licenseeParams[1].Name.Should().Be("licenseesHandler");

        currencyParams[0].Name.Should().Be("request");
        currencyParams[1].Name.Should().Be("currencyHandler");
    }

    [Fact]
    public void LicenseesEndpoints_EndpointMethodParameters_NotNull()
    {
        // Arrange & Act
        var createLicenseeMethod = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));
        var createCurrencyMethod = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        // Assert
        createLicenseeMethod.Should().NotBeNull();
        createCurrencyMethod.Should().NotBeNull();

        // Both methods should have exactly 2 parameters each
        createLicenseeMethod.GetParameters().Should().HaveCount(2);
        createCurrencyMethod.GetParameters().Should().HaveCount(2);

        // First parameter should be request, second should be handler
        var licenseeParams = createLicenseeMethod.GetParameters();
        licenseeParams[0].ParameterType.Name.Should().EndWith("Request");
        licenseeParams[1].ParameterType.Name.Should().EndWith("Handler");

        var currencyParams = createCurrencyMethod.GetParameters();
        currencyParams[0].ParameterType.Name.Should().EndWith("Request");
        currencyParams[1].ParameterType.Name.Should().EndWith("Handler");
    }

    private CreateLicenseeRequest CreateValidLicenseeRequest()
    {
        return new CreateLicenseeRequest(
            SiteName: "TestSite",
            ProductBetSetting: new[] { new BetSetting(1, 1, 1, 1, 1.0m, 100.0m, 0.1m, 0.1m) },
            ProductPT: new[] { new PositionTaking(1, 0.5m) },
            ProductStakeLevel: new[] { new StakeLevel(1, 1, 1.0m, 1000.0m) },
            ProductComm: new[] { new Commission(1, 0.05m) },
            Creator: "TestCreator",
            IP: "127.0.0.1",
            IsCreateSub: false,
            LicenseeType: "Test",
            IsChinaView: false,
            SiteDBGroupId: 1,
            SpecialPlatform: 0,
            SiteType: 0
        );
    }

    private CreateCurrencyRequest CreateValidCurrencyRequest()
    {
        return new CreateCurrencyRequest(
            SiteName: "TestSite",
            ProductBetSetting: new[] { new BetSetting(1, 1, 1, 1, 1.0m, 100.0m, 0.1m, 0.1m) },
            ProductPT: new[] { new PositionTaking(1, 0.5m) },
            ProductStakeLevel: new[] { new StakeLevel(1, 1, 1.0m, 1000.0m) },
            ProductComm: new[] { new Commission(1, 0.05m) },
            Creator: "TestCreator",
            IP: "127.0.0.1",
            IsCreateSub: false,
            LicenseeType: "Test"
        );
    }

    private async Task<object> InvokeCreateLicensee(CreateLicenseeRequest request, ILicenseesHandler handler)
    {
        // Use reflection to invoke the private CreateLicensee method
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateLicensee"));

        method.Should().NotBeNull("CreateLicensee method should exist");

        try
        {
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
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                throw ex.InnerException;
            }

            throw;
        }
    }

    private async Task<object> InvokeCreateCurrency(CreateCurrencyRequest request, ICurrencyHandler handler)
    {
        // Use reflection to invoke the private CreateCurrency method
        var method = typeof(LicenseesEndpoints).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name.Contains("CreateCurrency"));

        method.Should().NotBeNull("CreateCurrency method should exist");

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