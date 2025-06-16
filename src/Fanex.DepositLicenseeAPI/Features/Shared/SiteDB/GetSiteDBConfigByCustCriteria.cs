namespace Fanex.DepositLicenseeAPI.Features.Shared.SiteDB;

using Data.Repository;

public class GetSiteDBConfigByCustCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Acc_Dep_ServerDBConfigSelByCust";

    public override bool IsValid() => CustId > 0;
}