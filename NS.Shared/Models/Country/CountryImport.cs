using NS.Shared.Core.Types;

namespace NS.Shared.Models.Country
{
    public class CountryImport(string? id) : GroupDocument(new GroupIdentity(GroupType.Import, id))
    {
        public IDictionary<string, string> CustomNames { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public ISet<ImportEvent> Events { get; set; } = new HashSet<ImportEvent>();

        protected override object?[] EqualityValues => [Id];
    }

    public class ImportEvent
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int Success { get; set; }
        public int Failure { get; set; }
    }
}