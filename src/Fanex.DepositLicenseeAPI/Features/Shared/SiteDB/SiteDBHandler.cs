namespace Fanex.DepositLicenseeAPI.Features.Shared.SiteDB;

using Data.Repository;

public interface ISiteDBHandler
{
    Task<SiteDBConfig> GetBySiteDBGroupId(int siteDBGroupId);

    Task<SiteDBConfig> GetBySite(string site);

    Task<SiteDBConfig> GetByCustId(int custId);

    Task<string> GetSiteId(string siteName);
}

public class SiteDBHandler(IDynamicRepository repository) : ISiteDBHandler
{
    public async Task<string> GetSiteId(string siteName)
    {
        var command = new GetSiteIdCommand(siteName);
        await repository.ExecuteAsync(command);

        return command.SiteId;
    }

    public Task<SiteDBConfig> GetBySiteDBGroupId(int siteDBGroupId)
        => repository.GetAsync<SiteDBConfig>(new GetCustomerSiteDBCriteria(siteDBGroupId));

    public Task<SiteDBConfig> GetBySite(string site)
        => repository.GetAsync<SiteDBConfig>(new GetSiteDBConfigCriteria(site));

    public Task<SiteDBConfig> GetByCustId(int custId)
        => repository.GetAsync<SiteDBConfig>(new GetSiteDBConfigByCustCriteria(custId));
}