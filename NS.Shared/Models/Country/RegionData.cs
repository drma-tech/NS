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
            if (values.Count == 0) throw new ArgumentException("Values list cannot be empty");

            int filledCount = values.Count(v => v.HasValue);

            // If count is 3 or less, all values must be populated
            if (values.Count <= 3 && filledCount != values.Count)
                return null;

            // If less than 50% is filled, it returns null
            if (filledCount <= values.Count / 2)
                return null;

            double sum = values.Where(v => v.HasValue).Sum(v => v!.Value);

            return Math.Round(sum / filledCount, 1);
        }

        public double? GetGlobalScore()
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

            return Math.Round(totalScore / categories, 1);
        }

        #region Scores

        /// <summary>
        /// Society and Government (100)
        /// </summary>

        public double? GetSocietyAndGovernmentScore()
        {
            var democracy = CalculateAverage([DMDemocracyIndex, EconomistDemocracyIndex]);

            var scores = new List<double?>
            {
                CorruptionScore,
                HDI,
                democracy,
                FreedomExpressionIndex,
                FreedomScore,
                CensorshipIndex,
                HappinessIndex
            };

            return CalculateAverage(scores);
        }

        [FieldSettings("Corruption", Placeholder = "Corruption Perceptions Index (Transparency International)", Description = "CorruptionScoreDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? CorruptionScore { get; set; }

        [FieldSettings("HDI", Placeholder = "Human Development Index (Human Development Reports)", Description = "HDIDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? HDI { get; set; }

        [FieldSettings("Democracy", Placeholder = "Quality of Democracy (Democracy Matrix)", Description = "DMDemocracyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? DMDemocracyIndex { get; set; }

        [FieldSettings("Democracy", Placeholder = "Democracy Index (The Economist)", Description = "EconomistDemocracyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? EconomistDemocracyIndex { get; set; }

        [FieldSettings("Expression", Placeholder = "Freedom of Expression Index (Varieties of Democracy)", Description = "FreedomExpressionIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? FreedomExpressionIndex { get; set; }

        [FieldSettings("Freedom", Placeholder = "Freedom in the World Score (Freedom House)", Description = "FreedomScoreDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? FreedomScore { get; set; }

        [FieldSettings("Censorship", Placeholder = "Index on Censorship", Description = "CensorshipIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? CensorshipIndex { get; set; }

        [FieldSettings("Happiness", Placeholder = "World Happiness Report", Description = "HappinessIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? HappinessIndex { get; set; }

        //LGBTQ Equality Index //https://www.equaldex.com/equality-index
        //Global Gender Gap Report 2025 //https://reports.weforum.org/docs/WEF_GGGR_2025.pdf (extracted to json)
        //RuleOfLawIndex(World Justice Project)
        //PressFreedomIndex //https://rsf.org/en/index
        //https://www.transparency.org/en/cpi/2023
        //https://fragilestatesindex.org/global-data/
        //https://worldpopulationreview.com/country-rankings/education-index-by-country
        //https://www.theglobaleconomy.com/rankings/wb_political_stability/
        //https://worldpopulationreview.com/country-rankings/political-stability-by-country

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
                GDP_Nominal,
                CashlessIndex
            };

            return CalculateAverage(scores);
        }

        [FieldSettings("OECD", Placeholder = "The Organisation for Economic Co-operation and Development", Description = "OECDDesc", ResourceType = typeof(Translations.Enum.Field))]
        public bool OECD { get; set; } = false;

        [FieldSettings("GDP (PPP)", Placeholder = "GDP (Gross Domestic Product) per capita - PPP", Description = "GDP_PPPDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? GDP_PPP { get; set; } //pra quem ganha e gasta na moeda interna

        [FieldSettings("GDP (Nominal)", Placeholder = "GDP (Gross Domestic Product) per capita - Nominal", Description = "GDP_NominalDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? GDP_Nominal { get; set; } //pra quem ganha em moeda externa, investe em outro pais ou simplesmente quer comparar o pais a nivel global

        [FieldSettings("Economic Freedom", Placeholder = "Index of Economic Freedom (The Heritage Foundation)", Description = "EconomicFreedomIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? EconomicFreedomIndex { get; set; }

        [FieldSettings("Cashless Index", Placeholder = "Cash Index (FOREX)", Description = "CashlessIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? CashlessIndex { get; set; }

        //GiniIndex(World Bank – inequality measure) //https://data.worldbank.org/indicator/SI.POV.GINI //https://worldpopulationreview.com/country-rankings/gini-coefficient-by-country
        //CompetitivenessIndex(World Economic Forum – Global Competitiveness Report) //https://en.wikipedia.org/wiki/WEF_Global_Competitiveness_Report
        //EaseOfDoingBusiness(World Bank — OBS: descontinuado em 2021, mas substituído pelo Business Ready Project) o novo, tem poucos paises, o velho nao tem um index (apenas um ranking)
        //https://www.numbeo.com/cost-of-living/

        ////https://wageindicator.org/salary/minimum-wage/minimum-wages-per-country
        ////https://countryeconomy.com/national-minimum-wage
        //public decimal? MinimalWage { get; set; }
        //todo: few countries. look for alternatives
        //https://worldpopulationreview.com/country-rankings/highest-taxed-countries

        /// <summary>
        /// Security and Peace (300)
        /// </summary>

        public double? GetSecurityAndPeaceScore()
        {
            var safety = CalculateAverage([TsaSafetyIndex, NumbeoSafetyIndex]);

            var scores = new List<double?>
            {
                safety,
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
            //var score2 = (int?)ConflictLevel;
            var score3 = GetRisksScore();

            //if (score1 == null || score2 == null || score3 == null) return null;
            if (score1 == null || score3 == null) return null;

            totalScore += score1.Value;
            //totalScore += score2.Value;
            totalScore += score3.Value;

            return Math.Round(totalScore / categories, 1);
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

            return Math.Round(totalScore / categories, 1);
        }

        [FieldSettings("Safety", Placeholder = "Safety Index (Travel Safe - Abroad)", Description = "TsaSafetyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? TsaSafetyIndex { get; set; }

        //todo: tsa tips
        [FieldSettings("Safety", Placeholder = "Safety Index (Numbeo)", Description = "NumbeoSafetyIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? NumbeoSafetyIndex { get; set; }

        [FieldSettings("Terrorism", Placeholder = "Global Terrorism Index (Vision of Humanity)", Description = "GlobalTerrorismIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? GlobalTerrorismIndex { get; set; }

        [FieldSettings("Peace", Placeholder = "Global Peace Index (Vision of Humanity)", Description = "GlobalPeaceIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? GlobalPeaceIndex { get; set; }

        //https://www.gallup.com/analytics/356996/gallup-global-safety-research-center.aspx //Gallup’s Law and Order Index

        /// <summary>
        /// Environment and Health (400)
        /// </summary>

        public double? GetEnvironmentAndHealthScore()
        {
            var scores = new List<double?>
            {
                YaleWaterScore,
                NumbeoPollutionIndex,
                AirQuality,
                HealthCareIndex,
                AnnualTemperature
            };

            return CalculateAverage(scores);
        }

        [FieldSettings("Sanitation / Water", Placeholder = "Sanitation & Drinking Water Score (Environmental Performance Index - Yale)", Description = "YaleWaterScoreDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? YaleWaterScore { get; set; }

        [FieldSettings("Pollution", Placeholder = "Pollution Index (Numbeo)", Description = "NumbeoPollutionIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? NumbeoPollutionIndex { get; set; }

        [FieldSettings("Air Quality", Placeholder = "World Air Quality Report (IQAir)", Description = "AirQualityDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? AirQuality { get; set; }

        [FieldSettings("Health Care", Placeholder = "Health Care Index (CEOWORLD)", Description = "HealthCareIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? HealthCareIndex { get; set; }

        [FieldSettings("Annual Temperature", Placeholder = "Average annual surface temperature (World Bank Group)", Description = "AnnualTemperatureDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? AnnualTemperature { get; set; }

        /// <summary>
        /// Mobility and Tourism (500)
        /// </summary>

        public double? GetMobilityAndTourismScore()
        {
            var scores = new List<double?>
            {
                CalculatePassportIndex(),
                TourismIndex,
                AirConnectivityIndex,
                SustainableMobilityIndex
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

        [FieldSettings("Passport Index", Placeholder = "The Henley Passport Index (Henley & Partners)", Description = "VisaFreeDesc", ResourceType = typeof(Translations.Enum.Field))]
        public int? VisaFree { get; set; }

        [FieldSettings("Tourism Index", Placeholder = "Travel & Tourism Development Index (World Economic Forum)", Description = "TourismIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? TourismIndex { get; set; }

        [FieldSettings("Air Connectivity Index", Placeholder = "Air Connectivity Index (International Air Transport Association)", Description = "AirConnectivityIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? AirConnectivityIndex { get; set; }

        [FieldSettings("Sustainable Mobility Index", Placeholder = "Global Sustainable Mobility Index (GSMI - SuM4All)", Description = "SustainableMobilityIndexDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? SustainableMobilityIndex { get; set; }

        #endregion Scores

        #region Guide

        [FieldSettings("Languages", Placeholder = "Languages", ResourceType = typeof(Translations.Enum.Field))]
        public HashSet<Language> Languages { get; set; } = [];

        [FieldSettings("Risks", Placeholder = "Risks", ResourceType = typeof(Translations.Enum.Field))]
        public Risks? Risks { get; set; }

        [FieldSettings("Tipping", Placeholder = "Tipping", ResourceType = typeof(Translations.Enum.Field))]
        public Tipping? Tipping { get; set; }

        [FieldSettings("BroadbandSpeed", Placeholder = "Average broadband speed in Mbps", Description = "BroadbandSpeedDesc", ResourceType = typeof(Translations.Enum.Field))]
        public double? BroadbandSpeed { get; set; }

        [FieldSettings("Taxes", Placeholder = "Taxes", ResourceType = typeof(Translations.Enum.Field))]
        public Taxes? Taxes { get; set; }

        [FieldSettings("EmergencyNumbers", Placeholder = "EmergencyNumbers", ResourceType = typeof(Translations.Enum.Field))]
        public EmergencyNumbers? EmergencyNumbers { get; set; }

        [FieldSettings("Currencies", Placeholder = "Currencies", ResourceType = typeof(Translations.Enum.Field))]
        public HashSet<Currency> Currencies { get; set; } = [];

        [FieldSettings("Travel Requirements", Placeholder = "Travel Requirements", ResourceType = typeof(Translations.Enum.Field))]
        public TravelRequirements? TravelRequirements { get; set; }

        public HashSet<ReligionData> Religions { get; set; } = [];

        public ElectricityData? Electricity { get; set; }

        //https://worldpopulationreview.com/country-rankings/immigration-by-country
        //https://worldpopulationreview.com/country-rankings/countries-with-universal-healthcare

        #endregion Guide

        #region Lifestyle

        public double? GetLifestyleScore()
        {
            var apt = CalculateAverage([AptCityCenter?.Score, AptOutsideCenter?.Score]);
            var food = CalculateAverage([Meal?.Score, MarketWestern?.Score, MarketAsian?.Score]);

            var scores = new List<double?>
            {
                Income?.Score,
                apt,
                food
            };

            return CalculateAverage(scores);
        }

        public double? GetExpensesScore()
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

        [FieldSettings("Income", Placeholder = "IncomeDesc", ResourceType = typeof(Translations.Enum.Field))]
        public PriceRange? Income { get; set; }

        [FieldSettings("AptCityCenter", Placeholder = "AptCityCenterDesc", ResourceType = typeof(Translations.Enum.Field))]
        public PriceRange? AptCityCenter { get; set; }

        [FieldSettings("AptOutsideCenter", Placeholder = "AptOutsideCenterDesc", ResourceType = typeof(Translations.Enum.Field))]
        public PriceRange? AptOutsideCenter { get; set; }

        [FieldSettings("Meal", Placeholder = "MealDesc", ResourceType = typeof(Translations.Enum.Field))]
        public PriceRange? Meal { get; set; }

        [FieldSettings("MarketWestern", Placeholder = "MarketWesternDesc", ResourceType = typeof(Translations.Enum.Field))]
        public PriceRange? MarketWestern { get; set; }

        [FieldSettings("MarketAsian", Placeholder = "MarketAsianDesc", ResourceType = typeof(Translations.Enum.Field))]
        public PriceRange? MarketAsian { get; set; }

        #endregion Lifestyle

        public HashSet<string> Cities { get; set; } = [];
    }

    public class PriceRange
    {
        public decimal? Avg { get; set; }
        public double? Score { get; set; }
    }

    public class ReligionData
    {
        public Religion Religion { get; set; }
        public double Percent { get; set; }
    }

    public class ElectricityData
    {
        public HashSet<string> Plugs { get; set; } = [];
        public HashSet<string> Voltages { get; set; } = [];
    }

    public class Risks
    {
        [FieldSettings("TransportTaxis", ResourceType = typeof(Translations.Enum.Field))]
        public Level? TransportTaxis { get; set; } = Level.Low;

        [FieldSettings("Pickpockets", ResourceType = typeof(Translations.Enum.Field))]
        public Level? Pickpockets { get; set; } = Level.Medium;

        [FieldSettings("NaturalDisasters", ResourceType = typeof(Translations.Enum.Field))]
        public Level? NaturalDisasters { get; set; } = Level.Low;

        [FieldSettings("Mugging", ResourceType = typeof(Translations.Enum.Field))]
        public Level? Mugging { get; set; } = Level.High;

        [FieldSettings("Terrorism", ResourceType = typeof(Translations.Enum.Field))]
        public Level? Terrorism { get; set; } = Level.Medium;

        [FieldSettings("Scams", ResourceType = typeof(Translations.Enum.Field))]
        public Level? Scams { get; set; } = Level.High;

        [FieldSettings("WomenTravelers", ResourceType = typeof(Translations.Enum.Field))]
        public Level? WomenTravelers { get; set; } = Level.Low;

        [FieldSettings("TapWater", ResourceType = typeof(Translations.Enum.Field))]
        public Level? TapWater { get; set; } = Level.Low;
    }

    public class Tipping
    {
        [FieldSettings("Restaurant", ResourceType = typeof(Translations.Enum.Field))]
        public string? Restaurant { get; set; }

        [FieldSettings("Hotel", ResourceType = typeof(Translations.Enum.Field))]
        public string? Hotel { get; set; }

        [FieldSettings("Driver", ResourceType = typeof(Translations.Enum.Field))]
        public string? Driver { get; set; }
    }

    public class Taxes
    {
        [FieldSettings("Corporate", ResourceType = typeof(Translations.Enum.Field))]
        public string? Corporate { get; set; }

        [FieldSettings("IncomeLowest", ResourceType = typeof(Translations.Enum.Field))]
        public string? IncomeLowest { get; set; }

        [FieldSettings("IncomeHighest", ResourceType = typeof(Translations.Enum.Field))]
        public string? IncomeHighest { get; set; }

        [FieldSettings("CapitalGains", ResourceType = typeof(Translations.Enum.Field))]
        public string? CapitalGains { get; set; }

        [FieldSettings("Wealth", ResourceType = typeof(Translations.Enum.Field))]
        public string? Wealth { get; set; }

        [FieldSettings("Property", ResourceType = typeof(Translations.Enum.Field))]
        public string? Property { get; set; }

        [FieldSettings("InheritanceEstate", ResourceType = typeof(Translations.Enum.Field))]
        public string? InheritanceEstate { get; set; }

        [FieldSettings("VATGSTSales", ResourceType = typeof(Translations.Enum.Field))]
        public string? VATGSTSales { get; set; }
    }

    public class EmergencyNumbers
    {
        [FieldSettings("Police", ResourceType = typeof(Translations.Enum.Field))]
        public string? Police { get; set; }

        [FieldSettings("Ambulance", ResourceType = typeof(Translations.Enum.Field))]
        public string? Ambulance { get; set; }

        [FieldSettings("Fire", ResourceType = typeof(Translations.Enum.Field))]
        public string? Fire { get; set; }

        [FieldSettings("Others", ResourceType = typeof(Translations.Enum.Field))]
        public string? Others { get; set; }
    }

    public class TravelRequirements
    {
        [FieldSettings("Accommodation", ResourceType = typeof(Translations.Enum.Field))]
        public bool? Accommodation { get; set; }

        [FieldSettings("HealthInsurance", ResourceType = typeof(Translations.Enum.Field))]
        public bool? HealthInsurance { get; set; }

        [FieldSettings("ReturnTicket", ResourceType = typeof(Translations.Enum.Field))]
        public bool? ReturnTicket { get; set; }

        [FieldSettings("YellowFever", ResourceType = typeof(Translations.Enum.Field))]
        public bool? YellowFever { get; set; }

        [FieldSettings("MinimumFunds", ResourceType = typeof(Translations.Enum.Field))]
        public bool? MinimumFunds { get; set; }

        [FieldSettings("Warning", ResourceType = typeof(Translations.Enum.Field))]
        public string? Warning { get; set; }
    }
}