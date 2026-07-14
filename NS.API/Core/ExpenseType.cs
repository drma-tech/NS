namespace NS.API.Core
{
    public enum ExpenseType
    {
        [FieldSettings("Apartment", Description = "1 Bedroom Apartment in City Centre")]
        AptCityCenter = 1,

        [FieldSettings("Apartment", Description = "1 Bedroom Apartment Outside of City Centre")]
        AptOutsideCenter = 2,

        [FieldSettings("Meal", Description = "Meal at an Inexpensive Restaurant")]
        Meal = 3,

        [FieldSettings("Market", Description = "Recommended Minimum Amount of Money for food (2400 calories, Western food types)")]
        MarketWestern = 4,

        [FieldSettings("Market", Description = "Recommended Minimum Amount of Money for food (2400 calories, Asian food types)")]
        MarketAsian = 5
    }
}