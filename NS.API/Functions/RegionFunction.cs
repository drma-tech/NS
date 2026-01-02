using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Distributed;
using NS.Shared.Models.Country;
using System.Text.Json;

namespace NS.API.Functions;

public class RegionFunction(CosmosGroupRepository repo, IDistributedCache distributedCache)
{
    [Function("RegionGet")]
    public async Task<HttpResponseData?> RegionGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/region/get/{region}")] HttpRequestData req, string region, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(region)) throw new InvalidOperationException("region null");

            var cacheKey = $"region_get_{region}";
            var cachedBytes = await distributedCache.GetAsync(cacheKey, cancellationToken);
            RegionData? model;

            if (cachedBytes is { Length: > 0 })
            {
                model = JsonSerializer.Deserialize<RegionData>(cachedBytes);
            }
            else
            {
                model = await repo.Get<RegionData>(DocumentType.Country, region.ToUpper(), cancellationToken);

                await SaveCache(model, cacheKey, TtlCache.OneWeek);
            }

            return await req.CreateResponse(model, TtlCache.OneWeek, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("SuggestionGet")]
    public async Task<HttpResponseData?> SuggestionGet(
      [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "suggestion/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"suggestion_{id}";
            var cachedBytes = await distributedCache.GetAsync(cacheKey, cancellationToken);
            Suggestion? model;

            if (cachedBytes is { Length: > 0 })
            {
                model = JsonSerializer.Deserialize<Suggestion>(cachedBytes);
            }
            else
            {
                model = await repo.Get<Suggestion>(DocumentType.Suggestion, id, cancellationToken);

                await SaveCache(model, cacheKey, TtlCache.OneWeek);
            }

            return await req.CreateResponse(model, TtlCache.OneWeek, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("SuggestionPost")]
    public async Task<Suggestion> SuggestionPost(
       [HttpTrigger(AuthorizationLevel.Anonymous, Method.Post, Route = "suggestion")] HttpRequestData req, CancellationToken cancellationToken)
    {
        try
        {
            var obj = await req.GetPublicBody<Suggestion>(cancellationToken);

            return await repo.UpsertItemAsync(obj, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    private async Task SaveCache<TData>(TData? model, string cacheKey, TtlCache ttl) where TData : class, new()
    {
        if (model != null)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(model);
            await distributedCache.SetAsync(cacheKey, bytes, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds((int)ttl) });
        }
    }
}