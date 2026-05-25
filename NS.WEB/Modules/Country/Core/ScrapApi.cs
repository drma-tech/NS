using NS.Shared.Models.Country;

namespace NS.WEB.Modules.Country.Core;

public class ScrapApi(IHttpClientFactory factory) : ApiCosmos<RegionData>(factory, ApiType.Authenticated, null, ApiContext.Default.RegionData)
{
    public async Task ScrapPopulation(Field field, CancellationToken cancellationToken)
    {
        await PostAsync(Endpoint.ScrapPopulation(field), null, cancellationToken);
    }

    private struct Endpoint
    {
        public static string ScrapPopulation(Field field) => $"adm/scrap/{(int)field}";
    }
}