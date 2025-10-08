namespace NS.Shared.Enums
{
    public enum Field
    {
        //Society and Government (100)

        [Custom(Name = "Corruption")]
        CorruptionScore = 101,

        [Custom(Name = "Human Development")]
        HDI = 102,

        [Custom(Name = "DM Democracy")]
        DMDemocracyIndex = 103,

        [Custom(Name = "DM Classification")]
        DMClassification = 104,

        [Custom(Name = "The Economist Democracy")]
        EconomistDemocracyIndex = 105,

        [Custom(Name = "The Economist Regime Type")]
        EconomistRegimeType = 106,

        [Custom(Name = "Freedom of Expression")]
        FreedomExpressionIndex = 107,

        [Custom(Name = "Freedom")]
        FreedomScore = 108,

        [Custom(Name = "Censorship")]
        CensorshipIndex = 109,

        [Custom(Name = "Happiness")]
        HappinessIndex = 110,

        //Economy (200)

        [Custom(Name = "OECD")]
        OECD = 201,

        [Custom(Name = "GDP (PPP) per capita")]
        GDP_PPP = 202,

        [Custom(Name = "GDP (Nominal) per capita")]
        GDP_Nominal = 203,

        [Custom(Name = "Economic Freedom")]
        EconomicFreedomIndex = 204,

        //Security and Peace (300)

        [Custom(Name = "TSA Safety")]
        TsaSafetyIndex = 301,

        [Custom(Name = "Numbeo Safety")]
        NumbeoSafetyIndex = 302,

        [Custom(Name = "Global Terrorism")]
        GlobalTerrorismIndex = 303,

        [Custom(Name = "Global Peace")]
        GlobalPeaceIndex = 304,

        //Environment and Health (400)

        [Custom(Name = "Sanitation & Drinking Water")]
        YaleWaterScore = 401,

        [Custom(Name = "Numbeo Pollution")]
        NumbeoPollutionIndex = 402,

        //Mobility and Tourism (500)

        [Custom(Name = "Visa Free")]
        VisaFree = 501,

        [Custom(Name = "International Arrivals")]
        InternationalArrivals = 502,

        [Custom(Name = "Taxi Apps")]
        TaxiApps = 1001,
    }
}