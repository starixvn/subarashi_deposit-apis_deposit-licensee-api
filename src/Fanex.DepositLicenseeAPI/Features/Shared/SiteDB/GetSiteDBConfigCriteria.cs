namespace Fanex.DepositLicenseeAPI.Features.Shared.SiteDB;

using Data.Repository;

public class GetSiteDBConfigCriteria(string site) : CriteriaBase
{
    public string Site { get; } = site;

    public override string GetSettingKey() => "Acc_ServerDBConfigSel";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Site);
}