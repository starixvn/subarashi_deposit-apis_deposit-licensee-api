namespace Fanex.DepositLicenseeAPI.Features.Licensees.Criteria;

using Data.Repository;

public class RollbackCreateLicenseeCommand(string custIds) : NonQueryCommand
{
    public string CustIds { get; } = custIds;

    public byte IsRollbackFullLine { get; } = 1;

    public override string GetSettingKey() => "Acc_CustomerIns_Rollback_Multiple";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(CustIds);
}