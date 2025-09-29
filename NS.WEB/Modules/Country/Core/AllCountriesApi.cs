using NS.WEB.Shared;
using System.Net.Http.Json;

namespace NS.WEB.Modules.Country.Core;

public class AllCountriesApi(IHttpClientFactory factory) : ApiCore(factory, null, ApiType.Local)
{
    public async Task<AllCountries?> GetAll(RenderControlCore<AllCountries?>? core)
    {
        core?.LoadingStarted?.Invoke();
        var result = new AllCountries();
        try
        {
            result = await LocalHttp.GetFromJsonAsync<AllCountries>("/data/countries.json");
            return result;
        }
        finally
        {
            core?.LoadingFinished?.Invoke(result);
        }
    }
}