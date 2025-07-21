namespace Fanex.DepositLicenseeAPI.Features.Shared;

using System.Text;
using Logging;

public static class ILoggerExtensions
{
    public static Task LogErrExtraInfo(this ILogger logger, string errExtraInfo, string settingKey)
    {
        var message = new StringBuilder();
        message.AppendLine($"ErrExtraInfo: {errExtraInfo}");
        message.AppendLine($"SettingKey: {settingKey}");
        return logger.ErrorAsync(message.ToString());
    }
}