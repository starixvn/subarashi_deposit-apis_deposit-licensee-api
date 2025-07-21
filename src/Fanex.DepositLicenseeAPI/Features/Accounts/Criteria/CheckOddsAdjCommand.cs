namespace Fanex.DepositLicenseeAPI.Features.Accounts.Criteria;

using Data.Repository;

public class CheckOddsAdjCommand(string siteId) : NonQueryCommand
{
    public string SiteId { get; } = siteId;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "API_Dep_CustomerCheckOddsAdj";

    public override bool IsValid() => true;
}