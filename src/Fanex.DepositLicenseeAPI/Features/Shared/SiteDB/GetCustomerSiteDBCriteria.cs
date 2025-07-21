namespace Fanex.DepositLicenseeAPI.Features.Shared.SiteDB;

using Data.Repository;

public class GetCustomerSiteDBCriteria(int siteDBGroupId) : CriteriaBase
{
    public int SiteDBGroupId { get; } = siteDBGroupId;

    public override string GetSettingKey() => "API_SiteBuilder_GetCustomerSiteDB";

    public override bool IsValid() => true;
}