using NS.WEB.Api.Core;
using System.Net.Http.Json;

namespace NS.WEB.Api.Module.Local
{
    public class AllRegionsApi(IHttpClientFactory factory) : ApiLocal(factory)
    {
        public async Task<AllRegions?> GetAllRegions(CancellationToken cancellationToken)
        {
            return await LocalHttp.GetFromJsonAsync("/data/regions.json", ApiContext.Default.AllRegions, cancellationToken);
        }
    }
}