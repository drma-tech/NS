namespace NS.API.Core.Models
{
    public class Country
    {
        public string? code { get; set; }
        public string? country { get; set; }
        public bool has_data { get; set; }
        public string? region { get; set; }
        public int visa_free_count { get; set; }
        public double openness { get; set; }
        public bool is_schengen { get; set; }
        public string? visa_free_url { get; set; }
        public string? visa_required_url { get; set; }
        public string? visa_full_url { get; set; }
        public object? data { get; set; }
    }

    public class HerleyData
    {
        public List<Country> countries { get; set; } = [];
    }
}