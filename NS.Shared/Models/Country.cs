using System.Text.Json.Serialization;

namespace NS.Shared.Models;

public class AllCountries
{
    public List<CountryModel> Items { get; set; } = [];

    public List<CountryModel> GetCountries(string? region, string? subregion)
    {
        if (region.NotEmpty() && subregion.NotEmpty())
            return Items.Where(w => w.Region == region && w.Subregion == subregion).OrderBy(o => o.Name).ToList();
        if (region.NotEmpty())
            return Items.Where(w => w.Region == region).OrderBy(o => o.Name).ToList();
        else
            return Items.OrderBy(o => o.Name).ToList();
    }

    public List<string> GetRegions()
    {
        return Items.Select(s => s.Region).Distinct().Order().ToList();
    }

    public List<string?> GetSubRegions(string? region = null)
    {
        if (region.Empty())
            return [];
        else
            return Items.Where(w => w.Region == region).Select(s => s.Subregion).Distinct().Order().ToList();
    }
}

public class CountryModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? FullName { get; set; }
    public required string Region { get; set; }
    public string? Subregion { get; set; }

    [JsonIgnore]
    public string? Flag => $"https://flagcdn.com/{Id.ToLower()}.svg";
}