namespace NS.Shared.Models.Country
{
    public class RegionData() : ProtectedMainDocument(DocumentType.Country)
    {
        //description
        //https://en.wikivoyage.org/api/rest_v1/page/summary/United_States_of_America

        //do the following translations

        //China
        //United States
        //Germany
        //United Kingdom
        //France
        //Australia
        //Canada
        //Russia
        //Italy
        //India

        public static double? CalculateAverage(List<double?> values)
        {
            if (values.Count == 0) return null;

            int filledCount = values.Count(v => v.HasValue);

            // If less than 50% is filled, it returns null
            if (filledCount <= values.Count / 2)
                return null;

            double sum = values.Where(v => v.HasValue).Sum(v => v!.Value);

            return Math.Round(sum / filledCount, 2);
        }

        public double? GetAverageScore()
        {
            double totalScore = 0;
            int categories = 5;

            var score1 = GetSocietyAndGovernmentScore();
            var score2 = GetEconomyScore();
            var score3 = GetSecurityAndPeaceScore();
            var score4 = GetEnvironmentAndHealthScore();
            var score5 = GetMobilityAndTourismScore();

            if (score1 == null || score2 == null || score3 == null || score4 == null || score5 == null) return null;

            totalScore += score1.Value;
            totalScore += score2.Value;
            totalScore += score3.Value;
            totalScore += score4.Value;
            totalScore += score5.Value;

            return Math.Round(totalScore / categories, 2);
        }

        #region Scores

        /// <summary>
        /// Society and Government (100)
        /// </summary>

        public double? GetSocietyAndGovernmentScore()
        {
            var scores = new List<double?>
            {
                CorruptionScore,
                HDI,
                DMDemocracyIndex,
                DMClassification.HasValue ? (double)DMClassification : null,
                EconomistDemocracyIndex,
                EconomistRegimeType.HasValue ? (double)EconomistRegimeType : null,
                FreedomExpressionIndex,
                FreedomScore,
                CensorshipIndex,
                HappinessIndex
            };

            return CalculateAverage(scores);
        }

        [Custom(Name = "Corruption", Placeholder = "Corruption Perceptions Index (Transparency International)", Description = "Shows how honest people believe their leaders and public officials are, helping to understand how trust and fairness work in a country.")]
        public double? CorruptionScore { get; set; }

        [Custom(Name = "HDI", Placeholder = "Human Development Index (Human Development Reports)", Description = "Measures how well people can live healthy lives, gain education, and have a decent standard of living, showing overall human development.")]
        public double? HDI { get; set; }

        [Custom(Name = "Democracy", Placeholder = "Quality of Democracy (Democracy Matrix)", Description = "Evaluates how open and fair a country's political system is, including how people participate in choosing leaders and influencing decisions.")]
        public double? DMDemocracyIndex { get; set; }

        [Custom(Name = "Class.", Placeholder = "Classification (Democracy Matrix)", Description = "Classifies countries based on the type of government they have and how citizens are involved in decision-making processes.")]
        public DMClassification? DMClassification { get; set; }

        [Custom(Name = "Democracy", Placeholder = "Democracy Index (The Economist)", Description = "Measures how well citizens can vote, express opinions freely, enjoy rights, and participate in a government that is accountable and fair.")]
        public double? EconomistDemocracyIndex { get; set; }

        [Custom(Name = "Regime", Placeholder = "Regime Type (The Economist)", Description = "Shows the kind of government a country has and how it organizes power, laws, and the participation of its people.")]
        public EconomistRegimeType? EconomistRegimeType { get; set; }

        [Custom(Name = "Expression", Placeholder = "Freedom of Expression Index (Varieties of Democracy)", Description = "Indicates how freely people can discuss ideas, politics, and culture, and how independent media and educational spaces are in sharing opinions.")]
        public double? FreedomExpressionIndex { get; set; }

        [Custom(Name = "Freedom", Placeholder = "Freedom in the World Score (Freedom House)", Description = "Shows how much political rights and civil liberties people enjoy, including voting, speaking freely, and participating in society without fear.")]
        public double? FreedomScore { get; set; }

        [Custom(Name = "Censorship", Placeholder = "Index on Censorship", Description = "Measures how free people are to access and share information, including the extent of restrictions on media, ideas, and online content.")]
        public double? CensorshipIndex { get; set; }

