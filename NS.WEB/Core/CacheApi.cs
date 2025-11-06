using NS.Shared.Models.News;
using NS.WEB.Shared;

namespace NS.WEB.Core;

public struct Endpoint
{
    public static string News(string code, string mode)
    {
        return $"public/cache/news?code={code}&mode={mode}";
    }
}

public class CacheGoogleNewsApi(IHttpClientFactory http) : ApiCosmos<CacheDocument<NewsModel>>(http, ApiType.Anonymous, null)
{
    public async Task<CacheDocument<NewsModel>?> GetNews(string code, string mode, RenderControlCore<CacheDocument<NewsModel>?>? core)
    {
        return await GetAsync(Endpoint.News(code, mode), core);
    }
}