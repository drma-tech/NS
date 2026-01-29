using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Distributed;
using NS.API.Core.Auth;
using NS.Shared.Models.Auth;
using NS.Shared.Models.Energy;
using NS.Shared.Models.News;
using NS.Shared.Models.Weather;
using System.Net;
using System.Text.Json;

namespace NS.API.Functions;

public class CacheFunction(CosmosCacheRepository cacheRepo, CosmosRepository repo, IDistributedCache distributedCache, IHttpClientFactory factory)
{
    [Function("Energy")]
    public async Task<HttpResponseData?> Energy(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/cache/energy")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var ip = req.GetUserIP(false);
            var cacheKey = $"energy_{DateTime.UtcNow.Day}_{ip}";

            var doc = await cacheRepo.Get<EnergyModel>(cacheKey, cancellationToken);
            var model = doc?.Data;

            model ??= new EnergyModel() { ConsumedEnergy = 0, TotalEnergy = 10 };

            doc = await cacheRepo.UpsertItemAsync(new EnergyCache(model, cacheKey), cancellationToken); //check if upsert is needed
            await SaveCache(doc, cacheKey, TtlCache.OneWeek);

            return await req.CreateResponse(doc, TtlCache.OneWeek, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            req.LogError(ex.CancellationToken.IsCancellationRequested
                ? new NotificationException("Cancellation Requested")
                : new NotificationException("Timeout occurred"));

            return req.CreateResponse(HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            return await req.CreateResponse<CacheDocument<EnergyModel>>(null, TtlCache.OneWeek, cancellationToken);
        }
    }

    [Function("EnergyAuth")]
    public async Task<HttpResponseData?> EnergyAuth(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "cache/energy")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var ip = req.GetUserIP(false);
            var userId = await req.GetUserIdAsync(cancellationToken);

            var cacheKey = $"energy_auth_{DateTime.UtcNow.Day}_{ip}";

            var doc = await cacheRepo.Get<EnergyModel>(cacheKey, cancellationToken);
            var model = doc?.Data;

            model ??= new EnergyModel() { ConsumedEnergy = 0, TotalEnergy = 10 };

            var principal = await repo.Get<AuthPrincipal>(DocumentType.Principal, userId, cancellationToken);

            if (principal?.GetActiveSubscription() != null)
            {
                model.TotalEnergy = principal.GetActiveSubscription()!.ActiveProduct.GetRestrictions().Energy;
            }

            doc = await cacheRepo.UpsertItemAsync(new EnergyCache(model, cacheKey), cancellationToken); //todo: check if upsert is needed
            await SaveCache(doc, cacheKey, TtlCache.OneWeek);

            return await req.CreateResponse(doc, TtlCache.OneWeek, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            req.LogError(ex.CancellationToken.IsCancellationRequested
                ? new NotificationException("Cancellation Requested")
                : new NotificationException("Timeout occurred"));

            return req.CreateResponse(HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            return await req.CreateResponse<CacheDocument<EnergyModel>>(null, TtlCache.OneWeek, cancellationToken);
        }
    }

    [Function("EnergyAdd")]
    public async Task EnergyAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "public/cache/energy/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var ip = req.GetUserIP(false);
            var cacheKey = $"energy_{DateTime.UtcNow.Day}_{ip}";

            var doc = await cacheRepo.Get<EnergyModel>(cacheKey, cancellationToken);

            if (doc == null)
            {
                var model = new EnergyModel() { ConsumedEnergy = 1, TotalEnergy = 10 };

                doc = await cacheRepo.UpsertItemAsync(new EnergyCache(model, cacheKey), cancellationToken);
            }
            else
            {
                doc.Data!.ConsumedEnergy += 1;
            }

            await cacheRepo.UpsertItemAsync(doc!, cancellationToken);
            await SaveCache(doc, cacheKey, TtlCache.OneWeek);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
        }
    }

    [Function("EnergyAuthAdd")]
    public async Task EnergyAuthAdd(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "cache/energy/add")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var ip = req.GetUserIP(false);
            var userId = await req.GetUserIdAsync(cancellationToken);

            var cacheKey = $"energy_auth_{DateTime.UtcNow.Day}_{ip}";
            var doc = await cacheRepo.Get<EnergyModel>(cacheKey, cancellationToken);

            if (doc == null)
            {
                var model = new EnergyModel() { ConsumedEnergy = 1, TotalEnergy = 10 };
                var principal = await repo.Get<AuthPrincipal>(DocumentType.Principal, userId, cancellationToken);

                if (principal?.GetActiveSubscription() != null)
                {
                    model!.TotalEnergy = principal.GetActiveSubscription()!.ActiveProduct.GetRestrictions().Energy;
                }

                doc = await cacheRepo.UpsertItemAsync(new EnergyCache(model, cacheKey), cancellationToken);
            }
            else
            {
                doc.Data!.ConsumedEnergy += 1;
            }

            await cacheRepo.UpsertItemAsync(doc!, cancellationToken);
            await SaveCache(doc, cacheKey, TtlCache.OneWeek);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
        }
    }

    [Function("CacheNewRegion")]
    public async Task<HttpResponseData?> CacheNewRegion([HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/cache/news/region/{region}/{mode}")]
        HttpRequestData req, string region, string mode, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"lastnews_{region}_{mode}";
            CacheDocument<NewsModel>? doc;
            var cachedBytes = await distributedCache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes is { Length: > 0 })
            {
                doc = JsonSerializer.Deserialize<CacheDocument<NewsModel>>(cachedBytes);
            }
            else
            {
                doc = await cacheRepo.Get<NewsModel>(cacheKey, cancellationToken);

                if (doc == null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "regions.json");
                    var jsonContent = await File.ReadAllTextAsync(path, cancellationToken);
                    var regions = JsonSerializer.Deserialize<AllRegions>(jsonContent);
                    var objRegion = regions?.GetByCode(region);

                    var client = factory.CreateClient("rapidapi");
                    var obj = await client.GetNewsByGoogleNews<GoogleNews>(objRegion?.name, cancellationToken);

                    if (mode == "compact")
                    {
                        var compactModels = new NewsModel();

                        foreach (var item in obj?.data.Take(10) ?? [])
                        {
                            compactModels.Items.Add(new Item(Guid.NewGuid().ToString(), item.title, item.description, item.thumbnail, item.url, item.date));
                        }

                        doc = await cacheRepo.UpsertItemAsync(new NewsCache(compactModels, $"lastnews_{region}_compact"), cancellationToken);
                    }
                    else
                    {
                        var fullModels = new NewsModel();

                        foreach (var item in obj?.data ?? [])
                        {
                            fullModels.Items.Add(new Item(Guid.NewGuid().ToString(), item.title, item.description, item.thumbnail, item.url, item.date));
                        }

                        doc = await cacheRepo.UpsertItemAsync(new NewsCache(fullModels, $"lastnews_{region}_full"), cancellationToken);
                    }
                }

                await SaveCache(doc, cacheKey, TtlCache.OneWeek);
            }

            return await req.CreateResponse(doc, TtlCache.OneWeek, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            req.LogError(ex.CancellationToken.IsCancellationRequested
                ? new NotificationException("Cancellation Requested")
                : new NotificationException("Timeout occurred"));

            return req.CreateResponse(HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("CacheNewTopic")]
    public async Task<HttpResponseData?> CacheNewTopic([HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/cache/news/topic/{topic}/{mode}")]
        HttpRequestData req, string topic, string mode, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"lastnews_{topic}_{mode}";
            CacheDocument<NewsModel>? doc;
            var cachedBytes = await distributedCache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes is { Length: > 0 })
            {
                doc = JsonSerializer.Deserialize<CacheDocument<NewsModel>>(cachedBytes);
            }
            else
            {
                doc = await cacheRepo.Get<NewsModel>(cacheKey, cancellationToken);

                if (doc == null)
                {
                    var client = factory.CreateClient("rapidapi");
                    var obj = await client.GetNewsByNewsAPI<TopicNews>(topic, cancellationToken);

                    if (mode == "compact")
                    {
                        var compactModels = new NewsModel();

                        foreach (var item in obj?.data?.Take(10) ?? [])
                        {
                            compactModels.Items.Add(new Item(Guid.NewGuid().ToString(), item.title, item.excerpt, item.thumbnail, item.url, item.date));
                        }

                        doc = await cacheRepo.UpsertItemAsync(new NewsCache(compactModels, $"lastnews_{topic}_compact"), cancellationToken);
                    }
                    else
                    {
                        var fullModels = new NewsModel();

                        foreach (var item in obj?.data ?? [])
                        {
                            fullModels.Items.Add(new Item(Guid.NewGuid().ToString(), item.title, item.excerpt, item.thumbnail, item.url, item.date));
                        }

                        doc = await cacheRepo.UpsertItemAsync(new NewsCache(fullModels, $"lastnews_{topic}_full"), cancellationToken);
                    }
                }

                await SaveCache(doc, cacheKey, TtlCache.OneWeek);
            }

            return await req.CreateResponse(doc, TtlCache.OneWeek, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            req.LogError(ex.CancellationToken.IsCancellationRequested
                ? new NotificationException("Cancellation Requested")
                : new NotificationException("Timeout occurred"));

            return req.CreateResponse(HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("CacheWeather")]
    public async Task<HttpResponseData?> CacheWeather([HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/cache/weather/{city}/{mode}")]
        HttpRequestData req, string city, string mode, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"lastweather_{city}_{mode}";
            CacheDocument<WeatherModel>? doc;
            var cachedBytes = await distributedCache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes is { Length: > 0 })
            {
                doc = JsonSerializer.Deserialize<CacheDocument<WeatherModel>>(cachedBytes);
            }
            else
            {
                doc = await cacheRepo.Get<WeatherModel>(cacheKey, cancellationToken);

                if (doc == null)
                {
                    //var countries = EnumHelper.GetListCountry<Country>();
                    //var country = countries!.Single(f => f.Value.ToString().Equals(region, StringComparison.OrdinalIgnoreCase));

                    var now = DateTime.Now;
                    var today = now.ToString("yyyy-MM-dd");
                    var month1 = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 15).ToString("yyyy-MM-dd");
                    var month2 = new DateTime(now.AddMonths(2).Year, now.AddMonths(2).Month, 15).ToString("yyyy-MM-dd");

                    var client = factory.CreateClient("rapidapi");
                    var objToday = await client.GetWeatherByWeatherApi<WeatherApi>("forecast", city, today, cancellationToken);
                    var objMonth1 = await client.GetWeatherByWeatherApi<WeatherApi>("future", city, month1, cancellationToken);
                    var objMonth2 = await client.GetWeatherByWeatherApi<WeatherApi>("future", city, month2, cancellationToken);

                    var current = objToday?.current;
                    var forecast1 = objMonth1?.forecast?.forecastday?[0];
                    var forecast2 = objMonth2?.forecast?.forecastday?[0];

                    if (mode == "compact")
                    {
                        var compactModels = new WeatherModel
                        {
                            Current = new MonthlyWeather()
                            {
                                temp_c = current?.temp_c,
                                temp_f = current?.temp_f,
                                feels_like_c = current?.feelslike_c,
                                feels_like_f = current?.feelslike_f,
                                condition_text = current?.condition?.text,
                                condition_icon = current?.condition?.icon,
                            },
                            Month1 = new MonthlyWeather()
                            {
                                temp_c = forecast1?.day?.avgtemp_c,
                                temp_f = forecast1?.day?.avgtemp_f,
                                feels_like_c = (forecast1?.day?.maxtemp_c + forecast1?.day?.mintemp_c) / 2,
                                feels_like_f = (forecast1?.day?.maxtemp_f + forecast1?.day?.mintemp_f) / 2,
                                condition_text = forecast1?.day?.condition?.text,
                                condition_icon = forecast1?.day?.condition?.icon,
                            },
                            Month2 = new MonthlyWeather()
                            {
                                temp_c = forecast2?.day?.avgtemp_c,
                                temp_f = forecast2?.day?.avgtemp_f,
                                feels_like_c = (forecast2?.day?.maxtemp_c + forecast2?.day?.mintemp_c) / 2,
                                feels_like_f = (forecast2?.day?.maxtemp_f + forecast2?.day?.mintemp_f) / 2,
                                condition_text = forecast2?.day?.condition?.text,
                                condition_icon = forecast2?.day?.condition?.icon,
                            }
                        };

                        doc = await cacheRepo.UpsertItemAsync(new WeatherCache(compactModels, $"lastweather_{city}_compact"), cancellationToken);
                    }
                    else
                    {
                        //var fullModels = new WeatherModel
                        //{
                        //    Current = new MonthlyWeather()
                        //    {
                        //        temp_c = obj?.current?.temp_c,
                        //        cloud = obj?.current?.cloud
                        //    }
                        //};

                        //doc = await cacheRepo.UpsertItemAsync(new WeatherCache(fullModels, $"lastweather_{city}_full"), cancellationToken);
                    }
                }

                await SaveCache(doc, cacheKey, TtlCache.OneWeek);
            }

            return await req.CreateResponse(doc, TtlCache.OneWeek, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            req.LogError(ex.CancellationToken.IsCancellationRequested
                ? new NotificationException("Cancellation Requested")
                : new NotificationException("Timeout occurred"));

            return req.CreateResponse(HttpStatusCode.RequestTimeout);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    private async Task SaveCache<TData>(CacheDocument<TData>? doc, string cacheKey, TtlCache ttl) where TData : class, new()
    {
        if (doc != null)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(doc);
            await distributedCache.SetAsync(cacheKey, bytes, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds((int)ttl) });
        }
    }
}