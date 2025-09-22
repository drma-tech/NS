using NS.WEB.Shared;

namespace NS.WEB.Core;

public struct Endpoint
{
    public static string Trailers(string mode)
    {
        return $"public/cache/trailers?mode={mode}";
    }
}

//public class CacheYoutubeApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<TrailerModel>>(http, null)
//{
//    public async Task<CacheDocument<TrailerModel>?> GetTrailers(string mode,
//        RenderControlCore<CacheDocument<TrailerModel>?>? core)
//    {
//        return await GetAsync(Endpoint.Trailers(mode), core);
//    }
//}