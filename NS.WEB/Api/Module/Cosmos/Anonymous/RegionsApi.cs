using NS.Shared.Models.Country;
using NS.WEB.Api.Core;

namespace NS.WEB.Api.Module.Cosmos.Anonymous;

public class RegionsApi(IHttpClientFactory factory) : ApiCosmos<RegionData>(factory, ApiType.Anonymous, null, [], ApiContext.Default.RegionData)
{
    public async Task<RegionData?> GetRegion(string? region, CancellationToken cancellationToken)
    {
        if (region.Empty()) return null;

        return await GetAsync($"public/region/get/{region}", setNewVersion: false, states: [], cancellationToken);
    }
}

public class SuggestionsApi(IHttpClientFactory factory) : ApiCosmos<Suggestion>(factory, ApiType.Anonymous, null, [], ApiContext.Default.Suggestion)
{
    public async Task<Suggestion?> SuggestionGet(string id, RenderControlState<Suggestion?>[] states, CancellationToken cancellationToken)
    {
        if (id.Empty()) return null;
        return await GetAsync($"suggestion/{id}", setNewVersion: false, states, cancellationToken);
    }

    public async Task<Suggestion?> SuggestionPost(Suggestion suggestion, CancellationToken cancellationToken)
    {
        return await PostAsync("suggestion", suggestion, states: [], cancellationToken);
    }
}

public class ScoreApi(IHttpClientFactory factory) : ApiCosmos<Score>(factory, ApiType.Anonymous, key: null, [], ApiContext.Default.Score)
{
    public async Task<Score?> ScoreGet(string id, RenderControlState<Score?>[] states, CancellationToken cancellationToken)
    {
        if (id.Empty()) return null;
        return await GetAsync($"score/{id}", setNewVersion: false, states, cancellationToken);
    }
}