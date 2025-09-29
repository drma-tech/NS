namespace NS.Shared.Models.Country
{
    public class CountryImport() : ProtectedMainDocument(DocumentType.Import)
    {
        public Dictionary<string, string> CustomNames { get; set; } = [];
        public List<ImportEvent> Events { get; set; } = [];
    }

    public class ImportEvent
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int Success { get; set; }
        public int Failure { get; set; }
    }
}