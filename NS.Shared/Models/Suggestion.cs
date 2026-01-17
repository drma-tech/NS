namespace NS.Shared.Models
{
    public class Suggestion() : ProtectedMainDocument(DocumentType.Suggestion)
    {
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public List<SuggestionRegion> Regions { get; set; } = [];
    }

    public class SuggestionRegion
    {
        public int Index { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PhotoId { get; set; }
    }
}