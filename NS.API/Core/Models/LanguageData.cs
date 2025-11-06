namespace NS.API.Core.Models
{
    public class CountryItem
    {
        public string? country { get; set; }
        public HashSet<string> languages { get; set; } = [];
    }

    public class LanguageData
    {
        public HashSet<CountryItem> countries { get; set; } = [];
    }
}