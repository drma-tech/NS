using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var app = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<ApiMiddleware>();
    })
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            config.AddJsonFile("local.settings.json");
            config.AddUserSecrets<Program>();
        }

        var cfg = new Configurations();
        config.Build().Bind(cfg);
        ApiStartup.Configurations = cfg;

        ApiStartup.Startup(ApiStartup.Configurations.CosmosDB?.ConnectionString);
    })
    .ConfigureLogging(ConfigureLogging)
    .ConfigureServices(ConfigureServices)
    .Build();

await app.RunAsync();

return;

static void ConfigureLogging(ILoggingBuilder builder)
{
    builder.AddProvider(new CosmosLoggerProvider(new CosmosLogRepository()));
}

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddHttpClient("paddle");
    services.AddHttpClient("generic");

    services.AddSingleton<CosmosRepository>();
    services.AddSingleton<CosmosGroupRepository>();
    services.AddSingleton<CosmosCacheRepository>();
    services.AddDistributedMemoryCache();
}
