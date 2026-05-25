using NS.Shared.Models.GlobalConflicts;
using NS.Shared.Models.Holiday;
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

    public static string Holiday(string region)
    {
        return $"public/cache/holiday/{region}";
    }

    public static string Conflicts()
    {
        return $"public/cache/global-conflicts";
    }
}

public class CacheGoogleNewsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<NewsModel>>(http, ApiType.Anonymous, null, ApiContext.Default.CacheDocumentNewsModel)
{
    public async Task<CacheDocument<NewsModel>?> GetNewsRegion(string region, string mode, CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.NewsRegion(region, mode), false, cancellationToken);
    }
}

public class CacheNewsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<NewsModel>>(http, ApiType.Anonymous, null, ApiContext.Default.CacheDocumentNewsModel)
{
    public async Task<CacheDocument<NewsModel>?> GetNewsTopic(string topic, string mode, CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.NewsTopic(topic, mode), false, cancellationToken);
    }
}

public class CacheWeatherApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<WeatherModel>>(http, ApiType.Anonymous, null, ApiContext.Default.CacheDocumentWeatherModel)
{
    public async Task<CacheDocument<WeatherModel>?> GetWeather(string? city, string? mode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(mode);

        return await GetAsync(Endpoint.Weather(city, mode), false, cancellationToken);
    }
}

public class CacheHolidayApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<HolidayModel>>(http, ApiType.Anonymous, null, ApiContext.Default.CacheDocumentHolidayModel)
{
    public async Task<CacheDocument<HolidayModel>?> GetHoliday(string? region, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(region);

        return await GetAsync(Endpoint.Holiday(region), false, cancellationToken);
    }
}

public class GlobalConflictsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<GlobalConflicts>>(http, ApiType.Anonymous, null, ApiContext.Default.CacheDocumentGlobalConflicts)
{
    public async Task<CacheDocument<GlobalConflicts>?> GetConflicts(CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.Conflicts(), false, cancellationToken);
    }
}