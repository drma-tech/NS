namespace NS.Shared.Models.News
{
    public class Datum
    {
        public string? title { get; set; }
        public string? url { get; set; }
        public DateTime date { get; set; }
        public string? thumbnail { get; set; }
        public string? description { get; set; }
        public Source? source { get; set; }
        public List<string> keywords { get; set; } = [];
        public List<string> authors { get; set; } = [];
    }

    public class GoogleNews
    {
        public bool success { get; set; }
        public int total { get; set; }
        public List<Datum> data { get; set; } = [];
    }

    public class Source
    {
        public string? name { get; set; }
        public string? url { get; set; }
        public string? favicon { get; set; }
    }
}