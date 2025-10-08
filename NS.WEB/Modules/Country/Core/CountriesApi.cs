using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Country.Core;

public class CountriesApi(IHttpClientFactory factory) : ApiCosmos<CountryData>(factory, null)
{
    public async Task<CountryData?> GetCountry(string code)
    {
        return await GetAsync(Endpoint.CountryGet(code), null);
    }

    private struct Endpoint
    {
        public static string CountryGet(string code) => $"public/country/get?code={code}";
    }
}