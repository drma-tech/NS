namespace NS.Shared.Models;

public class TravelHistory() : PrivateMainDocument(DocumentType.TravelHistory)
{
    public HashSet<TravelHistoryEntry> Items { get; set; } = [];
}

public class TravelHistoryEntry
{
    public string? Id { get; set; }
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string? RegionCode { get; set; }
    public string? CityCode { get; set; }
    public string? RegionName { get; set; }
    public string? CityName { get; set; }
    public int? RegionRating { get; set; }
    public int? CityRating { get; set; }
    public string? Notes { get; set; }
}