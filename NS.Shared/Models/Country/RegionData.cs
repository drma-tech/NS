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

        public double? CorruptionScore { get; set; }
        public double? HDI { get; set; }
        public double? DMDemocracyIndex { get; set; }
        public double? EconomistDemocracyIndex { get; set; }
        public double? FreedomExpressionIndex { get; set; }
        public double? FreedomScore { get; set; }
        public double? CensorshipIndex { get; set; }
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

        public bool OECD { get; set; } = false;
        public double? GDP_PPP { get; set; } //pra quem ganha e gasta na moeda interna
        public double? GDP_Nominal { get; set; } //pra quem ganha em moeda externa, investe em outro pais ou simplesmente quer comparar o pais a nivel global
        public double? EconomicFreedomIndex { get; set; }
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

        public double? TsaSafetyIndex { get; set; }
        public double? NumbeoSafetyIndex { get; set; } //todo: tsa tips
        public double? GlobalTerrorismIndex { get; set; }
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

        public double? YaleWaterScore { get; set; }
        public double? NumbeoPollutionIndex { get; set; }
        public double? AirQuality { get; set; }
        public double? HealthCareIndex { get; set; }
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

        public int? VisaFree { get; set; }
        public double? TourismIndex { get; set; }
        public double? AirConnectivityIndex { get; set; }
        public double? SustainableMobilityIndex { get; set; }

        #endregion Scores

        #region Guide

        public HashSet<Language> Languages { get; set; } = [];
        public Risks? Risks { get; set; }
        public Tipping? Tipping { get; set; }
        public double? BroadbandSpeed { get; set; }
        public Taxes? Taxes { get; set; }
        public EmergencyNumbers? EmergencyNumbers { get; set; }
        public HashSet<Currency> Currencies { get; set; } = [];
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

        public PriceRange? Income { get; set; }
        public PriceRange? AptCityCenter { get; set; }
        public PriceRange? AptOutsideCenter { get; set; }
        public PriceRange? Meal { get; set; }
        public PriceRange? MarketWestern { get; set; }
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
        public Level? TransportTaxis { get; set; } = Level.Low;
        public Level? Pickpockets { get; set; } = Level.Medium;
        public Level? NaturalDisasters { get; set; } = Level.Low;
        public Level? Mugging { get; set; } = Level.High;
        public Level? Terrorism { get; set; } = Level.Medium;
        public Level? Scams { get; set; } = Level.High;
        public Level? WomenTravelers { get; set; } = Level.Low;
        public Level? TapWater { get; set; } = Level.Low;
    }

    public class Tipping
    {
        public string? Restaurant { get; set; }
        public string? Hotel { get; set; }
        public string? Driver { get; set; }
    }

    public class Taxes
    {
        public string? Corporate { get; set; }
        public string? IncomeLowest { get; set; }
        public string? IncomeHighest { get; set; }
        public string? CapitalGains { get; set; }
        public string? Wealth { get; set; }
        public string? Property { get; set; }
        public string? InheritanceEstate { get; set; }
        public string? VATGSTSales { get; set; }
    }

    public class EmergencyNumbers
    {
        public string? Police { get; set; }
        public string? Ambulance { get; set; }
        public string? Fire { get; set; }
        public string? Others { get; set; }
    }

    public class TravelRequirements
    {
        public bool? Accommodation { get; set; }
        public bool? HealthInsurance { get; set; }
        public bool? ReturnTicket { get; set; }
        public bool? YellowFever { get; set; }
        public bool? MinimumFunds { get; set; }
        public string? Warning { get; set; }
    }
}