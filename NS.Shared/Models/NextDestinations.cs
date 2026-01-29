namespace NS.Shared.Models;

public class NextDestinations() : PrivateMainDocument(DocumentType.NextDestinations)
{
    public HashSet<NextDestinationsEntry> Items { get; set; } = [];
}

public class NextDestinationsEntry
{
    public string? Id { get; set; }
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string? RegionCode { get; set; }
    public string? CityCode { get; set; }
    public string? RegionName { get; set; }
    public string? CityName { get; set; }
    public List<CheckListItem> CheckList { get; set; } = [];
    public string? Notes { get; set; }
}