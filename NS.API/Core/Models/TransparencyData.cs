namespace NS.API.Core.Models
{
    public class TransparencyData
    {
        public string? country { get; set; }
        public string? iso3 { get; set; }
        public string? region { get; set; }
        public int year { get; set; }
        public int score { get; set; }
        public int rank { get; set; }
        public int sources { get; set; }
        public string? standardError { get; set; }
    }
}