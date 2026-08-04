namespace NS.Shared.Models.GlobalConflicts;

public class GlobalConflictsCache(string id, GlobalConflictsModel data) : CacheDocumentData<GlobalConflictsModel>(new CacheIdentity(id), data, TtlCache.OneWeek)
{
}

public class GlobalConflictsModel
{
    public ICollection<GlobalConflictsItem> Items { get; set; } = [];
}

public class GlobalConflictsItem
{
    public GlobalConflictsItem()
    {
    }

    public GlobalConflictsItem(string? title, string? type, string? status, IReadOnlyCollection<string> regions)
    {
        this.title = title;
        this.type = type;
        this.status = status;
        this.regions = regions;
    }

    public string? title { get; set; }
    public string? type { get; set; }
    public string? status { get; set; }
    public IReadOnlyCollection<string> regions { get; set; } = [];
}