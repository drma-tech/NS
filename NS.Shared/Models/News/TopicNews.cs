namespace NS.Shared.Models.News
{
    public class DatumTopic
    {
        public string? title { get; set; }
        public string? url { get; set; }
        public string? excerpt { get; set; }
        public string? thumbnail { get; set; }
        public string? language { get; set; }
        public bool? paywall { get; set; }
        public int? contentLength { get; set; }
        public DateTime? date { get; set; }
        public List<string>? authors { get; set; }
        public List<string>? keywords { get; set; }
        public Publisher? publisher { get; set; }
    }

    public class Publisher
    {
        public string? name { get; set; }
        public string? url { get; set; }
        public string? favicon { get; set; }
    }

    public class TopicNews
    {
        public bool? success { get; set; }
        public int? size { get; set; }
        public int? totalHits { get; set; }
        public int? hitsPerPage { get; set; }
        public int? page { get; set; }
        public int? totalPages { get; set; }
        public int? timeMs { get; set; }
        public List<DatumTopic>? data { get; set; }
    }
}