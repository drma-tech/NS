namespace NS.Shared.Models;

public class AllTaxis
{
    public IReadOnlyCollection<TaxiModel> Items { get; set; } = [];

    public IEnumerable<TaxiModel> GetList(string? region = null)
    {
        if (region.NotEmpty())
            return Items.Where(w => w.regions.Contains(region, StringComparer.OrdinalIgnoreCase)).OrderBy(o => o.name, StringComparer.OrdinalIgnoreCase);

        return Items.OrderBy(o => o.name, StringComparer.OrdinalIgnoreCase);
    }

    public TaxiModel? GetByName(string? name)
    {
        return Items.SingleOrDefault(f => f.name!.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}

public class TaxiModel
{
    public string? name { get; set; }
    public string? logo { get; set; }
    public string? url { get; set; }
    public ICollection<string> regions { get; set; } = [];
}