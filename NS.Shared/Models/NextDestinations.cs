using NS.Shared.Core.Types;

namespace NS.Shared.Models;

public class NextDestinations(string? id) : MainDocument(new MainIdentity(MainType.NextDestinations, id))
{
    public ISet<NextDestinationsEntry> Items { get; set; } = new HashSet<NextDestinationsEntry>();

    protected override object?[] EqualityValues => [Id];
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
    public ISet<CheckListItem> CheckList { get; set; } = new HashSet<CheckListItem>();
    public string? Notes { get; set; }
}