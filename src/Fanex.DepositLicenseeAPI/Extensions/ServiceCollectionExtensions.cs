namespace Fanex.DepositLicenseeAPI.Extensions;

using Data.Repository;
using Data.Vault;
using Features.Accounts;
using Features.Licensees;
using Features.Shared.SiteDB;
using Features.UserLog;
using Logging;
using Logging.Sentry;
using Microsoft.AspNetCore.HttpLogging;
using Sentry;
using Serilog;
using ILogger = Logging.ILogger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        // Sentry
        LogManager
            .SetDefaultLogCategory(configuration["Sentry:Environment"])
            .Use(new List<ILogEngine>
            {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(configuration["Sentry:Dsn"]),
                    Environment = configuration["Sentry:Environment"],
                    MaxBreadcrumbs = int.TryParse(configuration["sentry:MaxBreadcrumbs"], out int maxBreadcrumbs) ? maxBreadcrumbs : 50,
                    SendDefaultPii = bool.TryParse(configuration["Sentry:SendDefaultPii"], out bool sendDefaultPii) && sendDefaultPii
                })
            });

        services.AddSingleton<ILogger, Logger>();

        // Serilog
        services.AddSerilog((s, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(s)
            .Enrich.FromLogContext()
            .Filter.ByExcluding("RequestPath like '/health%'")
            .WriteTo.Console()
            .Enrich.WithProperty("Team", "Subarashi")
            .Enrich.WithProperty("Application", "Deposit Licensee API")
            .Enrich.WithProperty("Environment", configuration["Environment"]));

        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
            logging.RequestHeaders.Add("sec-ch-ua");
            logging.RequestHeaders.Add("x-forwarded-for");
            logging.MediaTypeOptions.AddText("application/javascript");
            //logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
            logging.CombineLogs = true;
        });

        return services;
    }

    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IUserLogHandler, UserLogHandler>();
        services.AddSingleton<ISiteDBHandler, SiteDBHandler>();
        services.AddSingleton<ILicenseesHandler, LicenseesHandler>();
        services.AddSingleton<ICurrencyHandler, CurrencyHandler>();
        services.AddSingleton<IAccountsHandler, AccountsHandler>();
        services.AddSingleton<VaultDbAliasService, VaultDbAliasServiceImp>();

        return services;
    }
}