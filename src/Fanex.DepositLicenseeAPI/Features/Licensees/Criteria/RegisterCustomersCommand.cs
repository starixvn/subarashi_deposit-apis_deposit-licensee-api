namespace Fanex.DepositLicenseeAPI.Features.Licensees.Criteria;

using Data;
using Data.Repository;

public record RegisterCustomersParams(
    string RegisterUsernames,
    string SiteName,
    bool IsChinaView,
    byte SpecialPlatform,
    byte SiteType,
    string ProductStakeLevel,
    string IP,
    byte Mode,
    int SiteDBGroupId);

public class RegisterCustomersCommand : NonQueryCommand
{
    private readonly RegisterCustomersParams _params;

    public RegisterCustomersCommand(RegisterCustomersParams parameters)
    {
        _params = parameters;
    }

    [SpParam(SqlParamName = "Usernames_Register")]
    public string RegisterUsernames => _params.RegisterUsernames;

    public string SiteName => _params.SiteName;

    public bool IsChinaView => _params.IsChinaView;

    public byte SpecialPlatform => _params.SpecialPlatform;

    public byte SiteType => _params.SiteType;

    public string ProductStakeLevel => _params.ProductStakeLevel;

    public string IP => _params.IP;

    public byte Mode => _params.Mode;

    public int SiteDBGroupId => _params.SiteDBGroupId;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public int SiteId { get; set; }

    [SpParam(SqlParamName = "Customers_List")]
    public string Customers { get; set; }

    public string FailedCurrencies { get; set; }

    public string ErrExtraInfo { get; set; }

    public override string GetSettingKey() => "API_SiteBuilder_Customers_Register";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(_params.RegisterUsernames);
}