using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Distributed;
using NS.API.Core.Auth;
using NS.Shared.Models.Auth;
using NS.Shared.Models.Energy;
using NS.Shared.Models.News;
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
            await SaveCache(doc, cacheKey, TtlCache.OneDay);

            return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
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
            return await req.CreateResponse<CacheDocument<EnergyModel>>(null, TtlCache.OneDay, cancellationToken);
        }
    }

    [Function("EnergyAuth")]
    public async Task<HttpResponseData?> EnergyAuth(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "cache/energy")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var ip = req.GetUserIP(false);
            var userId = await req.GetUserIdAsync(factory, cancellationToken);

            var cacheKey = $"energy_auth_{DateTime.UtcNow.Day}_{ip}";

            var doc = await cacheRepo.Get<EnergyModel>(cacheKey, cancellationToken);
            var model = doc?.Data;

            model ??= new EnergyModel() { ConsumedEnergy = 0, TotalEnergy = 10 };

            var principal = await repo.Get<AuthPrincipal>(DocumentType.Principal, userId, cancellationToken);

            if (principal?.Subscription != null)
            {
                model.TotalEnergy = principal.Subscription.ActiveProduct.GetRestrictions().Energy;
            }

            doc = await cacheRepo.UpsertItemAsync(new EnergyCache(model, cacheKey), cancellationToken); //todo: check if upsert is needed
            await SaveCache(doc, cacheKey, TtlCache.OneDay);

            return await req.CreateResponse(doc, TtlCache.OneDay, cancellationToken);
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
            return await req.CreateResponse<CacheDocument<EnergyModel>>(null, TtlCache.OneDay, cancellationToken);
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
            await SaveCache(doc, cacheKey, TtlCache.OneDay);
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
            var userId = await req.GetUserIdAsync(factory, cancellationToken);

            var cacheKey = $"energy_auth_{DateTime.UtcNow.Day}_{ip}";
            var doc = await cacheRepo.Get<EnergyModel>(cacheKey, cancellationToken);

            if (doc == null)
            {
                var model = new EnergyModel() { ConsumedEnergy = 1, TotalEnergy = 10 };
                var principal = await repo.Get<AuthPrincipal>(DocumentType.Principal, userId, cancellationToken);

                if (principal?.Subscription != null)
                {
                    model!.TotalEnergy = principal.Subscription.ActiveProduct.GetRestrictions().Energy;
                }

                doc = await cacheRepo.UpsertItemAsync(new EnergyCache(model, cacheKey), cancellationToken);
            }
            else
            {
                doc.Data!.ConsumedEnergy += 1;
            }

            await cacheRepo.UpsertItemAsync(doc!, cancellationToken);
            await SaveCache(doc, cacheKey, TtlCache.OneDay);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
        }
    }

    [Function("CacheNew")]
    public async Task<HttpResponseData?> CacheNew([HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/cache/news")]
        HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            req.LogWarning("Cache news requested");

            var code = req.GetQueryParameters()["code"];
            var mode = req.GetQueryParameters()["mode"];
            var cacheKey = $"lastnews_{code}_{mode}";
            CacheDocument<NewsModel>? doc;
            var cachedBytes = await distributedCache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes is { Length: > 0 })
            {
                req.LogWarning("Cache news found in distributed cache");
                doc = JsonSerializer.Deserialize<CacheDocument<NewsModel>>(cachedBytes);
            }
            else
            {
                doc = await cacheRepo.Get<NewsModel>(cacheKey, cancellationToken);
                req.LogWarning("Cache news found in cosmos cache");

                if (doc == null)
                {
                    var countries = EnumHelper.GetListCountry<Country>();
                    var country = countries!.Single(f => f.Value.ToString().Equals(code, StringComparison.OrdinalIgnoreCase));

                    var client = factory.CreateClient("rapidapi");
                    var obj = await client.GetNewsByGoogleNews<GoogleNews>(country.Name, cancellationToken);
                    req.LogWarning("Cache news fetched from external API");

                    if (mode == "compact")
                    {
                        var compactModels = new NewsModel();

                        foreach (var item in obj?.data.Take(10) ?? [])
                        {
                            compactModels.Items.Add(new Item(Guid.NewGuid().ToString(), item.title, item.description, item.thumbnail, item.url, item.date));
                        }

                        doc = await cacheRepo.UpsertItemAsync(new NewsCache(compactModels, $"lastnews_{code}_compact"), cancellationToken);
                    }
                    else
                    {
                        var fullModels = new NewsModel();

                        foreach (var item in obj?.data ?? [])
                        {
                            fullModels.Items.Add(new Item(Guid.NewGuid().ToString(), item.title, item.description, item.thumbnail, item.url, item.date));
                        }

                        doc = await cacheRepo.UpsertItemAsync(new NewsCache(fullModels, $"lastnews_{code}_full"), cancellationToken);
                    }
                }

                await SaveCache(doc, cacheKey, TtlCache.OneWeek);
                req.LogWarning("Cache news saved to distributed cache");
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
            return await req.CreateResponse<CacheDocument<NewsModel>>(null, TtlCache.SixHours, cancellationToken);
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
