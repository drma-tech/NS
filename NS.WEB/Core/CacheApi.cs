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

public class CacheGoogleNewsApi(IHttpClientFactory http) : ApiCosmos<NewsCache>(http, ApiType.Anonymous, null, [], ApiContext.Default.NewsCache)
{
    public async Task<NewsCache?> GetNewsRegion(string region, string mode, ComponentActions<NewsCache>? actions, CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.NewsRegion(region, mode), false, actions, cancellationToken);
    }
}

public class CacheNewsApi(IHttpClientFactory http) : ApiCosmos<NewsCache>(http, ApiType.Anonymous, null, [], ApiContext.Default.NewsCache)
{
    public async Task<NewsCache?> GetNewsTopic(string topic, string mode, ComponentActions<NewsCache>? actions, CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.NewsTopic(topic, mode), false, actions, cancellationToken);
    }
}

public class CacheWeatherApi(IHttpClientFactory http) : ApiCosmos<WeatherCache>(http, ApiType.Anonymous, null, [], ApiContext.Default.WeatherCache)
{
    public async Task<WeatherCache?> GetWeather(string? city, string? mode, ComponentActions<WeatherCache>? actions, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(mode);

        return await GetAsync(Endpoint.Weather(city, mode), false, actions, cancellationToken);
    }
}

public class CacheHolidayApi(IHttpClientFactory http) : ApiCosmos<HolidayCache>(http, ApiType.Anonymous, null, [], ApiContext.Default.HolidayCache)
{
    public async Task<HolidayCache?> GetHoliday(string? region, ComponentActions<HolidayCache>? actions, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(region);

        return await GetAsync(Endpoint.Holiday(region), false, actions, cancellationToken);
    }
}

public class GlobalConflictsApi(IHttpClientFactory http) : ApiCosmos<GlobalConflictsCache>(http, ApiType.Anonymous, null, [], ApiContext.Default.GlobalConflictsCache)
{
    public async Task<GlobalConflictsCache?> GetConflicts(ComponentActions<GlobalConflictsCache> actions, CancellationToken cancellationToken)
    {
        return await GetAsync(Endpoint.Conflicts(), false, actions, cancellationToken);
    }
}