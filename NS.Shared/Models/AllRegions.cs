namespace NS.Shared.Models;

public class AllRegions
{
    public List<RegionModel> Items { get; set; } = [];

    public List<string?> GetContinents()
    {
        return Items.Select(s => s.continent).Distinct().Order().ToList();
    }

    public List<string?> GetSubContinents(string? continent = null)
    {
        if (continent.Empty())
            return [];
        else
            return Items.Where(w => w.continent == continent).Select(s => s.subcontinent).Distinct().Order().ToList();
    }

    public List<RegionModel> Filter(string? continent, string? subcontinent)
    {
        if (continent.NotEmpty() && subcontinent.NotEmpty())
            return Items.Where(w => w.continent == continent && w.subcontinent == subcontinent).OrderBy(o => o.name).ToList();
        if (continent.NotEmpty())
            return Items.Where(w => w.continent == continent).OrderBy(o => o.name).ToList();
        else
            return Items.OrderBy(o => o.name).ToList();
    }
}

public class RegionModel
{
    public string? code { get; set; }
    public string? name { get; set; }
    public string? fullName { get; set; }
    public string? description { get; set; }
    public string? capital { get; set; }
    public string? continent { get; set; }
    public string? subcontinent { get; set; }
    public int? score { get; set; }
}