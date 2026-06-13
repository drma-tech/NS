using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Country.Core;

public class RegionsApi(IHttpClientFactory factory) : ApiCosmos<RegionData>(factory, ApiType.Anonymous, null, ApiContext.Default.RegionData)
{
    public async Task<RegionData?> GetRegion(string? region, CancellationToken cancellationToken)
    {
        if (region.Empty()) return null;

        return await GetAsync(Endpoint.GetRegion(region), false, null, cancellationToken);
    }

    private struct Endpoint
    {
        public static string GetRegion(string region) => $"public/region/get/{region}";
    }
}

public class SuggestionsApi(IHttpClientFactory factory) : ApiCosmos<Suggestion>(factory, ApiType.Anonymous, null, ApiContext.Default.Suggestion)
{
    public async Task<Suggestion?> SuggestionGet(string id, ComponentActions<Suggestion?>? actions, CancellationToken cancellationToken)
    {
        if (id.Empty()) return null;
        return await GetAsync(Endpoint.SuggestionGet(id), false, actions, cancellationToken);
    }

    public async Task<Suggestion?> SuggestionPost(Suggestion suggestion, CancellationToken cancellationToken)
    {
        return await PostAsync(Endpoint.SuggestionPost, suggestion, cancellationToken);
    }

    private struct Endpoint
    {
        public static string SuggestionGet(string id) => $"suggestion/{id}";

        public static string SuggestionPost => "suggestion";
    }
}

public class ScoreApi(IHttpClientFactory factory) : ApiCosmos<Score>(factory, ApiType.Anonymous, null, ApiContext.Default.Score)
{
    public async Task<Score?> ScoreGet(string id, ComponentActions<Score?>? actions, CancellationToken cancellationToken)
    {
        if (id.Empty()) return null;
        return await GetAsync(Endpoint.ScoreGet(id), false, actions, cancellationToken);
    }

    private struct Endpoint
    {
        public static string ScoreGet(string id) => $"score/{id}";
    }
}