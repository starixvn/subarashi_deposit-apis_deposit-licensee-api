using System.Net.Mime;
using Fanex.Data.SqlServer;
using Fanex.DepositLicenseeAPI;
using Fanex.DepositLicenseeAPI.Extensions;
using Fanex.DepositLicenseeAPI.Features.Accounts;
using Fanex.DepositLicenseeAPI.Features.Licensees;
using Fanex.DepositLicenseeAPI.HealthChecks;
using Fanex.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services
        .AddLogging(builder.Configuration)
        .AddFanexSql(builder.Configuration)
        .AddDependencyInjection(builder.Configuration)
        .AddExceptionHandler<GlobalExceptionHandler>()
        .AddProblemDetails()
        .AddHttpContextAccessor();

    builder.Services
        .AddHealthChecks()
        .AddCheck<ServerHealthCheck>("Server")
        .AddCheck<VaultHealthCheck>("Vault");

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Deposit Licensee API", Version = "v1" }); });

    var app = builder.Build();
    LogService.Info("Application is running ...");
    app.UseExceptionHandler();

    app.UseSerilogRequestLogging();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("v1/swagger.json", "Deposit Licensee API V1");
    });

    app.MapGet("/", () => "Hello World!").ExcludeFromDescription();
    app.MapHealthChecks("/healthz");
    app.UseHealthChecks("/whoami", new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = async (context, report) =>
        {
            var result = JsonConvert.SerializeObject(new
            {
                status = report.Status.ToString(),
                monitors = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = Enum.GetName(typeof(HealthStatus), e.Value.Status),
                    description = e.Value.Description,
                    values = e.Value.Data
                })
            }, Formatting.Indented);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

    app.RegisterLicenseesEndpoints();
    app.RegisterAccountsEndpoints();

    if (!app.Environment.IsDevelopment())
    {
        builder.WebHost.UseUrls("http://0.0.0.0:5000");
    }

    app.Run();
}
catch (Exception ex)
{
    LogService.Critical("Application terminated unexpectedly", ex);
}