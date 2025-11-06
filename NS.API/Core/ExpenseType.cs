namespace NS.API.Core
{
    public enum ExpenseType
    {
        [Custom(Name = "Apartment", Description = "Apartment (1 bedroom) in City Centre")]
        AptCityCenter = 1,

        [Custom(Name = "Apartment", Description = "Apartment (1 bedroom) Outside of Centre")]
        AptOutsideCenter = 2,

        [Custom(Name = "Meal", Description = "Meal, Inexpensive Restaurant")]
        Meal = 3,

        [Custom(Name = "Market", Description = "Recommended Minimum Amount of Money for food (2400 calories, Western food types)")]
        MarketWestern = 4,

        [Custom(Name = "Market", Description = "Recommended Minimum Amount of Money for food (2400 calories, Asian food types)")]
        MarketAsian = 5
    }
}