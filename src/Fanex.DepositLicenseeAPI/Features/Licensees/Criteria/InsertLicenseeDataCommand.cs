namespace Fanex.DepositLicenseeAPI.Features.Licensees.Criteria;

using Data.Repository;

public class InsertLicenseeDataCommand(string siteName, string productList, string licenseeType) : NonQueryCommand
{
    public string SiteName { get; } = siteName;

    public string ProductList { get; } = productList;

    public string LicenseeType { get; } = licenseeType;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public int LicenseeId { get; set; }

    public override string GetSettingKey() => "API_SiteBuilder_LicenseeDB_Ins";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(SiteName)
           && !string.IsNullOrWhiteSpace(ProductList)
           && !string.IsNullOrWhiteSpace(LicenseeType);
}