        [Custom(Name = "Happiness", Placeholder = "World Happiness Report", Description = "Indicates how happy people feel in their daily life, considering their well-being, opportunities, community, and quality of life.")]
        public double? HappinessIndex { get; set; }

        //LGBTQ Equality Index //https://www.equaldex.com/equality-index
        //Global Gender Gap Report 2025 //https://reports.weforum.org/docs/WEF_GGGR_2025.pdf (extracted to json)
        //RuleOfLawIndex(World Justice Project)
        //PressFreedomIndex //https://rsf.org/en/index
        //https://www.transparency.org/en/cpi/2023

        /// <summary>
        /// Economy (200)
        /// </summary>

        public double? GetEconomyScore()
        {
            var scores = new List<double?>
            {
                OECD ? 7.5 : 2.5,
                EconomicFreedomIndex,
                GDP_PPP,
                GDP_Nominal
            };

            return CalculateAverage(scores);
        }

        [Custom(Name = "OECD", Placeholder = "The Organisation for Economic Co-operation and Development", Description = "Shows whether a country is part of an international group where nations share knowledge, ideas, and strategies to improve public policies and economic practices.")]
        public bool OECD { get; set; } = false;

        [Custom(Name = "GDP (PPP)", Placeholder = "GDP (Gross Domestic Product) per capita - PPP", Description = "Measures the average economic output per person in a country, adjusted for differences in cost of living, showing how much people can buy and their relative standard of living.")]
        public double? GDP_PPP { get; set; } //pra quem ganha e gasta na moeda interna

        [Custom(Name = "GDP (Nominal)", Placeholder = "GDP (Gross Domestic Product) per capita - Nominal", Description = "Shows the total value of goods and services produced per person in a country, measured in current US dollars, helping to compare countries' economies globally.")]
        public double? GDP_Nominal { get; set; } //pra quem ganha em moeda externa, investe em outro pais ou simplesmente quer comparar o pais a nivel global

        [Custom(Name = "Economic Freedom", Placeholder = "Index of Economic Freedom (The Heritage Foundation)", Description = "Indicates how much people and businesses can make economic choices freely, including starting businesses, trading, and using resources, which affects growth and prosperity.")]
        public double? EconomicFreedomIndex { get; set; }

        //GiniIndex(World Bank – inequality measure) //https://data.worldbank.org/indicator/SI.POV.GINI //https://worldpopulationreview.com/country-rankings/gini-coefficient-by-country
        //CompetitivenessIndex(World Economic Forum – Global Competitiveness Report) //https://en.wikipedia.org/wiki/WEF_Global_Competitiveness_Report
        //EaseOfDoingBusiness(World Bank — OBS: descontinuado em 2021, mas substituído pelo Business Ready Project) o novo, tem poucos paises, o velho nao tem um index (apenas um ranking)
        //https://www.numbeo.com/cost-of-living/

        ////https://wageindicator.org/salary/minimum-wage/minimum-wages-per-country
        ////https://countryeconomy.com/national-minimum-wage
        //public decimal? MinimalWage { get; set; }

        /// <summary>
        /// Security and Peace (300)
        /// </summary>

        public double? GetSecurityAndPeaceScore()
        {
            var scores = new List<double?>
            {
                TsaSafetyIndex,
                NumbeoSafetyIndex,
                GlobalTerrorismIndex,
                GlobalPeaceIndex
            };

            return CalculateAverage(scores);
        }

        public double? GetSafetyScore()
        {
            double totalScore = 0;
            int categories = 3;

            var score1 = GetSecurityAndPeaceScore();
            var score2 = (int?)ConflictLevel;
            var score3 = GetRisksScore();

            if (score1 == null || score2 == null || score3 == null) return null;

            totalScore += score1.Value;
            totalScore += score2.Value;
            totalScore += score3.Value;

            return Math.Round(totalScore / categories, 2);
        }

