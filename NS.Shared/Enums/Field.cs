namespace NS.Shared.Enums
{
    public enum Field
    {
        //Society and Government (100)

        [Custom(Name = "Corruption", Placeholder = "Corruption Perceptions Index (Transparency International)", Description = "CorruptionScoreDesc", ResourceType = typeof(Resources.Enum.Field))]
        CorruptionScore = 101,

        [Custom(Name = "HDI", Placeholder = "Human Development Index (Human Development Reports)", Description = "HDIDesc", ResourceType = typeof(Resources.Enum.Field))]
        HDI = 102,

        [Custom(Name = "Democracy", Placeholder = "Quality of Democracy (Democracy Matrix)", Description = "DMDemocracyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        DMDemocracyIndex = 103,

        [Custom(Name = "Class.", Placeholder = "Classification (Democracy Matrix)", Description = "DMClassificationDesc", ResourceType = typeof(Resources.Enum.Field))]
        DMClassification = 104,

        [Custom(Name = "Democracy", Placeholder = "Democracy Index (The Economist)", Description = "EconomistDemocracyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        EconomistDemocracyIndex = 105,

        [Custom(Name = "Regime", Placeholder = "Regime Type (The Economist)", Description = "EconomistRegimeTypeDesc", ResourceType = typeof(Resources.Enum.Field))]
        EconomistRegimeType = 106,

        [Custom(Name = "Expression", Placeholder = "Freedom of Expression Index (Varieties of Democracy)", Description = "FreedomExpressionIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        FreedomExpressionIndex = 107,

        [Custom(Name = "Freedom", Placeholder = "Freedom in the World Score (Freedom House)", Description = "FreedomScoreDesc", ResourceType = typeof(Resources.Enum.Field))]
        FreedomScore = 108,

        [Custom(Name = "Censorship", Placeholder = "Index on Censorship", Description = "CensorshipIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        CensorshipIndex = 109,

        [Custom(Name = "Happiness", Placeholder = "World Happiness Report", Description = "HappinessIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        HappinessIndex = 110,

        //Economy (200)

        [Custom(Name = "OECD", Placeholder = "The Organisation for Economic Co-operation and Development", Description = "OECDDesc", ResourceType = typeof(Resources.Enum.Field))]
        OECD = 201,

        [Custom(Name = "GDP (PPP)", Placeholder = "GDP (Gross Domestic Product) per capita - PPP", Description = "GDP_PPPDesc", ResourceType = typeof(Resources.Enum.Field))]
        GDP_PPP = 202,

        [Custom(Name = "GDP (Nominal)", Placeholder = "GDP (Gross Domestic Product) per capita - Nominal", Description = "GDP_NominalDesc", ResourceType = typeof(Resources.Enum.Field))]
        GDP_Nominal = 203,

        [Custom(Name = "Economic Freedom", Placeholder = "Index of Economic Freedom (The Heritage Foundation)", Description = "EconomicFreedomIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        EconomicFreedomIndex = 204,

        //Security and Peace (300)

        [Custom(Name = "Safety", Placeholder = "Safety Index (Travel Safe - Abroad)", Description = "TsaSafetyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        TsaSafetyIndex = 301,

        [Custom(Name = "Safety", Placeholder = "Safety Index (Numbeo)", Description = "NumbeoSafetyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        NumbeoSafetyIndex = 302,

        [Custom(Name = "Terrorism", Placeholder = "Global Terrorism Index (Vision of Humanity)", Description = "GlobalTerrorismIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        GlobalTerrorismIndex = 303,

        [Custom(Name = "Peace", Placeholder = "Global Peace Index (Vision of Humanity)", Description = "GlobalPeaceIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        GlobalPeaceIndex = 304,

        //Environment and Health (400)

        [Custom(Name = "Sanitation / Water", Placeholder = "Sanitation & Drinking Water Score (Environmental Performance Index - Yale)", Description = "YaleWaterScoreDesc", ResourceType = typeof(Resources.Enum.Field))]
        YaleWaterScore = 401,

        [Custom(Name = "Pollution", Placeholder = "Pollution Index (Numbeo)", Description = "NumbeoPollutionIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        NumbeoPollutionIndex = 402,

        //Mobility and Tourism (500)

        [Custom(Name = "Passport Index", Placeholder = "The Henley Passport Index (Henley & Partners)", Description = "VisaFreeDesc", ResourceType = typeof(Resources.Enum.Field))]
        VisaFree = 501,

        [Custom(Name = "Tourism Index", Placeholder = "Travel & Tourism Development Index (World Economic Forum)", Description = "TourismIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        TourismIndex = 502,

        //Guide (1000)

        [Custom(Name = "TaxiApps", Placeholder = "TaxiApps", ResourceType = typeof(Resources.Enum.Field))]
        TaxiApps = 1001,

        [Custom(Name = "Languages", Placeholder = "Languages", ResourceType = typeof(Resources.Enum.Field))]
        Languages = 1002,

        [Custom(Name = "Risks", Placeholder = "Risks", ResourceType = typeof(Resources.Enum.Field))]
        Risks = 1003,

        [Custom(Name = "Conflicts", Placeholder = "Conflicts", ResourceType = typeof(Resources.Enum.Field))]
        Conflicts = 1004,

        //Cost of Living (1100)

        /// <summary>
        /// This field is run for all expense fields (it needs to be run for all 5 columns separately).
        /// </summary>
        [Custom(Name = "Renting (City Center)", Placeholder = "Apartment (1 bedroom, City Center)")]
        AptCityCenter = 1102,

        /// <summary>
        /// This field is used to calculate the expense score (run only after the previous field).
        /// </summary>
        [Custom(Name = "Renting (Outside of Center)", Placeholder = "Apartment (1 bedroom, Outside of Center)")]
        AptOutsideCenter = 1103,

        [Custom(Name = "Meal", Placeholder = "Meal (Inexpensive Restaurant)")]
        Meal = 1104,

        [Custom(Name = "Market (Western food)", Placeholder = "Market (2400 calories, Western food types)")]
        MarketWestern = 1105,

        [Custom(Name = "Market (Asian food)", Placeholder = "Market (2400 calories, Asian food types)")]
        MarketAsian = 1106,

        //Other (9000)

        [Custom(Name = "Global Cities", Placeholder = "Global Cities")]
        GlobalCities = 9001,

        [Custom(Name = "TSA Cities", Placeholder = "TSA Cities")]
        TSACities = 9002,

        [Custom(Name = "Capital Cities", Placeholder = "Capital Cities")]
        CapitalCities = 9003,
    }
}