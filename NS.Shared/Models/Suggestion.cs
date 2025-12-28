namespace NS.Shared.Models
{
    public class Suggestion() : ProtectedMainDocument(DocumentType.Suggestion)
    {
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public List<SuggestionCountry> Countries { get; set; } = [];
    }

    public class SuggestionCountry
    {
        public int Index { get; set; }
        public Enums.Country Country { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}