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
    [Custom(Name = "1 – Idea / Brainstorm", Description = "You just discovered this destination. No concrete plans yet, just exploring possibilities.")]
    Phase1 = 1,

    [Custom(Name = "2 – Researching / Considering", Description = "You are gathering information: checking prices, best times to go, and travel requirements.")]
    Phase2 = 2,

    [Custom(Name = "3 – Planning / Likely", Description = "This destination has become a real candidate. You are considering dates, budget, and logistics.")]
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
    [Custom(Name = "Adventure", Description = "For extreme or exploratory activities")]
    Adventure = 10,

    [Custom(Name = "Relaxing", Description = "For rest, spas, calm beaches")]
    Relaxing = 11,

    [Custom(Name = "Cultural", Description = "Museums, history, local traditions")]
    Cultural = 12,

    [Custom(Name = "Luxury", Description = "Premium experience")]
    Luxury = 13,

    [Custom(Name = "Nature", Description = "Mountains, parks, trails")]
    Nature = 14,

    [Custom(Name = "Beach", Description = "Beaches and coastal activities")]
    Beach = 15,

    [Custom(Name = "Urban", Description = "City life, architecture, urban exploration")]
    Urban = 16,

    [Custom(Name = "Culinary", Description = "Gastronomy and culinary experiences")]
    Culinary = 17,

    [Custom(Name = "Wildlife", Description = "Animal observation, safaris")]
    Wildlife = 18,

    [Custom(Name = "Scenic Views", Description = "Landmarks with memorable views")]
    ScenicViews = 19,
}

public enum IntentionTag
{
    [Custom(Name = "Honeymoon", Description = "Romantic trip")]
    Honeymoon = 20,

    [Custom(Name = "Family Trip", Description = "Travel with family")]
    FamilyTrip = 21,

    [Custom(Name = "Solo Trip", Description = "Travel alone")]
    SoloTrip = 22,

    [Custom(Name = "Group Trip", Description = "Travel with friends or a group")]
    GroupTrip = 23,

    [Custom(Name = "First Trip", Description = "Dream destination or bucket list")]
    FirstTrip = 24,

    [Custom(Name = "Flexible Dates", Description = "Travel dates are flexible")]
    FlexibleDates = 25,

    [Custom(Name = "Must Visit", Description = "High priority, almost certain trip")]
    MustVisit = 26,

    [Custom(Name = "Someday", Description = "Destination for future consideration")]
    Someday = 27,
}

public enum ConditionsTag
{
    [Custom(Name = "Summer", Description = "Best visited in summer")]
    Summer = 30,

    [Custom(Name = "Winter", Description = "Best visited in winter")]
    Winter = 31,

    [Custom(Name = "Spring", Description = "Best visited in spring")]
    Spring = 32,

    [Custom(Name = "Fall", Description = "Best visited in autumn")]
    Fall = 33,

    [Custom(Name = "Seasonal Attraction", Description = "Seasonal events or phenomena")]
    SeasonalAttraction = 34,

    [Custom(Name = "Crowded Season", Description = "High season / crowded periods")]
    CrowdedSeason = 35,
}

public enum AlertsTag
{
    [Custom(Name = "Visa Required", Description = "Visa is required")]
    VisaRequired = 40,

    [Custom(Name = "Vaccine Required", Description = "Vaccination required")]
    VaccineRequired = 41,

    [Custom(Name = "Safety Concerns", Description = "Security precautions advised")]
    SafetyConcerns = 42,

    [Custom(Name = "Long Flight", Description = "Long-distance flight expected")]
    LongFlight = 43,

    [Custom(Name = "High Budget", Description = "High estimated cost")]
    HighBudget = 44,

    [Custom(Name = "Travel Restrictions", Description = "Possible travel restrictions")]
    TravelRestrictions = 45
}