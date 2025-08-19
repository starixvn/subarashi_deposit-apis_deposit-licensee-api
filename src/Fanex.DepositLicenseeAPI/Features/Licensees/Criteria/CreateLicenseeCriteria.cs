namespace Fanex.DepositLicenseeAPI.Features.Licensees.Criteria;

public record CreateLicenseeParams(
    SiteBuilderParams BaseParams,
    bool IsChinaView,
    byte SpecialPlatform,
    byte SiteType);

public class CreateLicenseeCriteria : SiteBuilderBaseCriteria
{
    private readonly CreateLicenseeParams _licenseeParams;

    public CreateLicenseeCriteria(CreateLicenseeParams parameters) : base(parameters.BaseParams)
    {
        _licenseeParams = parameters;
    }

    public bool IsChinaView => _licenseeParams.IsChinaView;

    public byte SpecialPlatform => _licenseeParams.SpecialPlatform;

    public byte SiteType => _licenseeParams.SiteType;

    public string ProductList { get; set; }

    public byte Mode => CreateMode.NewLicensee;
}