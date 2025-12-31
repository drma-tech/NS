using NS.Shared.Models.Country;
using NS.WEB.Shared;

namespace NS.WEB.Modules.Country.Core;

public class RegionsApi(IHttpClientFactory factory) : ApiCosmos<RegionData>(factory, ApiType.Anonymous, null)
{
    public async Task<RegionData?> GetRegion(string region, RenderControlCore<RegionData?>? core)
    {
        return await GetAsync(Endpoint.GetRegion(region), core);
    }

    private struct Endpoint
    {
        public static string GetRegion(string region) => $"public/region/get/{region}";
    }
}

public class SuggestionsApi(IHttpClientFactory factory) : ApiCosmos<Suggestion>(factory, ApiType.Anonymous, null)
{
    public async Task<Suggestion?> SuggestionGet(string id)
    {
        if (id.Empty()) return null;
        return await GetAsync(Endpoint.SuggestionGet(id), null);
    }

    public async Task<Suggestion?> SuggestionPost(Suggestion suggestion)
    {
        return await PostAsync(Endpoint.SuggestionPost, null, suggestion);
    }

    private struct Endpoint
    {
        public static string SuggestionGet(string id) => $"suggestion/{id}";

        public static string SuggestionPost => "suggestion";
    }
}