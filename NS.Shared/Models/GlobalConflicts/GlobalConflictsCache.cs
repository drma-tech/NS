namespace NS.Shared.Models.GlobalConflicts;

public class GlobalConflictsCache(string id, GlobalConflictsModel data) : CacheDocumentData<GlobalConflictsModel>(new CacheIdentity(id), data, TtlCache.OneWeek)
{
}

public class GlobalConflictsModel
{
    public ISet<GlobalConflictsItem> Items { get; set; } = new HashSet<GlobalConflictsItem>();
}

public class GlobalConflictsItem : EqualityBase<GlobalConflictsItem>
{
    public int id { get; set; }
    public string? title { get; set; }
    public IEnumerable<string> non_state_parties { get; set; } = [];
    public IEnumerable<string> state_parties { get; set; } = [];
    public IEnumerable<string> violations { get; set; } = [];

    protected override object?[] EqualityValues => [id];
}