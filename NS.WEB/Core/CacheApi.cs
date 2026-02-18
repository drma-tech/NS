using NS.Shared.Models.News;
using NS.Shared.Models.Weather;

namespace NS.WEB.Core;

public struct Endpoint
{
    public static string NewsRegion(string region, string mode)
    {
        return $"public/cache/news/region/{region}/{mode}";
    }

    public static string NewsTopic(string topic, string mode)
    {
        return $"public/cache/news/topic/{topic}/{mode}";
    }

    public static string Weather(string city, string mode)
    {
        return $"public/cache/weather/{city}/{mode}";
    }
}

public class CacheGoogleNewsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<NewsModel>>(http, ApiType.Anonymous, null)
{
    public async Task<CacheDocument<NewsModel>?> GetNewsRegion(string region, string mode)
    {
        return await GetAsync(Endpoint.NewsRegion(region, mode));
    }
}

public class CacheNewsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<NewsModel>>(http, ApiType.Anonymous, null)
{
    public async Task<CacheDocument<NewsModel>?> GetNewsTopic(string region, string mode)
    {
        return await GetAsync(Endpoint.NewsTopic(region, mode));
    }
}

public class CacheWeatherApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<WeatherModel>>(http, ApiType.Anonymous, null)
{
    public async Task<CacheDocument<WeatherModel>?> GetWeather(string? city, string? mode)
    {
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(mode);

        return await GetAsync(Endpoint.Weather(city, mode));
    }
}