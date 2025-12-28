using NS.Shared.Models.Country;
using NS.WEB.Shared;

namespace NS.WEB.Modules.Country.Core;

public class CountriesApi(IHttpClientFactory factory) : ApiCosmos<CountryData>(factory, ApiType.Anonymous, null)
{
    public async Task<CountryData?> GetCountry(string region, RenderControlCore<CountryData?> core)
    {
        return await GetAsync(Endpoint.CountryGet(region), core);
    }

    private struct Endpoint
    {
        public static string CountryGet(string region) => $"public/country/get/{region}";
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