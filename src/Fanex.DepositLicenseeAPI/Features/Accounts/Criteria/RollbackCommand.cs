namespace Fanex.DepositLicenseeAPI.Features.Accounts.Criteria;

using Data.Repository;

public class RollbackCommand(int custId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Acc_CustomerIns_Rollback";

    public override bool IsValid() => CustId > 0;
}