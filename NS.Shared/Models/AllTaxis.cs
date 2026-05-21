namespace NS.Shared.Models;

public class AllTaxis
{
    public List<TaxiModel> Items { get; set; } = [];

    public List<TaxiModel> GetList(string? region = null)
    {
        if (region.NotEmpty())
            return Items.Where(w => w.regions.Contains(region)).OrderBy(o => o.name).ToList();
        else
            return Items.OrderBy(o => o.name).ToList();
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
    public HashSet<string> regions { get; set; } = [];
}