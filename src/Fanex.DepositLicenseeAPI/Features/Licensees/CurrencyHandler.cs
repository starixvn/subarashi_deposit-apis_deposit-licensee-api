namespace Fanex.DepositLicenseeAPI.Features.Licensees;

using System.Text.Json;
using Criteria;
using Data.Repository;
using Logging;
using Shared;
using Shared.SiteDB;
using UserLog;

public interface ICurrencyHandler
{
    Task<Result> CreateCurrency(CreateCurrencyRequest request);
}

public class CurrencyHandler(
    IConfiguration configuration,
    IDynamicRepository repository,
    ISiteDBHandler siteDBHandler,
    IUserLogHandler userLogHandler,
    ILogger logger)
    : BaseHandler(configuration, repository, siteDBHandler, userLogHandler, logger), ICurrencyHandler
{
    public async Task<Result> CreateCurrency(CreateCurrencyRequest request)
    {
        var siteDBConfig = await siteDBHandler.GetBySite(request.SiteName);
        if (siteDBConfig == null)
        {
            return new Result(911, "Create customer failed. Please try again.");
        }

        var validationResult = await ValidateRequest(request.SiteName, CreateMode.NewCurrency, request, siteDBConfig.CustDBGroupId);
        if (validationResult.IsNotOK)
        {
            return validationResult;
        }

        var failedCurrencies = new List<string>();
        var registerOptions = new RegisterCustomersOptions(
            validationResult.RegisterUsernames,
            request.SiteName,
            request.ProductStakeLevel,
            request.IP,
            CreateMode.NewCurrency,
            siteDBConfig.CustDBGroupId);
        var registerResult = await RegisterCustomers(registerOptions);

        if (registerResult.result.IsNotOK)
        {
            return registerResult.result;
        }

        failedCurrencies.AddRange([.. registerResult.failedCurrencies]);

        var createResult = await CreateCurrency(request, registerResult.result.Customers, registerResult.result.SiteId, request.SiteName, siteDBConfig);
        failedCurrencies.AddRange([.. createResult.failedCurrencies]);

        return createResult.result;
    }

    private async Task<(Result result, List<string> failedCurrencies)> CreateCurrency(
        CreateCurrencyRequest request,
        string customers,
        int siteId,
        string siteName,
        SiteDBConfig siteDBConfig)
    {
        var criteria = CreateCurrencyCriteria(request, customers, siteId, siteName, siteDBConfig);
        var logParams = await repository.FetchAsync<LogParam>(criteria);
        var result = new Result(criteria.ErrCode, criteria.ErrMsg);

        await ProcessCurrencyResult(result, logParams, criteria, request, siteDBConfig);

        return (result, ParseFailedCurrencies(criteria.FailedCurrencies));
    }

    private CreateCurrencyCriteria CreateCurrencyCriteria(
        CreateCurrencyRequest request,
        string customers,
        int siteId,
        string siteName,
        SiteDBConfig siteDBConfig)
    {
        var parameters = new SiteBuilderParams(
            siteDBConfig.CustDBGroupId,
            customers,
            request.ProductBetSetting != null ? JsonSerializer.Serialize(request.ProductBetSetting) : null,
            request.ProductPT != null ? JsonSerializer.Serialize(request.ProductPT) : null,
            request.ProductStakeLevel != null ? JsonSerializer.Serialize(request.ProductStakeLevel) : null,
            request.ProductComm != null ? JsonSerializer.Serialize(request.ProductComm) : null,
            request.Creator,
            request.IP,
            request.IsCreateSub,
            request.LicenseeType,
            siteId,
            siteName,
            SettingStatus);
        return new CreateCurrencyCriteria(parameters);
    }

    private async Task ProcessCurrencyResult(
        Result result,
        IEnumerable<LogParam> logParams,
        CreateCurrencyCriteria criteria,
        CreateCurrencyRequest request,
        SiteDBConfig siteDBConfig)
    {
        if (result.IsOK && logParams.Any())
        {
            await ProcessSuccessfulCurrencyCreation(criteria, request, siteDBConfig, logParams);
        }

        await HandlePostProcessingTasks(criteria, siteDBConfig.SiteDBGroupId);
    }

    private async Task ProcessSuccessfulCurrencyCreation(
        CreateCurrencyCriteria criteria,
        CreateCurrencyRequest request,
        SiteDBConfig siteDBConfig,
        IEnumerable<LogParam> logParams)
    {
        await Task.WhenAll(
            WriteLogs(siteDBConfig.CustDBGroupId, logParams),
            InsertCurrencyData(criteria.SiteName, criteria.CurrencyList, criteria.PT_Sportsbook, request.IP));
    }

    private async Task HandlePostProcessingTasks(CreateCurrencyCriteria criteria, int siteDBGroupId)
    {
        if (!string.IsNullOrWhiteSpace(criteria.InsBalExceptCustList))
        {
            _ = Task.Run(() => InsertBalanceException(criteria.InsBalExceptCustList, siteDBGroupId));
        }

        if (!string.IsNullOrWhiteSpace(criteria.ListSuperRollBack))
        {
            await RollbackMultiple(criteria.ListSuperRollBack);
        }
    }

    private static List<string> ParseFailedCurrencies(string failedCurrencies)
        => failedCurrencies?.Split(',').ToList() ?? [];

    private async Task<Result> InsertCurrencyData(string siteName, string currencyList, decimal PT_Sportsbook, string ip)
    {
        var insertCurrencyCommand = new InsertCurrencyCommand(siteName, currencyList, PT_Sportsbook, ip);
        await repository.ExecuteAsync(insertCurrencyCommand);

        var result = new Result(insertCurrencyCommand.ErrCode, insertCurrencyCommand.ErrMsg);
        if (result.IsOK)
        {
            return await InsertCompanyWinLimit(insertCurrencyCommand.LicenseeId);
        }

        return result;
    }
}