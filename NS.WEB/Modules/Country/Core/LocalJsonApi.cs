using System.Net.Http.Json;

namespace NS.WEB.Modules.Country.Core;

public class LocalJsonApi(IHttpClientFactory factory) : ApiCore(factory, null, ApiType.Local)
{
    public async Task<AllRegions?> GetAllRegions()
    {
        return await LocalHttp.GetFromJsonAsync<AllRegions>("/data/regions.json");
    }
}