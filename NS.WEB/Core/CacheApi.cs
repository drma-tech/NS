using NS.Shared.Models.Energy;
using NS.Shared.Models.News;
using NS.Shared.Models.Weather;

namespace NS.WEB.Core;

public struct Endpoint
{
    public const string Energy = "public/cache/energy";
    public const string EnergyAuth = "cache/energy";
    public const string EnergyAdd = "public/cache/energy/add";
    public const string EnergyAuthAdd = "cache/energy/add";

    public static string News(string region, string mode)
    {
        return $"public/cache/news/{region}/{mode}";
    }

    public static string Weather(string city, string mode)
    {
        return $"public/cache/weather/{city}/{mode}";
    }
}

public class EnergyApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<EnergyModel>>(http, ApiType.Anonymous, "energy")
{
    public async Task<CacheDocument<EnergyModel>?> GetEnergy()
    {
        return await GetAsync(Endpoint.Energy, true);
    }

    public async Task AddEnergy()
    {
        await PostAsync(Endpoint.EnergyAdd, null);
    }
}

public class EnergyAuthApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<EnergyModel>>(http, ApiType.Authenticated, "energy-auth")
{
    public async Task<CacheDocument<EnergyModel>?> GetEnergy()
    {
        return await GetAsync(Endpoint.EnergyAuth, true);
    }

    public async Task AddEnergy()
    {
        await PostAsync(Endpoint.EnergyAuthAdd, null);
    }
}

public class CacheGoogleNewsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<NewsModel>>(http, ApiType.Anonymous, null)
{
    public async Task<CacheDocument<NewsModel>?> GetNews(string region, string mode)
    {
        return await GetAsync(Endpoint.News(region, mode));
    }
}

public class CacheWeatherApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<WeatherModel>>(http, ApiType.Anonymous, null)
{
    public async Task<CacheDocument<WeatherModel>?> GetWeather(string city, string mode)
    {
        return await GetAsync(Endpoint.Weather(city, mode));
    }
}