namespace NS.Shared.Models.Holiday;

public class HolidayCache(string id, HolidayModel data) : CacheDocumentData<HolidayModel>(new CacheIdentity(id), data, TtlCache.OneMonth)
{
}

public class HolidayModel
{
    public ICollection<HolidayModelItem> Items { get; set; } = [];
}

public class HolidayModelItem
{
    public HolidayModelItem()
    {
    }

    public HolidayModelItem(string? name, string? desc, DateTime? date, string? type)
    {
        this.name = name;
        this.desc = desc;
        this.date = date;
        this.type = type;
    }

    public string? name { get; set; }
    public string? desc { get; set; }
    public DateTime? date { get; set; }
    public string? type { get; set; }
}

public static class HolidayHelper
{
    private static readonly string[] ReligiousKeywords =
    {
        "religious",
        "christian",
        "orthodox",
        "muslim",
        "islam",
        "hindu",
        "jewish",
        "buddhist",
        "catholic",
        "hebrew",
    };

    public static bool IsReligiousHoliday(this string? type)
    {
        if (type.Empty()) return false;

        return ReligiousKeywords.Any(keyword => type.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}