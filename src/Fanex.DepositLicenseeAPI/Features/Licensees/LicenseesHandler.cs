namespace Fanex.DepositLicenseeAPI.Features.Licensees;

using System.Text.Json;
using Criteria;
using Data.Repository;
using Logging;
using Shared;
using Shared.SiteDB;
using UserLog;

public interface ILicenseesHandler
{
    Task<Result> CreateLicensee(CreateLicenseeRequest request);
}

public class LicenseesHandler(
    IConfiguration configuration,
    IDynamicRepository repository,
    ISiteDBHandler siteDBHandler,
    IUserLogHandler userLogHandler,
    ILogger logger)
    : BaseHandler(configuration, repository, siteDBHandler, userLogHandler, logger), ILicenseesHandler
{
    public async Task<Result> CreateLicensee(CreateLicenseeRequest request)
    {
        var siteDBConfig = await siteDBHandler.GetBySiteDBGroupId(request.SiteDBGroupId);
        if (siteDBConfig == null)
        {
            return new Result(911, "Create customer failed. Please try again.");
        }

        var validationResult = await ValidateRequest(request.SiteName, CreateMode.NewLicensee, request, siteDBConfig.CustDBGroupId);

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
            CreateMode.NewLicensee,
            siteDBConfig.SiteDBGroupId,
            request.IsChinaView,
            request.SpecialPlatform,
            request.SiteType);
        var registerResult = await RegisterCustomers(registerOptions);

        if (registerResult.result.IsNotOK)
        {
            return registerResult.result;
        }

        failedCurrencies.AddRange([.. registerResult.failedCurrencies]);

        _ = Task.Run(() => InsertBetRestrictionSite(registerResult.result.SiteId, siteDBConfig.CustDBGroupId));

        var createResult = await CreateLicensee(request, registerResult.result.Customers, registerResult.result.SiteId, siteDBConfig.CustDBGroupId);
        failedCurrencies.AddRange([.. createResult.failedCurrencies]);

        return createResult.result;
    }

    private async Task InsertBetRestrictionSite(int siteId, int custDBGroupId)
    {
        try
        {
            var command = new InsertBetRestrictionSiteCommand(custDBGroupId, siteId);
            await repository.ExecuteAsync(command);
            if (command.ErrCode > 0)
            {
                await logger.ErrorAsync(command.ErrMsg);
            }
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
        }
    }

    private async Task<(Result result, List<string> failedCurrencies)> CreateLicensee(
        CreateLicenseeRequest request,
        string customers,
        int siteId,
        int custDBGroupId)
    {
        var criteria = CreateLicenseeCriteria(request, customers, siteId, custDBGroupId);
        var logParams = await repository.FetchAsync<LogParam>(criteria);

        await LogErrorExtraInfoIfPresent(criteria);
        var result = new Result(criteria.ErrCode, criteria.ErrMsg);

        await ProcessLicenseeResult(result, logParams, criteria, request, custDBGroupId);

        return (result, ParseFailedCurrencies(criteria.FailedCurrencies));
    }

    private CreateLicenseeCriteria CreateLicenseeCriteria(
        CreateLicenseeRequest request,
        string customers,
        int siteId,
        int custDBGroupId)
    {
        var baseParams = new SiteBuilderParams(
            custDBGroupId,
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
            request.SiteName,
            SettingStatus);

        var parameters = new CreateLicenseeParams(
            baseParams,
            request.IsChinaView,
            request.SpecialPlatform,
            request.SiteType);

        return new CreateLicenseeCriteria(parameters);
    }

    private async Task LogErrorExtraInfoIfPresent(CreateLicenseeCriteria criteria)
    {
        if (!string.IsNullOrWhiteSpace(criteria.ErrExtraInfo))
        {
            await logger.LogErrExtraInfo(criteria.ErrExtraInfo, criteria.GetSettingKey());
        }
    }

    private async Task ProcessLicenseeResult(
        Result result,
        IEnumerable<LogParam> logParams,
        CreateLicenseeCriteria criteria,
        CreateLicenseeRequest request,
        int custDBGroupId)
    {
        if (result.IsOK)
        {
            await ProcessSuccessfulLicenseeCreation(custDBGroupId, logParams, criteria);
        }

        await HandleLicenseePostProcessingTasks(criteria, request.SiteDBGroupId);
    }

    private async Task ProcessSuccessfulLicenseeCreation(
        int custDBGroupId,
        IEnumerable<LogParam> logParams,
        CreateLicenseeCriteria criteria)
    {
        var writeLogsTask = WriteLogs(custDBGroupId, logParams);
        var insertLicenseeDataTask = InsertLicenseeData(criteria.SiteName, criteria.ProductList, criteria.LicenseeType);
        await Task.WhenAll(writeLogsTask, insertLicenseeDataTask);
    }

    private async Task HandleLicenseePostProcessingTasks(CreateLicenseeCriteria criteria, int siteDBGroupId)
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

    private async Task<Result> InsertLicenseeData(string siteName, string productList, string licenseeType)
    {
        var command = new InsertLicenseeDataCommand(siteName, productList, licenseeType);
        await repository.ExecuteAsync(command);
        var result = new Result(command.ErrCode, command.ErrMsg);

        if (result.IsOK)
        {
            return await InsertCompanyWinLimit(command.LicenseeId);
        }

        return result;
    }
}