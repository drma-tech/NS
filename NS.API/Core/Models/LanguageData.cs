namespace NS.API.Core.Models
{
    public class CountryItem
    {
        public string? country { get; set; }
        public IReadOnlyCollection<string> languages { get; set; } = [];
    }

    public class LanguageData
    {
        public IReadOnlyCollection<CountryItem> countries { get; set; } = [];
    }
}