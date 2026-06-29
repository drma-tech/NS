namespace NS.Shared.Models.News
{
    public class Metadata
    {
        public string? id { get; set; }
        public int total_hits { get; set; }
        public int took_ms { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class NavigationLink
    {
        public string? name { get; set; }
        public string? topic_id { get; set; }
    }

    public class NewsArticle
    {
        public string? type { get; set; }
        public string? title { get; set; }
        public string? url { get; set; }
        public DateTime? date { get; set; }
        public string? image { get; set; }
        public List<object>? authors { get; set; }
        public Source? source { get; set; }
    }

    public class Parameters
    {
        public string? query { get; set; }
        public string? language { get; set; }
        public string? from { get; set; }
        public string? to { get; set; }
        public string? country { get; set; }
    }

    public class GoogleNews
    {
        public Metadata? metadata { get; set; }
        public Parameters? parameters { get; set; }
        public List<NewsArticle>? news_articles { get; set; }
        public List<NavigationLink>? navigation_links { get; set; }
    }

    public class Source
    {
        public string? name { get; set; }
        public string? url { get; set; }
        public string? favicon { get; set; }
    }
}