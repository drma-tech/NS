namespace NS.Shared.Models.Holiday;

public class HolidayModel
{
    public List<Item> Items { get; set; } = [];
}

public class Item
{
    public Item()
    {
    }

    public Item(string? name, string? desc, DateTime? date, string? type)
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
        "hebrew"
    };

    public static bool IsReligiousHoliday(this string? type)
    {
        if (type.Empty()) return false;

        return ReligiousKeywords.Any(keyword => type.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}