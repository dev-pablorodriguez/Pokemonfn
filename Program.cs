using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

/*
 * The Functions host and the isolated process worker have separate configuration for log levels, etc.
 * Any Application Insights configuration in host.json will not affect the logging from the worker, and similarly,
 * configuration made in your worker code will not impact logging from the host. You need to apply changes in both
 * places if your scenario requires customization at both layers.
 * More info: https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows
 * 
 * */
builder.Logging.Services.Configure<LoggerFilterOptions>(options => {
    LoggerFilterRule defaultRule = options.Rules.FirstOrDefault(
        rule => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");

    if (defaultRule is not null) options.Rules.Remove(defaultRule);
});

builder.Build().Run();
