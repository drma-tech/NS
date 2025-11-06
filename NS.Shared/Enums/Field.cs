namespace NS.Shared.Enums
{
    public enum Field
    {
        //Society and Government (100)

        [Custom(Name = "Corruption", Placeholder = "Corruption", Description = "Scoring 180 countries around the world, the Corruption Perceptions Index is the leading global indicator of public sector corruption.")]
        CorruptionScore = 101,

        [Custom(Name = "HDI", Placeholder = "Human Development")]
        HDI = 102,

        [Custom(Name = "Democracy", Placeholder = "DM Democracy")]
        DMDemocracyIndex = 103,

        [Custom(Name = "Class.", Placeholder = "DM Classification")]
        DMClassification = 104,

        [Custom(Name = " Democracy", Placeholder = "The Economist Democracy")]
        EconomistDemocracyIndex = 105,

        [Custom(Name = "Regime", Placeholder = "The Economist Regime Type")]
        EconomistRegimeType = 106,

        [Custom(Name = "Expression", Placeholder = "Freedom of Expression")]
        FreedomExpressionIndex = 107,

        [Custom(Name = "Freedom", Placeholder = "Freedom")]
        FreedomScore = 108,

        [Custom(Name = "Censorship", Placeholder = "Censorship")]
        CensorshipIndex = 109,

        [Custom(Name = "Happiness", Placeholder = "Happiness")]
        HappinessIndex = 110,

        //Economy (200)

        [Custom(Name = "OECD", Placeholder = "Organisation for Economic Co-operation and Development")]
        OECD = 201,

        [Custom(Name = "GDP (PPP)", Placeholder = "GDP (PPP) per capita")]
        GDP_PPP = 202,

        [Custom(Name = "GDP (Nominal)", Placeholder = "GDP (Nominal) per capita")]
        GDP_Nominal = 203,

        [Custom(Name = "Economic Freedom", Placeholder = "Economic Freedom")]
        EconomicFreedomIndex = 204,

        //Security and Peace (300)

        [Custom(Name = "Safety", Placeholder = "TSA Safety")]
        TsaSafetyIndex = 301,

        [Custom(Name = "Safety", Placeholder = "Numbeo Safety")]
        NumbeoSafetyIndex = 302,

        [Custom(Name = "Terrorism", Placeholder = "Global Terrorism")]
        GlobalTerrorismIndex = 303,

        [Custom(Name = "Peace", Placeholder = "Global Peace")]
        GlobalPeaceIndex = 304,

        //Environment and Health (400)

        [Custom(Name = "Sanitation / Water", Placeholder = "Sanitation & Drinking Water")]
        YaleWaterScore = 401,

        [Custom(Name = "Pollution", Placeholder = "Numbeo Pollution")]
        NumbeoPollutionIndex = 402,

        //Mobility and Tourism (500)

        [Custom(Name = "Visa Free", Placeholder = "Visa Free")]
        VisaFree = 501,

        [Custom(Name = "Tourism Index", Placeholder = "Adventure Tourism Development Index (ATDI)")]
        TourismIndex = 502,

        //Guide (1000)

        [Custom(Name = "Taxi Apps", Placeholder = "Taxi Apps")]
        TaxiApps = 1001,

        [Custom(Name = "Languages", Placeholder = "Languages")]
        Languages = 1002,

        //Cost of Living (1100)

        [Custom(Name = "Renting (City Center)", Placeholder = "Apartment (1 bedroom, City Center)")]
        AptCityCenter = 1102,

        [Custom(Name = "Renting (Outside of Center)", Placeholder = "Apartment (1 bedroom, Outside of Center)")]
        AptOutsideCenter = 1103,

        [Custom(Name = "Meal", Placeholder = "Meal (Inexpensive Restaurant)")]
        Meal = 1104,

        [Custom(Name = "Market (Western food)", Placeholder = "Market (2400 calories, Western food types)")]
        MarketWestern = 1105,

        [Custom(Name = "Market (Asian food)", Placeholder = "Market (2400 calories, Asian food types)")]
        MarketAsian = 1106,
    }
}