namespace NS.Shared.Models.News;

public class NewsCache : CacheDocument<NewsModel>
{
    public NewsCache()
    {
    }

    public NewsCache(NewsModel data, string key) : base(key, data, TtlCache.OneWeek)
    {
    }
}