using System.ComponentModel.DataAnnotations.Schema;

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

    public List<RegionModel> GetList(string? continent = null, string? subcontinent = null)
    {
        if (continent.NotEmpty() && subcontinent.NotEmpty())
            return Items.Where(w => w.continent == continent && w.subcontinent == subcontinent).OrderBy(o => o.name).ToList();
        if (continent.NotEmpty())
            return Items.Where(w => w.continent == continent).OrderBy(o => o.name).ToList();
        else
            return Items.OrderBy(o => o.name).ToList();
    }

    public RegionModel? GetByCode(string? code)
    {
        return Items.SingleOrDefault(f => f.code!.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    public RegionModel? GetByName(string? name)
    {
        return Items.SingleOrDefault(f => f.name!.Equals(name, StringComparison.OrdinalIgnoreCase));
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

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [NotMapped]
    public string? customName => subcontinent.NotEmpty() ? $"{continent} | {subcontinent} | {fullName}" : $"{continent} | {fullName}";
}