using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NS.Shared.Models.Country;

namespace NS.API.Functions;

public class RegionFunction(CosmosGroupRepository repo)
{
    [Function("RegionGet")]
    public async Task<HttpResponseData?> RegionGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "public/region/get/{region}")] HttpRequestData req, string region, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(region)) throw new InvalidOperationException("region null");

            var model = await repo.Get<RegionData>(DocumentType.Country, region.ToUpper(), cancellationToken);

            return await req.CreateResponse(model, TtlCache.OneWeek, cancellationToken);
        }
        catch (Exception ex)
        {
            req.LogError(ex);
            throw;
        }
    }

    [Function("SuggestionGet")]
    public async Task<Suggestion?> SuggestionGet(
      [HttpTrigger(AuthorizationLevel.Anonymous, Method.Get, Route = "suggestion/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
    {
        try
        {
            return await repo.Get<Suggestion>(DocumentType.Suggestion, id, cancellationToken);
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
}