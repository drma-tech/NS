using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Country.Core;

public class CountriesApi(IHttpClientFactory factory) : ApiCosmos<CountryData>(factory, null)
{
    public async Task<CountryData?> GetCountry(string code)
    {
        return await GetAsync(Endpoint.CountryGet(code), null);
    }

    public async Task StartCountry(AllCountries model)
    {
        await PostAsync(Endpoint.CountryStart, null, model);
    }

    private struct Endpoint
    {
        public static string CountryGet(string code) => $"public/country/get?code={code}";
        public const string CountryStart = "adm/country/start";
    }
}