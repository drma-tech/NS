using System.Net.Http.Json;

namespace NS.WEB.Modules.Country.Core;

public class AllRegionsApi(IHttpClientFactory factory) : ApiCore(factory, null, ApiType.Local)
{
    public async Task<AllRegions?> GetAll()
    {
        return await LocalHttp.GetFromJsonAsync<AllRegions>("/data/regions.json");
    }
}