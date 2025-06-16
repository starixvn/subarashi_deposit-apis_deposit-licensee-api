namespace Fanex.DepositLicenseeAPI.Features.UserLog;

using Data.Repository;
using Logging;

public interface IUserLogHandler
{
    Task WriteLog(int custDBGroupId, LogParam param);
}

public class UserLogHandler(IDynamicRepository repository, ILogger logger) : IUserLogHandler
{
    public async Task WriteLog(int custDBGroupId, LogParam param)
    {
        try
        {
            await repository.ExecuteAsync(new InsertUserLogMainCommand(
                param.UserId,
                param.CustId,
                param.ActionId,
                param.TrackingValue,
                param.TrackingValue2)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);

            await repository.ExecuteAsync(new InsertUserLogSiteDBCommand(
                custDBGroupId,
                param.UserId,
                param.CustId,
                param.ActionId,
                param.TrackingValue,
                param.TrackingValue2)).ConfigureAwait(false);
        }
    }
}