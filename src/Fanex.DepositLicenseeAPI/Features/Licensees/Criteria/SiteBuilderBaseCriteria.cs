namespace Fanex.DepositLicenseeAPI.Features.Licensees.Criteria;

using Data;
using Data.Repository;
using Shared.SiteDB;

public record SiteBuilderParams(
    int CustDBGroupId,
    string Customers,
    string ProductBetSetting,
    string ProductPT,
    string ProductStakeLevel,
    string ProductComm,
    string Creator,
    string IP,
    bool IsCreateSub,
    string LicenseeType,
    int SiteId,
    string SiteName,
    byte SettingStatus);

public abstract class SiteBuilderBaseCriteria : CriteriaBase
{
    private readonly SiteBuilderParams _params;

    protected SiteBuilderBaseCriteria(SiteBuilderParams parameters)
    {
        _params = parameters;
    }

    [SpParam(Ignored = true)]
    public int CustDBGroupId => _params.CustDBGroupId;

    [SpParam(SqlParamName = "Customers_List")]
    public string Customers => _params.Customers;

    public string ProductBetSetting => _params.ProductBetSetting;

    public string ProductPT => _params.ProductPT;

    public string ProductStakeLevel => _params.ProductStakeLevel;

    public string ProductComm => _params.ProductComm;

    public string Creator => _params.Creator;

    public string IP => _params.IP;

    public bool IsCreateSub => _params.IsCreateSub;

    public string LicenseeType => _params.LicenseeType;

    public int SiteId => _params.SiteId;

    public string SiteName => _params.SiteName;

    public byte SettingStatus => _params.SettingStatus;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string ErrExtraInfo { get; set; }

    public string AgentList { get; set; }

    public string InsBalExceptCustList { get; set; }

    public string FailedCurrencies { get; set; }

    public string ListSuperRollBack { get; set; }

    public override string GetSettingKey() => SiteDBHelpers.GetSPNameInSiteDB(_params.CustDBGroupId, "API_SiteBuilder_Licensee_Insert");

    public override bool IsValid() => !string.IsNullOrEmpty(_params.Customers);
}