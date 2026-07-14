namespace NS.Shared.Models;

public class WishList() : PrivateMainDocument(DocumentType.WishList)
{
    public HashSet<WishListEntry> Items { get; set; } = [];
}

public class WishListEntry
{
    public string? Id { get; set; }
    public string? RegionCode { get; set; }
    public string? CityCode { get; set; }
    public string? RegionName { get; set; }
    public string? CityName { get; set; }
    public WishlistPhase Phase { get; set; } = WishlistPhase.Phase1;
    public List<CheckListItem> CheckList { get; set; } = [];
    public IReadOnlyCollection<ExperienceTag> ExperienceTags { get; set; } = [];
    public IReadOnlyCollection<IntentionTag> IntentionTags { get; set; } = [];
    public IReadOnlyCollection<ConditionsTag> ConditionsTags { get; set; } = [];
    public IReadOnlyCollection<AlertsTag> AlertsTags { get; set; } = [];
}

public enum WishlistPhase
{
    [FieldSettings("1 – Idea / Brainstorm", Description = "You just discovered this destination. No concrete plans yet, just exploring possibilities.")]
    Phase1 = 1,

    [FieldSettings("2 – Researching / Considering", Description = "You are gathering information: checking prices, best times to go, and travel requirements.")]
    Phase2 = 2,

    [FieldSettings("3 – Planning / Likely", Description = "This destination has become a real candidate. You are considering dates, budget, and logistics.")]
    Phase3 = 3
}

public class CheckListItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool Done { get; set; }
}

public enum ExperienceTag
{
    [FieldSettings("Adventure", Description = "For extreme or exploratory activities")]
    Adventure = 10,

    [FieldSettings("Relaxing", Description = "For rest, spas, calm beaches")]
    Relaxing = 11,

    [FieldSettings("Cultural", Description = "Museums, history, local traditions")]
    Cultural = 12,

    [FieldSettings("Luxury", Description = "Premium experience")]
    Luxury = 13,

    [FieldSettings("Nature", Description = "Mountains, parks, trails")]
    Nature = 14,

    [FieldSettings("Beach", Description = "Beaches and coastal activities")]
    Beach = 15,

    [FieldSettings("Urban", Description = "City life, architecture, urban exploration")]
    Urban = 16,

    [FieldSettings("Culinary", Description = "Gastronomy and culinary experiences")]
    Culinary = 17,

    [FieldSettings("Wildlife", Description = "Animal observation, safaris")]
    Wildlife = 18,

    [FieldSettings("Scenic Views", Description = "Landmarks with memorable views")]
    ScenicViews = 19,
}

public enum IntentionTag
{
    [FieldSettings("Honeymoon", Description = "Romantic trip")]
    Honeymoon = 20,

    [FieldSettings("Family Trip", Description = "Travel with family")]
    FamilyTrip = 21,

    [FieldSettings("Solo Trip", Description = "Travel alone")]
    SoloTrip = 22,

    [FieldSettings("Group Trip", Description = "Travel with friends or a group")]
    GroupTrip = 23,

    [FieldSettings("First Trip", Description = "Dream destination or bucket list")]
    FirstTrip = 24,

    [FieldSettings("Flexible Dates", Description = "Travel dates are flexible")]
    FlexibleDates = 25,

    [FieldSettings("Must Visit", Description = "High priority, almost certain trip")]
    MustVisit = 26,

    [FieldSettings("Someday", Description = "Destination for future consideration")]
    Someday = 27,
}

public enum ConditionsTag
{
    [FieldSettings("Summer", Description = "Best visited in summer")]
    Summer = 30,

    [FieldSettings("Winter", Description = "Best visited in winter")]
    Winter = 31,

    [FieldSettings("Spring", Description = "Best visited in spring")]
    Spring = 32,

    [FieldSettings("Fall", Description = "Best visited in autumn")]
    Fall = 33,

    [FieldSettings("Seasonal Attraction", Description = "Seasonal events or phenomena")]
    SeasonalAttraction = 34,

    [FieldSettings("Crowded Season", Description = "High season / crowded periods")]
    CrowdedSeason = 35,
}

public enum AlertsTag
{
    [FieldSettings("Visa Required", Description = "Visa is required")]
    VisaRequired = 40,

    [FieldSettings("Vaccine Required", Description = "Vaccination required")]
    VaccineRequired = 41,

    [FieldSettings("Safety Concerns", Description = "Security precautions advised")]
    SafetyConcerns = 42,

    [FieldSettings("Long Flight", Description = "Long-distance flight expected")]
    LongFlight = 43,

    [FieldSettings("High Budget", Description = "High estimated cost")]
    HighBudget = 44,

    [FieldSettings("Travel Restrictions", Description = "Possible travel restrictions")]
    TravelRestrictions = 45
}