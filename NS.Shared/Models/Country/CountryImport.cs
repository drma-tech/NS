using NS.Shared.Core.Types;

namespace NS.Shared.Models.Country
{
    public class CountryImport(string? id) : GroupDocument(new GroupIdentity(GroupType.Import, id))
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