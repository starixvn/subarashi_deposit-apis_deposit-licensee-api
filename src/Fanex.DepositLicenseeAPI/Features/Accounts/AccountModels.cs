namespace Fanex.DepositLicenseeAPI.Features.Accounts;

using System.ComponentModel.DataAnnotations;
using Shared;

public class CreateDummyMemberResult(int errorCode, string errorMessage, int custId, string username)
    : Result(errorCode, errorMessage)
{
    public int CustId { get; } = custId;

    public string Username { get; } = username;
}

public record CreateDummyMemberRequest([Required] int AgentId, [Required] int Creator, string IP);