using System.Net.Http.Json;

namespace NS.WEB.Modules.Country.Core;

public class LocalJsonApi(IHttpClientFactory factory) : ApiCore(factory, null, ApiType.Local)
{
    public async Task<AllRegions?> GetAllRegions(CancellationToken cancellationToken)
    {
        return await LocalHttp.GetFromJsonAsync("/data/regions.json", ApiContext.Default.AllRegions, cancellationToken);
    }

    public async Task<AllTaxis?> GetAllTaxis(CancellationToken cancellationToken)
    {
        return await LocalHttp.GetFromJsonAsync("/data/taxis.json", ApiContext.Default.AllTaxis, cancellationToken);
    }
}