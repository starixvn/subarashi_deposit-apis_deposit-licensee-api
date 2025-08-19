namespace Fanex.DepositLicenseeAPI.Features.Licensees.Criteria;

public class CreateCurrencyCriteria : SiteBuilderBaseCriteria
{
    public CreateCurrencyCriteria(SiteBuilderParams parameters) : base(parameters)
    {
    }
    public string CurrencyList { get; set; }

    public decimal PT_Sportsbook { get; set; }

    public byte Mode => CreateMode.NewCurrency;
}