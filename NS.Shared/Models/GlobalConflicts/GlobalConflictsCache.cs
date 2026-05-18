namespace NS.Shared.Models.GlobalConflicts;

public class GlobalConflictsCache : CacheDocument<GlobalConflicts>
{
    public GlobalConflictsCache()
    {
    }

    public GlobalConflictsCache(GlobalConflicts data, string key) : base(key, data, TtlCache.OneWeek)
    {
    }
}