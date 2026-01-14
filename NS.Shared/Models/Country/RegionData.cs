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

        [Custom(Name = "Corruption", Placeholder = "Corruption Perceptions Index (Transparency International)", Description = "Scoring 180 countries around the world, the Corruption Perceptions Index is the leading global indicator of public sector corruption.")]
        public double? CorruptionScore { get; set; }

        [Custom(Name = "HDI", Placeholder = "Human Development Index (Human Development Reports)")]
        public double? HDI { get; set; }

        [Custom(Name = "Democracy", Placeholder = "Quality of Democracy (Democracy Matrix)")]
        public double? DMDemocracyIndex { get; set; }

        [Custom(Name = "Class.", Placeholder = "Classification (Democracy Matrix)")]
        public DMClassification? DMClassification { get; set; }

        [Custom(Name = "Democracy", Placeholder = "Democracy Index (The Economist)")]
        public double? EconomistDemocracyIndex { get; set; }

        [Custom(Name = "Regime", Placeholder = "Regime Type (The Economist)")]
        public EconomistRegimeType? EconomistRegimeType { get; set; }

        [Custom(Name = "Expression", Placeholder = "Freedom of Expression Index (Varieties of Democracy)")]
        public double? FreedomExpressionIndex { get; set; }

        [Custom(Name = "Freedom", Placeholder = "Freedom in the World Score (Freedom House)")]
        public double? FreedomScore { get; set; }

        [Custom(Name = "Censorship", Placeholder = "Index on Censorship")]
        public double? CensorshipIndex { get; set; }

        [Custom(Name = "Happiness", Placeholder = "World Happiness Report")]
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

        [Custom(Name = "OECD", Placeholder = "The Organisation for Economic Co-operation and Development")]
        public bool OECD { get; set; } = false;

        [Custom(Name = "GDP (PPP)", Placeholder = "GDP (Gross Domestic Product) per capita - PPP")]
        public double? GDP_PPP { get; set; } //pra quem ganha e gasta na moeda interna

        [Custom(Name = "GDP (Nominal)", Placeholder = "GDP (Gross Domestic Product) per capita - Nominal")]
        public double? GDP_Nominal { get; set; } //pra quem ganha em moeda externa, investe em outro pais ou simplesmente quer comparar o pais a nivel global

        [Custom(Name = "Economic Freedom", Placeholder = "Index of Economic Freedom (The Heritage Foundation)")]
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

        [Custom(Name = "Safety", Placeholder = "Safety Index (Travel Safe - Abroad)")]
        public double? TsaSafetyIndex { get; set; }

        //todo: tsa tips
        [Custom(Name = "Safety", Placeholder = "Safety Index (Numbeo)")]
        public double? NumbeoSafetyIndex { get; set; }

        [Custom(Name = "Terrorism", Placeholder = "Global Terrorism Index (Vision of Humanity)")]
        public double? GlobalTerrorismIndex { get; set; }

        [Custom(Name = "Peace", Placeholder = "Global Peace Index (Vision of Humanity)")]
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

        [Custom(Name = "Sanitation / Water", Placeholder = "Sanitation & Drinking Water Score (Environmental Performance Index - Yale)")]
        public double? YaleWaterScore { get; set; }

        [Custom(Name = "Pollution", Placeholder = "Pollution Index (Numbeo)")]
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

        [Custom(Name = "Passport Index", Placeholder = "The Henley Passport Index (Henley & Partners)")]
        public int? VisaFree { get; set; }

        [Custom(Name = "Tourism Index", Placeholder = "Adventure Tourism Development Index (Adventure Travel Trade Association)")]
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

        [Custom(Name = "Conflict Level", Placeholder = "Current level of conflict in the region")]
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

        [Custom(Name = "Renting (City Center)", Placeholder = "Apartment (1 bedroom, City Center)")]
        public PriceRange? AptCityCenter { get; set; }

        [Custom(Name = "Renting (Outside of Center)", Placeholder = "Apartment (1 bedroom, Outside of Center)")]
        public PriceRange? AptOutsideCenter { get; set; }

        [Custom(Name = "Meal", Placeholder = "Meal (Inexpensive Restaurant)")]
        public PriceRange? Meal { get; set; }

        [Custom(Name = "Market (Western food)", Placeholder = "Market (2400 calories, Western food types)")]
        public PriceRange? MarketWestern { get; set; }

        [Custom(Name = "Market (Asian food)", Placeholder = "Market (2400 calories, Asian food types)")]
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