using NS.Shared.Models.Country;
using NS.WEB.Api.Core;
using System.Globalization;

namespace NS.WEB.Api.Module.Cosmos.Admin;

public class ScrapApi(IHttpClientFactory factory) : ApiCosmos<RegionData>(factory, ApiType.Authenticated, null, [], ApiContext.Default.RegionData)
{
    public async Task ScrapPopulation(Field field, CancellationToken cancellationToken)
    {
        await PostAsync(string.Create(CultureInfo.InvariantCulture, $"adm/scrap/{(int)field}"), null, states: [], cancellationToken);
    }
}