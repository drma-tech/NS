namespace NS.Shared.Enums
{
    public enum Field
    {
        //Society and Government (100)

        [FieldSettings("Corruption", Placeholder = "Corruption Perceptions Index (Transparency International)", Description = "CorruptionScoreDesc", ResourceType = typeof(Translations.Enum.Field))]
        CorruptionScore = 101,

        [FieldSettings("HDI", Placeholder = "Human Development Index (Human Development Reports)", Description = "HDIDesc", ResourceType = typeof(Translations.Enum.Field))]
        HDI = 102,

        [FieldSettings("Democracy", Placeholder = "Quality of Democracy (Democracy Matrix)", Description = "DMDemocracyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        DMDemocracyIndex = 103,

        [FieldSettings("Democracy", Placeholder = "Democracy Index (The Economist)", Description = "EconomistDemocracyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        EconomistDemocracyIndex = 105,

        [FieldSettings("Expression", Placeholder = "Freedom of Expression Index (Varieties of Democracy)", Description = "FreedomExpressionIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        FreedomExpressionIndex = 107,

        [FieldSettings("Freedom", Placeholder = "Freedom in the World Score (Freedom House)", Description = "FreedomScoreDesc", ResourceType = typeof(Translations.Enum.Field))]
        FreedomScore = 108,

        [FieldSettings("Censorship", Placeholder = "Index on Censorship", Description = "CensorshipIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        CensorshipIndex = 109,

        [FieldSettings("Happiness", Placeholder = "World Happiness Report", Description = "HappinessIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        HappinessIndex = 110,

        //Economy (200)

        [FieldSettings("OECD", Placeholder = "The Organisation for Economic Co-operation and Development", Description = "OECDDesc", ResourceType = typeof(Translations.Enum.Field))]
        OECD = 201,

        [FieldSettings("GDP (PPP)", Placeholder = "GDP (Gross Domestic Product) per capita - PPP", Description = "GDP_PPPDesc", ResourceType = typeof(Translations.Enum.Field))]
        GDP_PPP = 202,

        [FieldSettings("GDP (Nominal)", Placeholder = "GDP (Gross Domestic Product) per capita - Nominal", Description = "GDP_NominalDesc", ResourceType = typeof(Translations.Enum.Field))]
        GDP_Nominal = 203,

        [FieldSettings("Economic Freedom", Placeholder = "Index of Economic Freedom (The Heritage Foundation)", Description = "EconomicFreedomIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        EconomicFreedomIndex = 204,

        [FieldSettings("Cashless Index", Placeholder = "Cash Index (FOREX)", Description = "CashlessIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        CashlessIndex = 205,

        //Security and Peace (300)

        [FieldSettings("Safety", Placeholder = "Safety Index (Travel Safe - Abroad)", Description = "TsaSafetyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        TsaSafetyIndex = 301,

        [FieldSettings("Safety", Placeholder = "Safety Index (Numbeo)", Description = "NumbeoSafetyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        NumbeoSafetyIndex = 302,

        [FieldSettings("Terrorism", Placeholder = "Global Terrorism Index (Vision of Humanity)", Description = "GlobalTerrorismIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        GlobalTerrorismIndex = 303,

        [FieldSettings("Peace", Placeholder = "Global Peace Index (Vision of Humanity)", Description = "GlobalPeaceIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        GlobalPeaceIndex = 304,

        //Environment and Health (400)

        [FieldSettings("Sanitation / Water", Placeholder = "Sanitation & Drinking Water Score (Environmental Performance Index - Yale)", Description = "YaleWaterScoreDesc", ResourceType = typeof(Translations.Enum.Field))]
        YaleWaterScore = 401,

        [FieldSettings("Pollution", Placeholder = "Pollution Index (Numbeo)", Description = "NumbeoPollutionIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        NumbeoPollutionIndex = 402,

        [FieldSettings("Air Quality", Placeholder = "World Air Quality Report (IQAir)", Description = "AirQualityDesc", ResourceType = typeof(Translations.Enum.Field))]
        AirQuality = 403,

        [FieldSettings("Health Care", Placeholder = "Health Care Index (CEOWORLD)", Description = "HealthCareIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        HealthCareIndex = 404,

        [FieldSettings("Annual Temperature", Placeholder = "Average annual surface temperature (World Bank Group)", Description = "AnnualTemperatureDesc", ResourceType = typeof(Translations.Enum.Field))]
        AnnualTemperature = 405,

        //Mobility and Tourism (500)

        [FieldSettings("Passport Index", Placeholder = "The Henley Passport Index (Henley & Partners)", Description = "VisaFreeDesc", ResourceType = typeof(Translations.Enum.Field))]
        VisaFree = 501,

        [FieldSettings("Tourism Index", Placeholder = "Travel & Tourism Development Index (World Economic Forum)", Description = "TourismIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        TourismIndex = 502,

        [FieldSettings("Air Connectivity Index", Placeholder = "Air Connectivity Index (International Air Transport Association)", Description = "AirConnectivityIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        AirConnectivityIndex = 503,

        [FieldSettings("Sustainable Mobility Index", Placeholder = "Global Sustainable Mobility Index (GSMI - SuM4All)", Description = "SustainableMobilityIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        SustainableMobilityIndex = 504,

        //Guide (1000)

        [FieldSettings("Taxis", Placeholder = "Taxis", ResourceType = typeof(Translations.Enum.Field))]
        Taxis = 1001,

        [FieldSettings("Languages", Placeholder = "Languages", ResourceType = typeof(Translations.Enum.Field))]
        Languages = 1002,

        [FieldSettings("Risks", Placeholder = "Risks", ResourceType = typeof(Translations.Enum.Field))]
        Risks = 1003,

        [FieldSettings("Tipping", Placeholder = "Tipping", ResourceType = typeof(Translations.Enum.Field))]
        Tipping = 1005,

        [FieldSettings("Broadband Speed", Placeholder = "Average broadband speed in Mbps", ResourceType = typeof(Translations.Enum.Field))]
        BroadbandSpeed = 1006,

        [FieldSettings("Tax", Placeholder = "Tax", ResourceType = typeof(Translations.Enum.Field))]
        Tax = 1007,

        [FieldSettings("EmergencyNumbers", Placeholder = "EmergencyNumbers", ResourceType = typeof(Translations.Enum.Field))]
        EmergencyNumbers = 1008,

        [FieldSettings("Currencies", Placeholder = "Currencies", ResourceType = typeof(Translations.Enum.Field))]
        Currencies = 1009,

        [FieldSettings("TravelRequirements", Placeholder = "TravelRequirements", ResourceType = typeof(Translations.Enum.Field))]
        TravelRequirements = 1010,

        [FieldSettings("Religions", Placeholder = "Religions", ResourceType = typeof(Translations.Enum.Field))]
        Religions = 1011,

        [FieldSettings("Electricity", Placeholder = "Electricity", ResourceType = typeof(Translations.Enum.Field))]
        Electricity = 1012,

        //Lifestyle (1100)

        [FieldSettings("Income", Placeholder = "Average Monthly Income (worlddata)", Description = "IncomeDesc", ResourceType = typeof(Translations.Enum.Field))]
        Income = 1101,

        /// <summary>
        /// This field is run for all expense fields (it needs to be run for all 5 columns separately).
        /// </summary>
        [FieldSettings("Renting (City Center)", Placeholder = "Apartment (1 bedroom, City Center)")]
        AptCityCenter = 1102,

        /// <summary>
        /// This field is used to calculate the expense score (run only after the previous field).
        /// </summary>
        [FieldSettings("Renting (Outside of Center)", Placeholder = "Apartment (1 bedroom, Outside of Center)")]
        AptOutsideCenter = 1103,

        [FieldSettings("Meal", Placeholder = "Meal (Inexpensive Restaurant)")]
        Meal = 1104,

        [FieldSettings("Market (Western food)", Placeholder = "Market (2400 calories, Western food types)")]
        MarketWestern = 1105,

        [FieldSettings("Market (Asian food)", Placeholder = "Market (2400 calories, Asian food types)")]
        MarketAsian = 1106,

        //Other (9000)

        [FieldSettings("Global Cities", Placeholder = "Global Cities")]
        GlobalCities = 9001,

        [FieldSettings("TSA Cities", Placeholder = "TSA Cities")]
        TSACities = 9002,

        [FieldSettings("Capital Cities", Placeholder = "Capital Cities")]
        CapitalCities = 9003,
    }
}