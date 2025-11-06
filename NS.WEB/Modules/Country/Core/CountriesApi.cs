using NS.Shared.Models.Country;
using NS.WEB.Shared;

namespace NS.WEB.Modules.Country.Core;

public class CountriesApi(IHttpClientFactory factory) : ApiCosmos<CountryData>(factory, ApiType.Anonymous, null)
{
    public async Task<CountryData?> GetCountry(string code, RenderControlCore<CountryData?> core)
    {
        return await GetAsync(Endpoint.CountryGet(code), core);
    }

    private struct Endpoint
    {
        public static string CountryGet(string code) => $"public/country/get?code={code}";
    }
}