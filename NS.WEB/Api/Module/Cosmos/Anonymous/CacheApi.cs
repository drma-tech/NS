using NS.Shared.Models.GlobalConflicts;
using NS.Shared.Models.Holiday;
using NS.Shared.Models.News;
using NS.Shared.Models.Weather;
using NS.WEB.Api.Core;

namespace NS.WEB.Api.Module.Cosmos.Anonymous;

public class CacheGoogleNewsApi(IHttpClientFactory factory) : ApiCosmos<NewsCache>(factory, ApiType.Anonymous, null, [], ApiContext.Default.NewsCache)
{
    public async Task<NewsCache?> GetNewsRegion(string region, string mode, RenderControlState<NewsCache?>[] states, CancellationToken cancellationToken)
    {
        return await GetAsync($"public/cache/news/region/{region}/{mode}", setNewVersion: false, states, cancellationToken);
    }
}

public class CacheNewsApi(IHttpClientFactory factory) : ApiCosmos<NewsCache>(factory, ApiType.Anonymous, null, [], ApiContext.Default.NewsCache)
{
    public async Task<NewsCache?> GetNewsTopic(string topic, string mode, RenderControlState<NewsCache?>[] states, CancellationToken cancellationToken)
    {
        return await GetAsync($"public/cache/news/topic/{topic}/{mode}", setNewVersion: false, states, cancellationToken);
    }
}

public class CacheWeatherApi(IHttpClientFactory factory) : ApiCosmos<WeatherCache>(factory, ApiType.Anonymous, null, [], ApiContext.Default.WeatherCache)
{
    public async Task<WeatherCache?> GetWeather(string? city, string? mode, RenderControlState<WeatherCache?>[] states, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(mode);

        return await GetAsync($"public/cache/weather/{city}/{mode}", setNewVersion: false, states, cancellationToken);
    }
}

public class CacheHolidayApi(IHttpClientFactory factory) : ApiCosmos<HolidayCache>(factory, ApiType.Anonymous, null, [], ApiContext.Default.HolidayCache)
{
    public async Task<HolidayCache?> GetHoliday(string? region, RenderControlState<HolidayCache?>[] states, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(region);

        return await GetAsync($"public/cache/holiday/{region}", setNewVersion: false, states, cancellationToken);
    }
}

public class GlobalConflictsApi(IHttpClientFactory factory) : ApiCosmos<GlobalConflictsCache>(factory, ApiType.Anonymous, null, [], ApiContext.Default.GlobalConflictsCache)
{
    public async Task<GlobalConflictsCache?> GetConflicts(RenderControlState<GlobalConflictsCache?>[] states, CancellationToken cancellationToken)
    {
        return await GetAsync($"public/cache/global-conflicts", setNewVersion: false, states, cancellationToken);
    }
}