using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Country.Core;

public class ScrapApi(IHttpClientFactory factory) : ApiCosmos<CountryData>(factory, null)
{
    public async Task ScrapPopulation(AllCountries model, Field field)
    {
        await PostAsync(Endpoint.ScrapPopulation(field), null, model);
    }

    private struct Endpoint
    {
        public static string ScrapPopulation(Field field) => $"adm/scrap/{(int)field}";
    }
}