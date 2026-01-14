using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Country.Core;

public class ScrapApi(IHttpClientFactory factory) : ApiCosmos<RegionData>(factory, ApiType.Authenticated, null)
{
    public async Task ScrapPopulation(Field field)
    {
        await PostAsync(Endpoint.ScrapPopulation(field), null);
    }

    private struct Endpoint
    {
        public static string ScrapPopulation(Field field) => $"adm/scrap/{(int)field}";
    }
}