        private double? GetRisksScore()
        {
            double totalScore = 0;
            int categories = 8;

            var score1 = (double?)Risks?.TransportTaxis;
            var score2 = (double?)Risks?.Pickpockets;
            var score3 = (double?)Risks?.NaturalDisasters;
            var score4 = (double?)Risks?.Mugging;
            var score5 = (double?)Risks?.Terrorism;
            var score6 = (double?)Risks?.Scams;
            var score7 = (double?)Risks?.WomenTravelers;
            var score8 = (double?)Risks?.TapWater;

            if (score1.HasValue) totalScore += score1.Value.Invert(1, 3).Rescale(1, 3, 0, 10);
            if (score2.HasValue) totalScore += score2.Value.Invert(1, 3).Rescale(1, 3, 0, 10);
            if (score3.HasValue) totalScore += score3.Value.Invert(1, 3).Rescale(1, 3, 0, 10);
            if (score4.HasValue) totalScore += score4.Value.Invert(1, 3).Rescale(1, 3, 0, 10);
            if (score5.HasValue) totalScore += score5.Value.Invert(1, 3).Rescale(1, 3, 0, 10);
            if (score6.HasValue) totalScore += score6.Value.Invert(1, 3).Rescale(1, 3, 0, 10);
            if (score7.HasValue) totalScore += score7.Value.Invert(1, 3).Rescale(1, 3, 0, 10);
            if (score8.HasValue) totalScore += score8.Value.Invert(1, 3).Rescale(1, 3, 0, 10);

            return Math.Round(totalScore / categories, 2);
        }

        [Custom(Name = "Safety", Placeholder = "Safety Index (Travel Safe - Abroad)", Description = "Shows how safe it is for people to live or travel in a country, taking into account risks such as crime, property theft, social tensions, and general public safety.")]
        public double? TsaSafetyIndex { get; set; }

        //todo: tsa tips
        [Custom(Name = "Safety", Placeholder = "Safety Index (Numbeo)", Description = "Estimates the overall level of crime in a city or country, helping to understand how safe everyday life is for residents and visitors.")]
        public double? NumbeoSafetyIndex { get; set; }

        [Custom(Name = "Terrorism", Placeholder = "Global Terrorism Index (Vision of Humanity)", Description = "Measures the impact of terrorism on a country, including incidents, injuries, and fatalities, giving a sense of how likely such events are to affect daily life.")]
        public double? GlobalTerrorismIndex { get; set; }

        [Custom(Name = "Peace", Placeholder = "Global Peace Index (Vision of Humanity)", Description = "Indicates how peaceful a country is, considering safety, conflict levels, and social stability, to show how calm or tense life can be in that country.")]
        public double? GlobalPeaceIndex { get; set; }

        //CrimeIndex(Numbeo – atualizado 2x/ano) //https://www.numbeo.com/crime/ not necessary - (its just the safet index oposite)
        //ConflictRiskIndex(ACLED datasets ou PRIO conflict data) //https://acleddata.com/platform/weekly-conflict-index (teoricamente atualizado toda semana)

        /// <summary>
        /// Environment and Health (400)
        /// </summary>

        public double? GetEnvironmentAndHealthScore()
        {
            var scores = new List<double?>
            {
                YaleWaterScore,
                NumbeoPollutionIndex
            };

            return CalculateAverage(scores);
        }

        [Custom(Name = "Sanitation / Water", Placeholder = "Sanitation & Drinking Water Score (Environmental Performance Index - Yale)", Description = "Shows how well a country provides clean drinking water and proper sanitation, helping people stay healthy and avoid diseases.")]
        public double? YaleWaterScore { get; set; }

        [Custom(Name = "Pollution", Placeholder = "Pollution Index (Numbeo)", Description = "Indicates the level of pollution in a city or country, including air, water, noise, waste, and overall cleanliness, helping to understand how healthy the environment is.")]
        public double? NumbeoPollutionIndex { get; set; }

        //AirQualityIndex(IQAir – anual por país) //https://www.iqair.com/us/world-most-polluted-countries
        //ClimateRiskIndex(Germanwatch – Global Climate Risk Index) - mortes por temperatura, nao eh importante.
        //HealthcareIndex(Numbeo ou The Lancet Healthcare Access & Quality Index) //https://www.numbeo.com/health-care/

        /// <summary>
        /// Mobility and Tourism (500)
        /// </summary>

        public double? GetMobilityAndTourismScore()
        {
            var scores = new List<double?>
            {
                CalculatePassportIndex(),
                TourismIndex
            };

            return CalculateAverage(scores);
        }

        public double? CalculatePassportIndex()
        {
            int existingPassports = 200;

            if (VisaFree.HasValue)
            {
                if (VisaFree.Value >= existingPassports)
                {
                    return 10.0;
                }
                else
                {
                    return (double)VisaFree.Value / existingPassports * 10;
                }
            }
            return null;
        }

