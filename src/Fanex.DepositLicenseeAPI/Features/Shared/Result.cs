namespace Fanex.DepositLicenseeAPI.Features.Shared;

using System.Text.Json.Serialization;

public class Result(int errorCode, string errorMessage)
{
    public int ErrorCode { get; } = errorCode;

    public string ErrorMessage { get; } = errorMessage;

    [JsonIgnore]
    public bool IsOK => ErrorCode == 0;

    [JsonIgnore]
    public bool IsNotOK => !IsOK;
}