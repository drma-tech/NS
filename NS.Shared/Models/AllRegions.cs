using System.ComponentModel.DataAnnotations.Schema;

namespace NS.Shared.Models;

public class AllRegions
{
    public IReadOnlyCollection<RegionModel> Items { get; set; } = [];

    public IEnumerable<string?> GetContinents()
    {
        return Items.Select(s => s.continent).Distinct(StringComparer.OrdinalIgnoreCase).Order(StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<string?> GetSubContinents(string? continent = null)
    {
        if (continent.Empty())
            return [];

        return Items.Where(w => string.Equals(w.continent, continent, StringComparison.OrdinalIgnoreCase)).Select(s => s.subcontinent).Distinct(StringComparer.OrdinalIgnoreCase).Order(StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<RegionModel> GetList(string? continent = null, string? subcontinent = null, bool filterRequired = false)
    {
        if (filterRequired && continent.Empty() && subcontinent.Empty()) return [];

        if (continent.NotEmpty() && subcontinent.NotEmpty())
            return Items.Where(w => string.Equals(w.continent, continent, StringComparison.OrdinalIgnoreCase) && string.Equals(w.subcontinent, subcontinent, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.name, StringComparer.OrdinalIgnoreCase);
        if (continent.NotEmpty())
            return Items.Where(w => string.Equals(w.continent, continent, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.name, StringComparer.OrdinalIgnoreCase);

        return Items.OrderBy(o => o.name, StringComparer.OrdinalIgnoreCase);
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
    public string? code3 { get; set; }
    public string? name { get; set; }
    public string? fullName { get; set; }
    public string? description { get; set; }
    public string? capital { get; set; }
    public string? continent { get; set; }
    public string? subcontinent { get; set; }
    public double? score { get; set; }
    public double? expensesScore { get; set; }
    public double? safetyScore { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [NotMapped]
    public string? customName => subcontinent.NotEmpty() ? $"{continent} | {subcontinent} | {fullName}" : $"{continent} | {fullName}";
}