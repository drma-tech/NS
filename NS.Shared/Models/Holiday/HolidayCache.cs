namespace NS.Shared.Models.Holiday;

public class HolidayCache : CacheDocument<HolidayModel>
{
    public HolidayCache()
    {
    }

    public HolidayCache(HolidayModel data, string key) : base(key, data, TtlCache.OneMonth)
    {
    }
}