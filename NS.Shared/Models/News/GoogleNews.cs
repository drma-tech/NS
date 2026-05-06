namespace NS.Shared.Models.News
{
    public class Article
    {
        public string? title { get; set; }
        public string? url { get; set; }
        public DateTime date { get; set; }
        public string? thumbnail { get; set; }
        public string? description { get; set; }
        public Source? source { get; set; }
        public List<string>? keywords { get; set; }
        public List<string>? authors { get; set; }
    }

    public class GoogleNews
    {
        public bool? success { get; set; }
        public int? totalHits { get; set; }
        public int? size { get; set; }
        public int? pageSize { get; set; }
        public int? page { get; set; }
        public int? totalPages { get; set; }
        public List<Article>? articles { get; set; }
    }

    public class Source
    {
        public string? name { get; set; }
        public string? url { get; set; }
        public string? favicon { get; set; }
    }
}