        [Custom(Name = "Passport Index", Placeholder = "The Henley Passport Index (Henley & Partners)", Description = "Shows how many countries a passport holder can visit without needing a visa, indicating travel freedom and ease of international mobility.")]
        public int? VisaFree { get; set; }

        [Custom(Name = "Tourism Index", Placeholder = "Travel & Tourism Development Index (World Economic Forum)", Description = "Measures how well a country supports travel and tourism, including facilities, policies, and opportunities, showing how easy and enjoyable it is to visit.")]
        public double? TourismIndex { get; set; }

        //AirConnectivityIndex(IATA – yearly data) - pesquisar depois. dificil de achar algo com sentido
        //HotelPriceIndex(Numbeo / HPI from Hotels.com, anual) //whatever, vou usar o preco do numbeo msm

        #endregion Scores

        #region Guide

        //https://bobthetravelnerd.com/the-best-ride-hailing-app-in-every-country-on-earth/
        //https://johnnyafrica.com/ride-hailing-apps-in-all-countries/?utm_source=chatgpt.com

        [Custom(Name = "Taxi Apps", Placeholder = "Taxi Apps")]
        public HashSet<TaxiApp> TaxiApps { get; set; } = [];

        [Custom(Name = "Languages", Placeholder = "Languages")]
        public HashSet<Language> Languages { get; set; } = [];

        [Custom(Name = "Risks", Placeholder = "Risks")]
        public Risks? Risks { get; set; }

        [Custom(Name = "Conflict Level", Placeholder = "Current level of conflict in the region", Description = "Conflict Index provides a singular measure of conflict intensity in every country in the world.")]
        public ConflictLevel? ConflictLevel { get; set; } = Enums.ConflictLevel.Minimal;

        [Custom(Name = "Conflict Forecast", Placeholder = "Conflict Forecast for the Coming Months")]
        public ConflictForecast? ConflictForecast { get; set; }

        #endregion Guide

        #region Cost of Living

        public double? GetCostOfLivingScore()
        {
            var scores = new List<double?>
            {
                AptCityCenter?.Score,
                AptOutsideCenter?.Score,
                Meal?.Score,
                MarketWestern?.Score,
                MarketAsian?.Score
            };

            return CalculateAverage(scores);
        }

        [Custom(Name = "Renting (Center)", Placeholder = "Apartment (1 bedroom, City Center)")]
        public PriceRange? AptCityCenter { get; set; }

        [Custom(Name = "Renting (Outside)", Placeholder = "Apartment (1 bedroom, Outside of Center)")]
        public PriceRange? AptOutsideCenter { get; set; }

        [Custom(Name = "Meal", Placeholder = "Meal (Inexpensive Restaurant)")]
        public PriceRange? Meal { get; set; }

        [Custom(Name = "Market (Western)", Placeholder = "Market (2400 calories, Western food types)")]
        public PriceRange? MarketWestern { get; set; }

        [Custom(Name = "Market (Asian)", Placeholder = "Market (2400 calories, Asian food types)")]
        public PriceRange? MarketAsian { get; set; }

        #endregion Cost of Living

        public HashSet<string> Cities { get; set; } = [];
    }

    public class PriceRange
    {
        public decimal? Min { get; set; }
        public decimal? Avg { get; set; }
        public decimal? Max { get; set; }
        public double? Score { get; set; }
    }

    public class Risks
    {
        [Custom(Name = "Transport & Taxis")]
        public Level? TransportTaxis { get; set; } = Level.Low;

        [Custom(Name = "Pickpockets")]
        public Level? Pickpockets { get; set; } = Level.Medium;

        [Custom(Name = "Natural Disasters")]
        public Level? NaturalDisasters { get; set; } = Level.Low;

        [Custom(Name = "Mugging")]
        public Level? Mugging { get; set; } = Level.High;

        [Custom(Name = "Terrorism")]
        public Level? Terrorism { get; set; } = Level.Medium;

        [Custom(Name = "Scams")]
        public Level? Scams { get; set; } = Level.High;

        [Custom(Name = "Women Travelers")]
        public Level? WomenTravelers { get; set; } = Level.Low;

        [Custom(Name = "Tap Water")]
        public Level? TapWater { get; set; } = Level.Low;
    }
}