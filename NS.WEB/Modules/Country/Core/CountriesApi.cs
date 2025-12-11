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