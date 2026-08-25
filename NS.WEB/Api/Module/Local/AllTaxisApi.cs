using NS.WEB.Api.Core;
using System.Net.Http.Json;

namespace NS.WEB.Api.Module.Local
{
    public class AllTaxisApi(IHttpClientFactory factory) : ApiLocal(factory)
    {
        public async Task<AllTaxis?> GetAllTaxis(CancellationToken cancellationToken)
        {
            return await LocalHttp.GetFromJsonAsync("/data/taxis.json", ApiContext.Default.AllTaxis, cancellationToken);
        }
    }
}