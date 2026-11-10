using NS.Shared.Core.Types;

namespace NS.Shared.Models;

public class WishList(string? id) : MainDocument(new MainIdentity(MainType.WishList, id))
{
    public ISet<WishListEntry> Items { get; set; } = new HashSet<WishListEntry>();

    protected override object?[] EqualityValues => [Id];
}

public class WishListEntry
{
    public string? Id { get; set; }
    public string? RegionCode { get; set; }
    public string? CityCode { get; set; }
    public string? RegionName { get; set; }
    public string? CityName { get; set; }
    public WishlistPhase Phase { get; set; } = WishlistPhase.Phase1;
    public ISet<CheckListItem> CheckList { get; set; } = new HashSet<CheckListItem>();
    public IReadOnlyCollection<ExperienceTag> ExperienceTags { get; set; } = [];
    public IReadOnlyCollection<IntentionTag> IntentionTags { get; set; } = [];
    public IReadOnlyCollection<ConditionsTag> ConditionsTags { get; set; } = [];
    public IReadOnlyCollection<AlertsTag> AlertsTags { get; set; } = [];
}

public enum WishlistPhase
{
    [FieldSettings("1 – Idea / Brainstorm", Description = "You just discovered this destination. You have no concrete plans yet and are simply exploring possibilities.")]
    Phase1 = 1,

    [FieldSettings("2 – Researching / Considering", Description = "You are gathering information, including prices, the best time to visit, and travel requirements.")]
    Phase2 = 2,

    [FieldSettings("3 – Planning / Likely", Description = "This destination is a serious option. You are considering dates, budget, and logistics.")]
    Phase3 = 3,
}

public class CheckListItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool Done { get; set; }
}

public enum ExperienceTag
{
    [FieldSettings("Adventure", Description = "For adventurous or exploratory activities")]
    Adventure = 10,

    [FieldSettings("Relaxing", Description = "For relaxation, spas, and peaceful beaches")]
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

    [FieldSettings("Wildlife", Description = "Wildlife watching and safaris")]
    Wildlife = 18,

    [FieldSettings("Scenic Views", Description = "Scenic landmarks and memorable views")]
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

    [FieldSettings("First Trip", Description = "A dream destination or bucket-list trip")]
    FirstTrip = 24,

    [FieldSettings("Flexible Dates", Description = "Travel dates are flexible")]
    FlexibleDates = 25,

    [FieldSettings("Must Visit", Description = "High priority, likely to happen")]
    MustVisit = 26,

    [FieldSettings("Someday", Description = "A destination to consider in the future")]
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

    [FieldSettings("Seasonal Attraction", Description = "Seasonal events or natural phenomena")]
    SeasonalAttraction = 34,

    [FieldSettings("Crowded Season", Description = "Peak season and crowded periods")]
    CrowdedSeason = 35,
}

public enum AlertsTag
{
    [FieldSettings("Visa Required", Description = "Visa is required")]
    VisaRequired = 40,

    [FieldSettings("Vaccine Required", Description = "Vaccination is required")]
    VaccineRequired = 41,

    [FieldSettings("Safety Concerns", Description = "Security precautions are recommended")]
    SafetyConcerns = 42,

    [FieldSettings("Long Flight", Description = "A long-haul flight is expected")]
    LongFlight = 43,

    [FieldSettings("High Budget", Description = "The estimated cost is high")]
    HighBudget = 44,

    [FieldSettings("Travel Restrictions", Description = "Travel restrictions may apply")]
    TravelRestrictions = 45,
}