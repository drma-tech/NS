namespace NS.Shared.Enums
{
    public enum Field
    {
        //Society and Government (100)

        [Custom(Name = "Corruption", Placeholder = "Corruption Perceptions Index (Transparency International)", Description = "Shows how honest people believe their leaders and public officials are, helping to understand how trust and fairness work in a country.")]
        CorruptionScore = 101,

        [Custom(Name = "HDI", Placeholder = "Human Development Index (Human Development Reports)", Description = "Measures how well people can live healthy lives, gain education, and have a decent standard of living, showing overall human development.")]
        HDI = 102,

        [Custom(Name = "Democracy", Placeholder = "Quality of Democracy (Democracy Matrix)", Description = "Evaluates how open and fair a country's political system is, including how people participate in choosing leaders and influencing decisions.")]
        DMDemocracyIndex = 103,

        [Custom(Name = "Class.", Placeholder = "Classification (Democracy Matrix)", Description = "Classifies countries based on the type of government they have and how citizens are involved in decision-making processes.")]
        DMClassification = 104,

        [Custom(Name = "Democracy", Placeholder = "Democracy Index (The Economist)", Description = "Measures how well citizens can vote, express opinions freely, enjoy rights, and participate in a government that is accountable and fair.")]
        EconomistDemocracyIndex = 105,

        [Custom(Name = "Regime", Placeholder = "Regime Type (The Economist)", Description = "Shows the kind of government a country has and how it organizes power, laws, and the participation of its people.")]
        EconomistRegimeType = 106,

        [Custom(Name = "Expression", Placeholder = "Freedom of Expression Index (Varieties of Democracy)", Description = "Indicates how freely people can discuss ideas, politics, and culture, and how independent media and educational spaces are in sharing opinions.")]
        FreedomExpressionIndex = 107,

        [Custom(Name = "Freedom", Placeholder = "Freedom in the World Score (Freedom House)", Description = "Shows how much political rights and civil liberties people enjoy, including voting, speaking freely, and participating in society without fear.")]
        FreedomScore = 108,

        [Custom(Name = "Censorship", Placeholder = "Index on Censorship", Description = "Measures how free people are to access and share information, including the extent of restrictions on media, ideas, and online content.")]
        CensorshipIndex = 109,

        [Custom(Name = "Happiness", Placeholder = "World Happiness Report", Description = "Indicates how happy people feel in their daily life, considering their well-being, opportunities, community, and quality of life.")]
        HappinessIndex = 110,

        //Economy (200)

        [Custom(Name = "OECD", Placeholder = "The Organisation for Economic Co-operation and Development", Description = "Shows whether a country is part of an international group where nations share knowledge, ideas, and strategies to improve public policies and economic practices.")]
        OECD = 201,

        [Custom(Name = "GDP (PPP)", Placeholder = "GDP (Gross Domestic Product) per capita - PPP", Description = "Measures the average economic output per person in a country, adjusted for differences in cost of living, showing how much people can buy and their relative standard of living.")]
        GDP_PPP = 202,

        [Custom(Name = "GDP (Nominal)", Placeholder = "GDP (Gross Domestic Product) per capita - Nominal", Description = "Shows the total value of goods and services produced per person in a country, measured in current US dollars, helping to compare countries' economies globally.")]
        GDP_Nominal = 203,

        [Custom(Name = "Economic Freedom", Placeholder = "Index of Economic Freedom (The Heritage Foundation)", Description = "Indicates how much people and businesses can make economic choices freely, including starting businesses, trading, and using resources, which affects growth and prosperity.")]
        EconomicFreedomIndex = 204,

        //Security and Peace (300)

        [Custom(Name = "Safety", Placeholder = "Safety Index (Travel Safe - Abroad)", Description = "Shows how safe it is for people to live or travel in a country, taking into account risks such as crime, property theft, social tensions, and general public safety.")]
        TsaSafetyIndex = 301,

        [Custom(Name = "Safety", Placeholder = "Safety Index (Numbeo)", Description = "Estimates the overall level of crime in a city or country, helping to understand how safe everyday life is for residents and visitors.")]
        NumbeoSafetyIndex = 302,

        [Custom(Name = "Terrorism", Placeholder = "Global Terrorism Index (Vision of Humanity)", Description = "Measures the impact of terrorism on a country, including incidents, injuries, and fatalities, giving a sense of how likely such events are to affect daily life.")]
        GlobalTerrorismIndex = 303,

        [Custom(Name = "Peace", Placeholder = "Global Peace Index (Vision of Humanity)", Description = "Indicates how peaceful a country is, considering safety, conflict levels, and social stability, to show how calm or tense life can be in that country.")]
        GlobalPeaceIndex = 304,

        //Environment and Health (400)

        [Custom(Name = "Sanitation / Water", Placeholder = "Sanitation & Drinking Water Score (Environmental Performance Index - Yale)", Description = "Shows how well a country provides clean drinking water and proper sanitation, helping people stay healthy and avoid diseases.")]
        YaleWaterScore = 401,

        [Custom(Name = "Pollution", Placeholder = "Pollution Index (Numbeo)", Description = "Indicates the level of pollution in a city or country, including air, water, noise, waste, and overall cleanliness, helping to understand how healthy the environment is.")]
        NumbeoPollutionIndex = 402,

        //Mobility and Tourism (500)

        [Custom(Name = "Passport Index", Placeholder = "The Henley Passport Index (Henley & Partners)", Description = "Shows how many countries a passport holder can visit without needing a visa, indicating travel freedom and ease of international mobility.")]
        VisaFree = 501,

        [Custom(Name = "Tourism Index", Placeholder = "Travel & Tourism Development Index (World Economic Forum)", Description = "Measures how well a country supports travel and tourism, including facilities, policies, and opportunities, showing how easy and enjoyable it is to visit.")]
        TourismIndex = 502,

        //Guide (1000)

        [Custom(Name = "Taxi Apps", Placeholder = "Taxi Apps")]
        TaxiApps = 1001,

        [Custom(Name = "Languages", Placeholder = "Languages")]
        Languages = 1002,

        [Custom(Name = "Risks", Placeholder = "Risks")]
        Risks = 1003,

        [Custom(Name = "Conflicts", Placeholder = "Conflicts")]
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