using NS.WEB.Shared;
using System.Net.Http.Json;

namespace NS.WEB.Modules.Country.Core;

public class AllRegionsApi(IHttpClientFactory factory) : ApiCore(factory, null, ApiType.Local)
{
    public async Task<AllRegions?> GetAll(RenderControlCore<AllRegions?>? core)
    {
        core?.LoadingStarted?.Invoke();
        var result = new AllRegions();
        try
        {
            result = await LocalHttp.GetFromJsonAsync<AllRegions>("/data/regions.json");
            return result;
        }
        finally
        {
            core?.LoadingFinished?.Invoke(result);
        }
    }
}