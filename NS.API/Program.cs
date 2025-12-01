using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NS.API.Core.Auth;
using System.Text.Json;
using System.Text.RegularExpressions;

try
{
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

        var key = ApiStartup.Configurations.Firebase?.PrivateKey ?? throw new NotificationException("PrivateKey null");

        var firebaseConfig = new FirebaseConfig
        {
            project_id = "streaming-discovery-4c483",
            private_key_id = ApiStartup.Configurations.Firebase?.PrivateKeyId ?? throw new NotificationException("PrivateKeyId null"),
            private_key = Regex.Unescape(key),
            client_email = ApiStartup.Configurations.Firebase?.ClientEmail ?? throw new NotificationException("ClientEmail null"),
            client_id = ApiStartup.Configurations.Firebase?.ClientId ?? throw new NotificationException("ClientId null"),
            client_x509_cert_url = ApiStartup.Configurations.Firebase?.CertUrl ?? throw new NotificationException("Firebase null")
        };

        var firebaseJson = JsonSerializer.Serialize(firebaseConfig);

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(firebaseJson)
            });
        }
    })
    .ConfigureServices(ConfigureServices)
    .Build();

    await app.RunAsync();
}
catch (Exception ex)
{
    var tempClient = new CosmosClient(ApiStartup.Configurations.CosmosDB?.ConnectionString, new CosmosClientOptions()
    {
        SerializerOptions = new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        }
    });
    var tempRepo = new CosmosLogRepository(tempClient);
    var provider = new CosmosLoggerProvider(tempRepo);
    var loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(provider));
    var logger = loggerFactory.CreateLogger("ConfigureAppConfiguration");

    logger.LogError(ex, "ConfigureAppConfiguration");
}

static void ConfigureServices(IServiceCollection services)
{
    try
    {
        //http clients

        services.AddHttpClient("paddle");
        services.AddHttpClient("apple");
        services.AddHttpClient("auth", client => { client.Timeout = TimeSpan.FromSeconds(60); });
        services.AddHttpClient("ipinfo");
        services.AddHttpClient("generic");

        //repositories

        services.AddSingleton(provider =>
        {
            return new CosmosClient(ApiStartup.Configurations.CosmosDB?.ConnectionString, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });
        });

        services.AddSingleton<CosmosRepository>();
        services.AddSingleton<CosmosGroupRepository>();
        services.AddSingleton<CosmosCacheRepository>();
        services.AddSingleton<CosmosLogRepository>();

        services.AddSingleton<ILoggerProvider>(provider =>
        {
            var repo = provider.GetRequiredService<CosmosLogRepository>();
            return new CosmosLoggerProvider(repo);
        });

        //general services

        services.AddDistributedMemoryCache();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://securetoken.google.com/streaming-discovery-4c483";
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/streaming-discovery-4c483",
                    ValidateAudience = true,
                    ValidAudience = "streaming-discovery-4c483",
                    ValidateLifetime = true
                };
            });
    }
    catch (Exception ex)
    {
        var tempClient = new CosmosClient(ApiStartup.Configurations.CosmosDB?.ConnectionString, new CosmosClientOptions()
        {
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        });
        var tempRepo = new CosmosLogRepository(tempClient);
        var provider = new CosmosLoggerProvider(tempRepo);
        var loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(provider));
        var logger = loggerFactory.CreateLogger("ConfigureServices");

        logger.LogWarning($"PrivateKey: {ApiStartup.Configurations.Firebase?.PrivateKey}", "ConfigureServices");
        logger.LogError(ex, "ConfigureServices");
    }
}