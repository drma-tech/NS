using NS.Shared.Models.Energy;
using NS.Shared.Models.News;
using NS.WEB.Shared;

namespace NS.WEB.Core;

public struct Endpoint
{
    public const string Energy = "public/cache/energy";
    public const string EnergyAuth = "cache/energy";
    public const string EnergyAdd = "public/cache/energy/add";
    public const string EnergyAuthAdd = "cache/energy/add";

    public static string News(string region, string mode)
    {
        return $"public/cache/news?region={region}&mode={mode}";
    }
}

public class EnergyApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<EnergyModel>>(http, ApiType.Anonymous, "energy")
{
    public async Task<CacheDocument<EnergyModel>?> GetEnergy()
    {
        return await GetAsync(Endpoint.Energy, null, true);
    }

    public async Task AddEnergy()
    {
        await PostAsync(Endpoint.EnergyAdd, null, null);
    }
}

public class EnergyAuthApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<EnergyModel>>(http, ApiType.Authenticated, "energy-auth")
{
    public async Task<CacheDocument<EnergyModel>?> GetEnergy()
    {
        return await GetAsync(Endpoint.EnergyAuth, null, true);
    }

    public async Task AddEnergy()
    {
        await PostAsync(Endpoint.EnergyAuthAdd, null, null);
    }
}

public class CacheGoogleNewsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<NewsModel>>(http, ApiType.Anonymous, null)
{
    public async Task<CacheDocument<NewsModel>?> GetNews(string region, string mode, RenderControlCore<CacheDocument<NewsModel>?>? core)
    {
        return await GetAsync(Endpoint.News(region, mode), core);
    }
}