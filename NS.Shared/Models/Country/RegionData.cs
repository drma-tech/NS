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

        [Custom(Name = "Corruption", Placeholder = "Corruption Perceptions Index (Transparency International)", Description = "CorruptionScoreDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? CorruptionScore { get; set; }

        [Custom(Name = "HDI", Placeholder = "Human Development Index (Human Development Reports)", Description = "HDIDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? HDI { get; set; }

        [Custom(Name = "Democracy", Placeholder = "Quality of Democracy (Democracy Matrix)", Description = "DMDemocracyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? DMDemocracyIndex { get; set; }

        [Custom(Name = "Democracy", Placeholder = "Democracy Index (The Economist)", Description = "EconomistDemocracyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? EconomistDemocracyIndex { get; set; }

        [Custom(Name = "Expression", Placeholder = "Freedom of Expression Index (Varieties of Democracy)", Description = "FreedomExpressionIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? FreedomExpressionIndex { get; set; }

        [Custom(Name = "Freedom", Placeholder = "Freedom in the World Score (Freedom House)", Description = "FreedomScoreDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? FreedomScore { get; set; }

        [Custom(Name = "Censorship", Placeholder = "Index on Censorship", Description = "CensorshipIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? CensorshipIndex { get; set; }

        [Custom(Name = "Happiness", Placeholder = "World Happiness Report", Description = "HappinessIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
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

        [Custom(Name = "OECD", Placeholder = "The Organisation for Economic Co-operation and Development", Description = "OECDDesc", ResourceType = typeof(Resources.Enum.Field))]
        public bool OECD { get; set; } = false;

        [Custom(Name = "GDP (PPP)", Placeholder = "GDP (Gross Domestic Product) per capita - PPP", Description = "GDP_PPPDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? GDP_PPP { get; set; } //pra quem ganha e gasta na moeda interna

        [Custom(Name = "GDP (Nominal)", Placeholder = "GDP (Gross Domestic Product) per capita - Nominal", Description = "GDP_NominalDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? GDP_Nominal { get; set; } //pra quem ganha em moeda externa, investe em outro pais ou simplesmente quer comparar o pais a nivel global

        [Custom(Name = "Economic Freedom", Placeholder = "Index of Economic Freedom (The Heritage Foundation)", Description = "EconomicFreedomIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? EconomicFreedomIndex { get; set; }

        [Custom(Name = "Cashless Index", Placeholder = "Cash Index (FOREX)", Description = "CashlessIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
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

        [Custom(Name = "Safety", Placeholder = "Safety Index (Travel Safe - Abroad)", Description = "TsaSafetyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? TsaSafetyIndex { get; set; }

        //todo: tsa tips
        [Custom(Name = "Safety", Placeholder = "Safety Index (Numbeo)", Description = "NumbeoSafetyIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? NumbeoSafetyIndex { get; set; }

        [Custom(Name = "Terrorism", Placeholder = "Global Terrorism Index (Vision of Humanity)", Description = "GlobalTerrorismIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? GlobalTerrorismIndex { get; set; }

        [Custom(Name = "Peace", Placeholder = "Global Peace Index (Vision of Humanity)", Description = "GlobalPeaceIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
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

        [Custom(Name = "Sanitation / Water", Placeholder = "Sanitation & Drinking Water Score (Environmental Performance Index - Yale)", Description = "YaleWaterScoreDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? YaleWaterScore { get; set; }

        [Custom(Name = "Pollution", Placeholder = "Pollution Index (Numbeo)", Description = "NumbeoPollutionIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? NumbeoPollutionIndex { get; set; }

        [Custom(Name = "Air Quality", Placeholder = "World Air Quality Report (IQAir)", Description = "AirQualityDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? AirQuality { get; set; }

        [Custom(Name = "Health Care", Placeholder = "Health Care Index (CEOWORLD)", Description = "HealthCareIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? HealthCareIndex { get; set; }

        [Custom(Name = "Annual Temperature", Placeholder = "Average annual surface temperature (World Bank Group)", Description = "AnnualTemperatureDesc", ResourceType = typeof(Resources.Enum.Field))]
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

        [Custom(Name = "Passport Index", Placeholder = "The Henley Passport Index (Henley & Partners)", Description = "VisaFreeDesc", ResourceType = typeof(Resources.Enum.Field))]
        public int? VisaFree { get; set; }

        [Custom(Name = "Tourism Index", Placeholder = "Travel & Tourism Development Index (World Economic Forum)", Description = "TourismIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? TourismIndex { get; set; }

        [Custom(Name = "Air Connectivity Index", Placeholder = "Air Connectivity Index (International Air Transport Association)", Description = "AirConnectivityIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? AirConnectivityIndex { get; set; }

        [Custom(Name = "Sustainable Mobility Index", Placeholder = "Global Sustainable Mobility Index (GSMI - SuM4All)", Description = "SustainableMobilityIndexDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? SustainableMobilityIndex { get; set; }

        #endregion Scores

        #region Guide

        [Custom(Name = "Languages", Placeholder = "Languages", ResourceType = typeof(Resources.Enum.Field))]
        public HashSet<Language> Languages { get; set; } = [];

        [Custom(Name = "Risks", Placeholder = "Risks", ResourceType = typeof(Resources.Enum.Field))]
        public Risks? Risks { get; set; }

        [Custom(Name = "Tipping", Placeholder = "Tipping", ResourceType = typeof(Resources.Enum.Field))]
        public Tipping? Tipping { get; set; }

        [Custom(Name = "BroadbandSpeed", Placeholder = "Average broadband speed in Mbps", Description = "BroadbandSpeedDesc", ResourceType = typeof(Resources.Enum.Field))]
        public double? BroadbandSpeed { get; set; }

        [Custom(Name = "Taxes", Placeholder = "Taxes", ResourceType = typeof(Resources.Enum.Field))]
        public Taxes? Taxes { get; set; }

        [Custom(Name = "EmergencyNumbers", Placeholder = "EmergencyNumbers", ResourceType = typeof(Resources.Enum.Field))]
        public EmergencyNumbers? EmergencyNumbers { get; set; }

        [Custom(Name = "Currencies", Placeholder = "Currencies", ResourceType = typeof(Resources.Enum.Field))]
        public HashSet<Currency> Currencies { get; set; } = [];

        [Custom(Name = "Travel Requirements", Placeholder = "Travel Requirements", ResourceType = typeof(Resources.Enum.Field))]
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

        [Custom(Name = "Income", Placeholder = "IncomeDesc", ResourceType = typeof(Resources.Enum.Field))]
        public PriceRange? Income { get; set; }

        [Custom(Name = "AptCityCenter", Placeholder = "AptCityCenterDesc", ResourceType = typeof(Resources.Enum.Field))]
        public PriceRange? AptCityCenter { get; set; }

        [Custom(Name = "AptOutsideCenter", Placeholder = "AptOutsideCenterDesc", ResourceType = typeof(Resources.Enum.Field))]
        public PriceRange? AptOutsideCenter { get; set; }

        [Custom(Name = "Meal", Placeholder = "MealDesc", ResourceType = typeof(Resources.Enum.Field))]
        public PriceRange? Meal { get; set; }

        [Custom(Name = "MarketWestern", Placeholder = "MarketWesternDesc", ResourceType = typeof(Resources.Enum.Field))]
        public PriceRange? MarketWestern { get; set; }

        [Custom(Name = "MarketAsian", Placeholder = "MarketAsianDesc", ResourceType = typeof(Resources.Enum.Field))]
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
        [Custom(Name = "TransportTaxis", ResourceType = typeof(Resources.Enum.Field))]
        public Level? TransportTaxis { get; set; } = Level.Low;

        [Custom(Name = "Pickpockets", ResourceType = typeof(Resources.Enum.Field))]
        public Level? Pickpockets { get; set; } = Level.Medium;

        [Custom(Name = "NaturalDisasters", ResourceType = typeof(Resources.Enum.Field))]
        public Level? NaturalDisasters { get; set; } = Level.Low;

        [Custom(Name = "Mugging", ResourceType = typeof(Resources.Enum.Field))]
        public Level? Mugging { get; set; } = Level.High;

        [Custom(Name = "Terrorism", ResourceType = typeof(Resources.Enum.Field))]
        public Level? Terrorism { get; set; } = Level.Medium;

        [Custom(Name = "Scams", ResourceType = typeof(Resources.Enum.Field))]
        public Level? Scams { get; set; } = Level.High;

        [Custom(Name = "WomenTravelers", ResourceType = typeof(Resources.Enum.Field))]
        public Level? WomenTravelers { get; set; } = Level.Low;

        [Custom(Name = "TapWater", ResourceType = typeof(Resources.Enum.Field))]
        public Level? TapWater { get; set; } = Level.Low;
    }

    public class Tipping
    {
        [Custom(Name = "Restaurant", ResourceType = typeof(Resources.Enum.Field))]
        public string? Restaurant { get; set; }

        [Custom(Name = "Hotel", ResourceType = typeof(Resources.Enum.Field))]
        public string? Hotel { get; set; }

        [Custom(Name = "Driver", ResourceType = typeof(Resources.Enum.Field))]
        public string? Driver { get; set; }
    }

    public class Taxes
    {
        [Custom(Name = "Corporate", ResourceType = typeof(Resources.Enum.Field))]
        public string? Corporate { get; set; }

        [Custom(Name = "IncomeLowest", ResourceType = typeof(Resources.Enum.Field))]
        public string? IncomeLowest { get; set; }

        [Custom(Name = "IncomeHighest", ResourceType = typeof(Resources.Enum.Field))]
        public string? IncomeHighest { get; set; }

        [Custom(Name = "CapitalGains", ResourceType = typeof(Resources.Enum.Field))]
        public string? CapitalGains { get; set; }

        [Custom(Name = "Wealth", ResourceType = typeof(Resources.Enum.Field))]
        public string? Wealth { get; set; }

        [Custom(Name = "Property", ResourceType = typeof(Resources.Enum.Field))]
        public string? Property { get; set; }

        [Custom(Name = "InheritanceEstate", ResourceType = typeof(Resources.Enum.Field))]
        public string? InheritanceEstate { get; set; }

        [Custom(Name = "VATGSTSales", ResourceType = typeof(Resources.Enum.Field))]
        public string? VATGSTSales { get; set; }
    }

    public class EmergencyNumbers
    {
        [Custom(Name = "Police", ResourceType = typeof(Resources.Enum.Field))]
        public string? Police { get; set; }

        [Custom(Name = "Ambulance", ResourceType = typeof(Resources.Enum.Field))]
        public string? Ambulance { get; set; }

        [Custom(Name = "Fire", ResourceType = typeof(Resources.Enum.Field))]
        public string? Fire { get; set; }

        [Custom(Name = "Others", ResourceType = typeof(Resources.Enum.Field))]
        public string? Others { get; set; }
    }

    public class TravelRequirements
    {
        [Custom(Name = "Accommodation", ResourceType = typeof(Resources.Enum.Field))]
        public bool? Accommodation { get; set; }

        [Custom(Name = "HealthInsurance", ResourceType = typeof(Resources.Enum.Field))]
        public bool? HealthInsurance { get; set; }

        [Custom(Name = "ReturnTicket", ResourceType = typeof(Resources.Enum.Field))]
        public bool? ReturnTicket { get; set; }

        [Custom(Name = "YellowFever", ResourceType = typeof(Resources.Enum.Field))]
        public bool? YellowFever { get; set; }

        [Custom(Name = "MinimumFunds", ResourceType = typeof(Resources.Enum.Field))]
        public bool? MinimumFunds { get; set; }

        [Custom(Name = "Warning", ResourceType = typeof(Resources.Enum.Field))]
        public string? Warning { get; set; }
    }
}